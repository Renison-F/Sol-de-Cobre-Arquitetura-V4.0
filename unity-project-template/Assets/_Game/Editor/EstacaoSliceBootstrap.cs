using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using SolDeCobre.App;
using SolDeCobre.Gameplay;
using SolDeCobre.Narrative;
using SolDeCobre.Presentation;
using SolDeCobre.World;

namespace SolDeCobre.EditorTools
{
    // GRAYBOX do slice da Estação (LVL-01): pátio ~24 m, plataformas a 0/2/4 m, player
    // com motor/laço, Fiscal com núcleo laçável, dois inimigos, checkpoints e o
    // GameDirector. NÃO é o slice final (sem arte/áudio/HUD montado nem balanceamento).
    // O canvas da HUD é montado à parte (Assets/_Game/UI/README.md). Rodar no Unity URP.
    public static class EstacaoSliceBootstrap
    {
        const string Folder = "Assets/_Generated/SC_Slice_Estacao_V4";
        const string ScenePath = Folder + "/EstacaoSlice.unity";

        [MenuItem("Sol de Cobre/Bootstrap/Create Estação slice (graybox)")]
        public static void CreateSlice()
        {
            if (Directory.Exists(Folder))
                throw new InvalidOperationException("Slice folder already exists. Preserve it and choose a new version before regenerating.");
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).isDirty)
                    throw new InvalidOperationException("Save your open scenes before generating the slice.");
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) throw new InvalidOperationException("Create a Universal 3D project and resolve URP first.");

            var previous = EditorSceneManager.GetSceneManagerSetup();
            Directory.CreateDirectory(Folder);
            AssetDatabase.Refresh();
            try
            {
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var mat = new Material(shader) { name = "SliceSandstone" };
                mat.SetColor("_BaseColor", new Color(.56f, .37f, .21f));
                AssetDatabase.CreateAsset(mat, Folder + "/SliceSandstone.mat");

                // Controles e herói
                var controls = BuildControls();
                var hero = ScriptableObject.CreateInstance<HeroDefinition>();
                hero.heroId = "mara"; hero.canUseLasso = true;
                AssetDatabase.CreateAsset(hero, Folder + "/MaraHero.asset");

                // Pátio 24 m e três plataformas a 0/2/4 m (BOSS-01.md)
                Block("Floor", new Vector3(0, -.5f, 0), new Vector3(24, 1, 4), mat);
                Block("Platform_0", new Vector3(-8, 0.0f, 0), new Vector3(4, .5f, 3), mat);
                Block("Platform_2", new Vector3(0, 2.0f, 0), new Vector3(4, .5f, 3), mat);
                Block("Platform_4", new Vector3(8, 4.0f, 0), new Vector3(4, .5f, 3), mat);

                var player = BuildPlayer(hero, controls, mat);

                // O laço do player interage com o núcleo-core do chefe
                var lasso = player.GetComponent<LassoAbility>();

                // Fiscal (BOSS-01) com núcleo laçável
                var fiscalObj = new GameObject("BOSS-01_Fiscal_GRAYBOX");
                fiscalObj.transform.position = new Vector3(6, 0.5f, 0);
                var fiscalVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                fiscalVisual.name = "FiscalVisual"; fiscalVisual.transform.SetParent(fiscalObj.transform, false);
                fiscalVisual.transform.localScale = new Vector3(1.4f, 2.4f, 1.4f);
                fiscalVisual.GetComponent<Renderer>().sharedMaterial = mat;
                var ring = new GameObject("BOSS-01_Ring");
                ring.transform.SetParent(fiscalObj.transform, false);
                ring.transform.localPosition = new Vector3(0, 1.2f, 0);
                var ringSource = ring.AddComponent<AbilityTargetSource>();
                SetField(ringSource, "targetId", "BOSS-01-RING");
                SetField(ringSource, "missionObject", true);
                var boss = fiscalObj.AddComponent<FiscalBoss>();
                SetRef(boss, "lasso", lasso);

                // Dois inimigos simples (hover + patrol)
                var enemyA = BuildEnemy("ENM-01_Hover", EnemyFamily.Hover, "ENM-01", new Vector3(-4, 2.5f, 0), player.transform, mat);
                var enemyB = BuildEnemy("ENM-04_Patrol", EnemyFamily.Patrol, "ENM-04", new Vector3(2, 0.4f, 0), player.transform, mat);

                // Checkpoints
                MakeCheckpoint("CP-01-01", new Vector3(-8, 0.4f, 0));
                MakeCheckpoint("CP-01-02", new Vector3(0, 2.4f, 0));

                // Missão e diretor (composition root)
                var mission = new GameObject("MissionController").AddComponent<MissionController>();
                var directorObj = new GameObject("GameDirector");
                var director = directorObj.AddComponent<GameDirector>();
                SetRef(director, "mission", mission);
                SetRef(director, "boss", boss);
                SetRef(director, "input", player.GetComponent<PlayerInputBridge>());

                // Câmera 2.5D perspectiva
                var camObj = new GameObject("Main Camera"); camObj.tag = "MainCamera";
                var cam = camObj.AddComponent<Camera>();
                cam.orthographic = false; cam.fieldOfView = 35f;
                cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = new Color(.08f, .11f, .17f);
                camObj.transform.position = new Vector3(0, 2, -14);
                camObj.AddComponent<AudioListener>();
                SetRef(camObj.AddComponent<CameraFollow2_5D>(), "target", player.transform);

                var light = new GameObject("Key Light").AddComponent<Light>();
                light.type = LightType.Directional; light.intensity = 1.2f;
                light.transform.rotation = Quaternion.Euler(45, -35, 0);
                RenderSettings.ambientLight = new Color(.4f, .4f, .4f);

                if (!EditorSceneManager.SaveScene(scene, ScenePath)) throw new IOException("Scene save failed.");
                AssetDatabase.SaveAssets();
                Debug.Log("Estação slice (graybox) criado: " + ScenePath +
                    ". Montar o canvas da HUD e produzir arte/áudio/balanceamento à parte; não é o slice final.");
            }
            finally
            {
                if (previous.Length > 0) EditorSceneManager.RestoreSceneManagerSetup(previous);
            }
        }

        static InputActionAsset BuildControls()
        {
            var actions = GameplayControls.Build();
            try { File.WriteAllText(Folder + "/SliceControls.inputactions", actions.ToJson()); }
            finally { UnityEngine.Object.DestroyImmediate(actions); }
            AssetDatabase.ImportAsset(Folder + "/SliceControls.inputactions");
            var loaded = AssetDatabase.LoadAssetAtPath<InputActionAsset>(Folder + "/SliceControls.inputactions");
            if (loaded == null) throw new InvalidOperationException("Input Actions import failed.");
            return loaded;
        }

        static GameObject BuildPlayer(HeroDefinition hero, InputActionAsset controls, Material mat)
        {
            var player = new GameObject("MARA_GRAYBOX");
            player.transform.position = new Vector3(-10, .08f, 0);
            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.68f; controller.radius = .32f; controller.center = new Vector3(0, .84f, 0);
            controller.stepOffset = .25f; controller.skinWidth = .03f;
            var motor = player.AddComponent<PlayerMotor2_5D>();
            SetRef(motor, "hero", hero);
            var bridge = player.AddComponent<PlayerInputBridge>();
            SetRef(bridge, "actionTemplate", controls);
            var targeting = player.AddComponent<AbilityTargeting>();
            var lasso = player.AddComponent<LassoAbility>();
            SetRef(lasso, "targeting", targeting);
            SetRef(lasso, "aim", player.transform);
            var driver = player.AddComponent<AbilityDriver>();
            SetRef(driver, "input", bridge);
            SetRef(driver, "abilitySource", lasso);
            var visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visual.name = "GrayboxVisual"; UnityEngine.Object.DestroyImmediate(visual.GetComponent<Collider>());
            visual.transform.SetParent(player.transform, false);
            visual.transform.localPosition = new Vector3(0, .84f, 0);
            visual.transform.localScale = new Vector3(.64f, .84f, .64f);
            visual.GetComponent<Renderer>().sharedMaterial = mat;
            return player;
        }

        static GameObject BuildEnemy(string name, EnemyFamily family, string id, Vector3 pos, Transform target, Material mat)
        {
            var e = GameObject.CreatePrimitive(PrimitiveType.Cube);
            e.name = name; e.transform.position = pos; e.transform.localScale = new Vector3(.6f, .6f, .6f);
            e.GetComponent<Renderer>().sharedMaterial = mat;
            var enemy = e.AddComponent<CommonEnemy>();
            SetField(enemy, "enemyId", id);
            SetField(enemy, "family", (int)family);
            SetRef(enemy, "target", target);
            return e;
        }

        static void MakeCheckpoint(string id, Vector3 pos)
        {
            var cp = new GameObject("Checkpoint_" + id);
            cp.transform.position = pos;
            var marker = cp.AddComponent<CheckpointMarker>();
            SetField(marker, "checkpointId", id);
        }

        static void Block(string name, Vector3 position, Vector3 scale, Material mat)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube); block.name = name;
            block.transform.position = position; block.transform.localScale = scale;
            block.GetComponent<Renderer>().sharedMaterial = mat;
        }

        // Fiação de referências e campos serializados privados (como no ProjectBootstrap).
        static void SetRef(UnityEngine.Object target, string field, UnityEngine.Object value)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(field);
            if (p == null) throw new InvalidOperationException("Missing serialized field: " + field + " em " + target.GetType().Name);
            p.objectReferenceValue = value; so.ApplyModifiedPropertiesWithoutUndo();
        }
        static void SetField(UnityEngine.Object target, string field, string value)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(field);
            if (p == null) throw new InvalidOperationException("Missing serialized field: " + field);
            p.stringValue = value; so.ApplyModifiedPropertiesWithoutUndo();
        }
        static void SetField(UnityEngine.Object target, string field, bool value)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(field);
            if (p == null) throw new InvalidOperationException("Missing serialized field: " + field);
            p.boolValue = value; so.ApplyModifiedPropertiesWithoutUndo();
        }
        static void SetField(UnityEngine.Object target, string field, int value)
        {
            var so = new SerializedObject(target);
            var p = so.FindProperty(field);
            if (p == null) throw new InvalidOperationException("Missing serialized field: " + field);
            if (p.propertyType == SerializedPropertyType.Enum) p.enumValueIndex = value; else p.intValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
