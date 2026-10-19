using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using SolDeCobre.Gameplay;
using SolDeCobre.Presentation;

namespace SolDeCobre.EditorTools
{
    // A movement laboratory, NOT the Station chapter or a commercially gated game.
    public static class ProjectBootstrap
    {
        const string Folder = "Assets/_Generated/SC_Lab_V4";
        const string ScenePath = Folder + "/MovementLab.unity";

        [MenuItem("Sol de Cobre/Bootstrap/Create movement laboratory")]
        public static void CreateLab()
        {
            if (Directory.Exists(Folder))
                throw new InvalidOperationException("Lab folder already exists. Preserve it and choose a new version before regenerating.");
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).isDirty)
                    throw new InvalidOperationException("Save your open scenes before generating the lab.");
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) throw new InvalidOperationException("Create a Universal 3D project and resolve URP first.");

            var previous = EditorSceneManager.GetSceneManagerSetup();
            Directory.CreateDirectory(Folder);
            AssetDatabase.Refresh();
            try
            {
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var material = new Material(shader) { name = "LabSandstone" };
                material.SetColor("_BaseColor", new Color(.56f, .37f, .21f));
                AssetDatabase.CreateAsset(material, Folder + "/LabSandstone.mat");

                // Mapa de controles central (teclado/gamepad/touch por ação).
                var actions = GameplayControls.Build();
                try { File.WriteAllText(Folder + "/LabControls.inputactions", actions.ToJson()); }
                finally { UnityEngine.Object.DestroyImmediate(actions); }
                AssetDatabase.ImportAsset(Folder + "/LabControls.inputactions");
                var controls = AssetDatabase.LoadAssetAtPath<InputActionAsset>(Folder + "/LabControls.inputactions");
                if (controls == null) throw new InvalidOperationException("Input Actions import failed.");
                var hero = ScriptableObject.CreateInstance<HeroDefinition>();
                hero.heroId = "mara"; hero.canUseLasso = false;
                AssetDatabase.CreateAsset(hero, Folder + "/LabHero.asset");

                var player = new GameObject("LAB_Mara_Capsule_NOT_FINAL");
                player.transform.position = new Vector3(0, .08f, 0);
                var controller = player.AddComponent<CharacterController>();
                controller.height = 1.68f; controller.radius = .32f;
                controller.center = new Vector3(0, .84f, 0);
                controller.stepOffset = .25f; controller.skinWidth = .03f;
                var motor = player.AddComponent<PlayerMotor2_5D>();
                SetReference(motor, "hero", hero);
                var input = player.AddComponent<PlayerInputBridge>();
                SetReference(input, "actionTemplate", controls);
                var visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                visual.name = "GrayboxVisual";
                UnityEngine.Object.DestroyImmediate(visual.GetComponent<Collider>());
                visual.transform.SetParent(player.transform, false);
                visual.transform.localPosition = new Vector3(0, .84f, 0);
                visual.transform.localScale = new Vector3(.64f, .84f, .64f);
                visual.GetComponent<Renderer>().sharedMaterial = material;
                PrefabUtility.SaveAsPrefabAsset(player, Folder + "/LabPlayer.prefab", out bool saved);
                if (!saved) throw new IOException("Prefab save failed.");

                Block("Floor", new Vector3(12, -.5f, 0), new Vector3(120, 1, 4), material);
                Block("Step_1", new Vector3(5, .4f, 0), new Vector3(2, .8f, 3), material);
                Block("Step_2", new Vector3(9, .8f, 0), new Vector3(2, 1.6f, 3), material);
                Block("Step_3", new Vector3(13, 1.2f, 0), new Vector3(2, 2.4f, 3), material);
                var cameraObject = new GameObject("Main Camera"); cameraObject.tag = "MainCamera";
                var camera = cameraObject.AddComponent<Camera>();
                // Perspectiva FOV vertical 35° (camera-2-5d.md); distância em Z precisa
                // ser calibrada para ~18 m de largura no aspect de referência.
                camera.orthographic = false; camera.fieldOfView = 35f;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(.08f, .11f, .17f);
                cameraObject.transform.position = new Vector3(0, 1, -11);
                cameraObject.AddComponent<AudioListener>();
                SetReference(cameraObject.AddComponent<CameraFollow2_5D>(), "target", player.transform);
                var light = new GameObject("Key Light").AddComponent<Light>();
                light.type = LightType.Directional; light.intensity = 1.2f;
                light.transform.rotation = Quaternion.Euler(45, -35, 0);
                RenderSettings.ambientLight = new Color(.4f, .4f, .4f);
                if (!EditorSceneManager.SaveScene(scene, ScenePath)) throw new IOException("Scene save failed.");
                AssetDatabase.SaveAssets();
                Debug.Log("Movement laboratory created: " + ScenePath + ". Keyboard/gamepad only; not the vertical slice.");
            }
            finally
            {
                if (previous.Length > 0) EditorSceneManager.RestoreSceneManagerSetup(previous);
            }
        }

        static void SetReference(UnityEngine.Object target, string field, UnityEngine.Object value)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(field);
            if (property == null) throw new InvalidOperationException("Missing serialized field: " + field);
            property.objectReferenceValue = value; serialized.ApplyModifiedPropertiesWithoutUndo();
        }
        static void Block(string name, Vector3 position, Vector3 scale, Material material)
        {
            var block = GameObject.CreatePrimitive(PrimitiveType.Cube); block.name = name;
            block.transform.position = position; block.transform.localScale = scale;
            block.GetComponent<Renderer>().sharedMaterial = material;
        }

        public static void BuildLabAndroid()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                throw new InvalidOperationException("Start the Editor with -buildTarget Android and install Android support.");
            if (!File.Exists(ScenePath)) throw new InvalidOperationException("Create the lab first.");
            const string output = "Builds/MovementLab/SC-Lab.apk";
            if (File.Exists(output)) throw new IOException("Lab APK already exists; archive it before rebuilding.");
            Directory.CreateDirectory("Builds/MovementLab");
            bool previous = EditorUserBuildSettings.buildAppBundle;
            try
            {
                EditorUserBuildSettings.buildAppBundle = false;
                var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                    scenes = new[] { ScenePath }, locationPathName = output,
                    target = BuildTarget.Android, options = BuildOptions.Development
                });
                if (report.summary.result != BuildResult.Succeeded)
                    throw new InvalidOperationException("Laboratory build failed: " + report.summary.result);
            }
            finally { EditorUserBuildSettings.buildAppBundle = previous; }
        }
    }
}
