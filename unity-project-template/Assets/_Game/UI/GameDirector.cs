using System;
using System.Collections.Generic;
using UnityEngine;
using SolDeCobre.Core;
using SolDeCobre.Gameplay;
using SolDeCobre.Hud;
using SolDeCobre.Narrative;
using SolDeCobre.Progression;
using SolDeCobre.UI;
using SolDeCobre.World;

namespace SolDeCobre.App
{
    // Composition root do slice: amarra sinais de domínio, máquina de fases, save de
    // três slots, missão ordenada, HUD, áudio e o Fiscal. Nenhuma lógica de regra vive
    // aqui — tudo delega às peças já testadas. Monta-se na cena e recebe as referências.
    public sealed class GameDirector : MonoBehaviour
    {
        [SerializeField] HudController hud;
        [SerializeField] MissionController mission;
        [SerializeField] FiscalBoss boss;
        [SerializeField] AccessibilityController accessibility;
        [SerializeField] PlayerInputBridge input;
        [SerializeField] Transform heroTransform;                 // posição do herói (dano/checkpoint)
        [SerializeField] CommonEnemy[] enemies = Array.Empty<CommonEnemy>();
        [SerializeField] CheckpointMarker[] checkpoints = Array.Empty<CheckpointMarker>();
        [SerializeField] int slot = 1;
        [SerializeField] bool autoStartOnPlay = true;             // inicia a sessão no Play
        [SerializeField] float hitRange = 1.2f;                   // alcance do dano por contato
        [SerializeField] float checkpointRange = 1.5f;            // raio para ativar checkpoint

        readonly GameSignals signals = new GameSignals();
        readonly GamePhaseMachine phase = new GamePhaseMachine();
        readonly InputDeviceTracker device = new InputDeviceTracker();
        readonly HashSet<IThreatSource> threatHitThisWindow = new HashSet<IThreatSource>();
        readonly HashSet<string> checkpointsTriggered = new HashSet<string>();
        IThreatSource[] threats = Array.Empty<IThreatSource>();
        SaveSystem saves;
        SessionState session;
        HeroVitality hero;

        public GamePhase Phase => phase.Current;
        public GameSignals Signals => signals;

        void Awake()
        {
            saves = new SaveSystem();
            hero = new HeroVitality(5f);
            if (mission != null) mission.Bind(signals);
            if (boss != null) boss.Bind(signals);
            signals.CheckpointReached += OnCheckpoint;
            signals.QuestFlagGranted += OnFlagGranted;
            if (input != null) input.PauseToggled += TogglePause;
            BuildThreats();
        }

        void Start()
        {
            if (autoStartOnPlay) StartSession();
        }

        // Coleta as fontes de ameaça (chefe + inimigos) para o aplicador de dano.
        void BuildThreats()
        {
            var list = new List<IThreatSource>();
            if (boss != null) list.Add(boss);
            foreach (var e in enemies) if (e != null) list.Add(e);
            threats = list.ToArray();
        }
        void OnDestroy()
        {
            signals.CheckpointReached -= OnCheckpoint;
            signals.QuestFlagGranted -= OnFlagGranted;
            if (input != null) input.PauseToggled -= TogglePause;
        }

        // Fluxo do comprador: menu -> slot -> capítulo. Save inválido vai à recuperação.
        public bool StartSession()
        {
            var result = saves.Read(slot);
            switch (result.Status)
            {
                case SaveLoadStatus.Missing:
                    session = new SessionState { Slot = slot };
                    break;
                case SaveLoadStatus.Loaded:
                case SaveLoadStatus.RecoveredBackup:
                    session = SessionState.FromSave(result.Data);
                    break;
                case SaveLoadStatus.Migrated:
                    session = SessionState.FromSave(result.Data);
                    saves.Write(session.ToSave(GameClock.NowUnix())); // promove a cópia v3 migrada
                    break;
                default: // CorruptPreserved / UnsupportedVersion -> tela de recuperação
                    Debug.LogWarning("Save do slot " + slot + " exige recuperação: " + result.Status);
                    return false;
            }
            RestoreFromSession();
            // Caminho até Playing a partir de Boot/Menu/Results; se já em Playing/Paused
            // (reentrada/reinício), mantém. O ESTADO FINAL decide o retorno, não a última aresta.
            if (phase.Current == GamePhase.Boot) phase.TryTransition(GamePhase.Menu);
            phase.TryTransition(GamePhase.Loading);
            phase.TryTransition(GamePhase.Playing);
            bool playing = phase.Current == GamePhase.Playing;
            if (playing && boss != null) boss.Begin(); // inicia o encontro (no-op se já ativo)
            return playing;
        }

        void RestoreFromSession()
        {
            hero.Reset();
            // rewardGranted:true => se a missão já está completa no save, o reward já foi
            // concedido antes (Restore faz rewardGranted && Completed); evita re-conceder.
            if (mission != null) mission.Restore(session.Flags, rewardGranted: true);
            if (accessibility != null) accessibility.LoadFrom(session.Options);
            foreach (var e in enemies) if (e != null) e.ResetToNeutral();
        }

        // Dano ao herói (chamado por uma hitbox na janela Active do chefe/inimigo).
        public void DamageHero(float amount)
        {
            if (phase.Current != GamePhase.Playing) return;
            if (hero.TakeDamage(amount)) RestoreCheckpoint(); // morte -> volta ao checkpoint
        }

        void RestoreCheckpoint()
        {
            var result = saves.Read(slot);
            if (result.Data != null) session = SessionState.FromSave(result.Data);
            RestoreFromSession();
            if (boss != null) boss.RestoreCheckpoint();
            threatHitThisWindow.Clear(); // após respawn, ameaças podem acertar de novo
        }

        void OnCheckpoint(string checkpointId)
        {
            if (session == null) return;
            session.Checkpoint = checkpointId;
            session.NextRevision();
            if (!saves.Write(session.ToSave(GameClock.NowUnix())))
                Debug.LogWarning("Falha ao gravar checkpoint; save anterior preservado."); // sem sucesso fictício
        }

        void OnFlagGranted(string flag) => session?.GrantFlag(flag);

        public void TogglePause()
        {
            if (phase.TryTransition(GamePhase.Paused)) SetFrozen(true);
            else if (phase.Current == GamePhase.Paused && phase.TryTransition(GamePhase.Playing)) SetFrozen(false);
        }
        void SetFrozen(bool frozen)
        {
            if (boss != null) boss.SetPaused(frozen);
            foreach (var e in enemies) if (e != null) e.Freeze(frozen);
        }

        void Update()
        {
            ApplyThreatDamage();
            CheckCheckpoints();
            UpdateHud();
        }

        // Dano demonstrável: só na janela Active da ameaça e por alcance; um acerto por
        // janela (rearma quando a janela fecha). Fora disso, nenhum contato fere.
        void ApplyThreatDamage()
        {
            if (heroTransform == null || phase.Current != GamePhase.Playing) return;
            foreach (var t in threats)
            {
                if (t == null) continue;
                if (!t.DamageActive) { threatHitThisWindow.Remove(t); continue; }
                if (threatHitThisWindow.Contains(t) || t.Origin == null) continue;
                if (Vector3.Distance(t.Origin.position, heroTransform.position) <= hitRange)
                {
                    threatHitThisWindow.Add(t);
                    DamageHero(t.DamageAmount);
                }
            }
        }

        // Checkpoint por proximidade: ativa uma vez e grava o save.
        void CheckCheckpoints()
        {
            if (heroTransform == null || phase.Current != GamePhase.Playing) return;
            foreach (var cp in checkpoints)
            {
                if (cp == null || string.IsNullOrEmpty(cp.CheckpointId)) continue;
                if (checkpointsTriggered.Contains(cp.CheckpointId)) continue;
                if (Vector3.Distance(cp.transform.position, heroTransform.position) <= checkpointRange)
                {
                    checkpointsTriggered.Add(cp.CheckpointId);
                    cp.Activate(signals); // -> OnCheckpoint grava o save
                }
            }
        }

        void UpdateHud()
        {
            if (hud == null) return;
            var level = boss != null ? ThreatPresentation.FromBossPhase(boss.Phase) : ThreatLevel.None;
            bool reduced = accessibility != null && accessibility.ReducedFlashes;
            var threat = ThreatPresentation.For(level, reduced);
            var snapshot = HudModel.Build(hero, mission != null ? mission.NextObjective : "",
                device, threat, hintAvailable: false, accessibility != null ? accessibility.Preferences : null);
            hud.SetSnapshot(snapshot);
        }
    }
}
