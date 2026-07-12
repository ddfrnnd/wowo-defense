using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[InitializeOnLoad]
public static class SceneRebuilder
{
    static SceneRebuilder()
    {
        // Delay execution until Unity is fully loaded and ready
        EditorApplication.delayCall += AutoRebuild;
    }

    [MenuItem("Tools/Rebuild Scenes (Keep Original Layout)")]
    public static void ManualRebuild()
    {
        // Delete the flag file to force rebuilding
        string flagPath = Path.Combine(Application.dataPath, "../Library/TropicalScenesRebuilt.txt");
        if (File.Exists(flagPath))
        {
            File.Delete(flagPath);
        }
        Rebuild();
    }

    private static void AutoRebuild()
    {
        string flagPath = Path.Combine(Application.dataPath, "../Library/TropicalScenesRebuilt.txt");
        if (File.Exists(flagPath))
        {
            return;
        }

        Rebuild();
    }

    private static void Rebuild()
    {
        string[] targets = new string[] {
            "Assets/UTS/Level1.unity",
            "Assets/UTS/Level2.unity"
        };

        // Load prefab assets
        GameObject templePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UTS/LowPolyShooterPack/Prefabs/Buildings/Building07.prefab");
        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/P_LPSP_FP_CH.prefab");
        GameObject canvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UTS/Prefabs/GameplayCanvas.prefab");
        GameObject uiManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UTS/Prefabs/GameUIManager.prefab");
        GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UTS/Enemy.prefab");

        if (templePrefab == null || playerPrefab == null || canvasPrefab == null || uiManagerPrefab == null || enemyPrefab == null)
        {
            Debug.LogError("[SceneRebuilder] One or more required prefabs are missing! Please check paths.");
            return;
        }

        foreach (string targetScenePath in targets)
        {
            if (!File.Exists(targetScenePath))
            {
                Debug.LogError("[SceneRebuilder] Target scene not found at " + targetScenePath);
                continue;
            }

            // 1. Open the existing scene in-place (DO NOT overwrite it with a demo scene)
            var scene = EditorSceneManager.OpenScene(targetScenePath);
            Debug.Log("[SceneRebuilder] Rebuilding original layout for scene: " + targetScenePath);

            // 2. Clean up old elements to avoid duplicates or conflicts
            // Destroy old player controllers and input managers
            GameObject oldPlayer = GameObject.Find("Player");
            if (oldPlayer != null) Undo.DestroyObjectImmediate(oldPlayer);

            GameObject oldInput = GameObject.Find("PlayerInputHandler");
            if (oldInput != null) Undo.DestroyObjectImmediate(oldInput);

            GameObject oldCanvas = GameObject.Find("Canvas");
            if (oldCanvas != null) Undo.DestroyObjectImmediate(oldCanvas);

            GameObject oldGameplayCanvas = GameObject.Find("GameplayCanvas");
            if (oldGameplayCanvas != null) Undo.DestroyObjectImmediate(oldGameplayCanvas);

            GameObject oldUIManager = GameObject.Find("GameUIManager");
            if (oldUIManager != null) Undo.DestroyObjectImmediate(oldUIManager);

            GameObject oldGameManager = GameObject.Find("GameManager");
            if (oldGameManager != null) Undo.DestroyObjectImmediate(oldGameManager);

            GameObject oldMobileBuilder = GameObject.Find("MobileControlsBuilder");
            if (oldMobileBuilder != null) Undo.DestroyObjectImmediate(oldMobileBuilder);

            GameObject oldMobileControls = GameObject.Find("MobileControls");
            if (oldMobileControls != null) Undo.DestroyObjectImmediate(oldMobileControls);

            GameObject oldSpawnPoints = GameObject.Find("SpawnPoints");
            if (oldSpawnPoints != null) Undo.DestroyObjectImmediate(oldSpawnPoints);

            // Destroy any individual SpawnPoint GameObjects
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>(true);
            foreach (GameObject obj in allObjects)
            {
                if (obj != null && obj.name.StartsWith("SpawnPoint_"))
                {
                    Undo.DestroyObjectImmediate(obj);
                }
            }

            // Destroy old generated village houses to prevent duplicate accumulation
            GameObject oldVillage = GameObject.Find("Village_Buildings");
            if (oldVillage != null) Undo.DestroyObjectImmediate(oldVillage);

            // Clean up any remaining cameras in the scene to avoid overlay/rendering issues
            Camera[] allCams = Object.FindObjectsOfType<Camera>(true);
            foreach (Camera c in allCams)
            {
                if (c != null) Undo.DestroyObjectImmediate(c.gameObject);
            }

            // Clean up any remaining UI canvases in the scene
            Canvas[] allCanvases = Object.FindObjectsOfType<Canvas>(true);
            foreach (Canvas c in allCanvases)
            {
                if (c != null) Undo.DestroyObjectImmediate(c.gameObject);
            }

            // 3. Locate or instantiate Temple (Kuil)
            GameObject templeInstance = GameObject.Find("Kuil");
            if (templeInstance == null) templeInstance = GameObject.FindGameObjectWithTag("Kuil");

            // If not found in scene, instantiate it at the center of the village layout
            if (templeInstance == null)
            {
                templeInstance = (GameObject)PrefabUtility.InstantiatePrefab(templePrefab);
                templeInstance.name = "Kuil";
                templeInstance.tag = "Kuil";
                templeInstance.transform.position = new Vector3(-2.01f, -0.06f, 48.44f);
                templeInstance.transform.rotation = Quaternion.identity;
            }

            // Ensure the Kuil has exactly one BoxCollider (configured as a trigger for defense checks)
            BoxCollider[] oldColliders = templeInstance.GetComponents<BoxCollider>();
            foreach (BoxCollider col in oldColliders)
            {
                Undo.DestroyObjectImmediate(col);
            }
            BoxCollider boxCollider = templeInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.center = new Vector3(0f, 3.8f, 0f);
            boxCollider.size = new Vector3(4.7f, 7.6f, 4.7f);

            // Set up TempleHealth component
            TempleHealth templeHealth = templeInstance.GetComponent<TempleHealth>();
            if (templeHealth == null) templeHealth = templeInstance.AddComponent<TempleHealth>();
            templeHealth.maxHealth = 100;

            Vector3 templePos = templeInstance.transform.position;

            // 4. Instantiate Player slightly in front of the temple, facing it
            GameObject playerInstance = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
            playerInstance.name = "Player";
            playerInstance.transform.position = templePos + new Vector3(0f, 1.2f, -5.5f);
            playerInstance.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f)); // Face temple

            // 5. Instantiate UI Elements
            GameObject canvasInstance = (GameObject)PrefabUtility.InstantiatePrefab(canvasPrefab);
            canvasInstance.name = "GameplayCanvas";

            GameObject uiManagerInstance = (GameObject)PrefabUtility.InstantiatePrefab(uiManagerPrefab);
            uiManagerInstance.name = "GameUIManager";

            // Link TempleHealth UI references
            Slider hpBar = canvasInstance.GetComponentInChildren<Slider>(true);
            templeHealth.healthBar = hpBar;
            templeHealth.gameUI = uiManagerInstance.GetComponent<GameUIManager>();

            // 6. Create GameManager and WaveManager
            GameObject gameManagerObj = new GameObject("GameManager");
            WaveManager waveManager = gameManagerObj.AddComponent<WaveManager>();
            waveManager.totalWaves = 3;
            waveManager.baseEnemyCount = 8;
            waveManager.enemyIncreasePerWave = 4;
            waveManager.timeBetweenWaves = 5f;
            waveManager.spawnDelay = 0.8f;
            waveManager.speedIncreasePerWave = 0.4f;
            waveManager.gameUI = uiManagerInstance.GetComponent<GameUIManager>();
            waveManager.enemyPrefab = enemyPrefab;
            waveManager.enemyParent = gameManagerObj.transform;

            // 7. Generate 8 spawn points in a circle around the temple (radius 40f)
            // Match the temple ground Y level directly to prevent flying/falling issues
            Transform[] spawnPoints = new Transform[8];
            for (int i = 0; i < 8; i++)
            {
                float angle = i * Mathf.PI / 4f;
                Vector3 spawnPos = templePos + new Vector3(Mathf.Cos(angle) * 40f, 0f, Mathf.Sin(angle) * 40f);
                spawnPos.y = templePos.y; // Lock height to flat ground level

                GameObject spawnPointObj = new GameObject("SpawnPoint_" + i);
                spawnPointObj.transform.position = spawnPos;
                spawnPoints[i] = spawnPointObj.transform;
            }
            waveManager.spawnPoints = spawnPoints;

            // 8. Spawn 10 village houses in a ring around the temple to make the village dense and crowded (radius 27f)
            GameObject villageParent = new GameObject("Village_Buildings");
            int buildingCount = 10;
            float villageRadius = 27f;
            for (int i = 0; i < buildingCount; i++)
            {
                float angle = i * 2f * Mathf.PI / buildingCount;
                Vector3 housePos = templePos + new Vector3(Mathf.Cos(angle) * villageRadius, 0f, Mathf.Sin(angle) * villageRadius);
                housePos.y = templePos.y; // Set flat on ground

                // Dynamically load building prefabs 1-6
                int prefabIndex = (i % 6) + 1;
                string prefabPath = string.Format("Assets/UTS/LowPolyShooterPack/Prefabs/Buildings/Building0{0}.prefab", prefabIndex);
                GameObject housePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (housePrefab != null)
                {
                    GameObject house = (GameObject)PrefabUtility.InstantiatePrefab(housePrefab);
                    house.name = "VillageHouse_" + i;
                    house.transform.SetParent(villageParent.transform);
                    house.transform.position = housePos;

                    // Rotate house to face the central temple
                    Vector3 lookDir = templePos - housePos;
                    lookDir.y = 0f;
                    house.transform.rotation = Quaternion.LookRotation(lookDir);
                }
            }

            // 9. MobileControlsBuilder is deprecated as mobile controls are now built directly into the Canvas prefab.
            // No runtime builder required.

            // 9.5. Mark environment as Navigation Static and bake NavMesh
            var stack = new System.Collections.Generic.Stack<GameObject>();
            foreach (GameObject rootGo in scene.GetRootGameObjects())
            {
                stack.Push(rootGo);
            }

            while (stack.Count > 0)
            {
                GameObject go = stack.Pop();
                if (go == null) continue;

                // Skip player, lights, post-processing, canvas, spawn points, and manager objects
                if (go.name == "Player" || go.name == "GameManager" || go.name == "GameUIManager" || 
                    go.name == "GameplayCanvas" || go.name.StartsWith("Directional Light") || 
                    go.name.StartsWith("SpawnPoint_") || go.name == "EventSystem" || go.name == "Post-Processing")
                {
                    continue;
                }

                GameObjectUtility.SetStaticEditorFlags(go, StaticEditorFlags.NavigationStatic);

                for (int c = 0; c < go.transform.childCount; c++)
                {
                    stack.Push(go.transform.GetChild(c).gameObject);
                }
            }

            UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

            // 10. Save and mark dirty
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[SceneRebuilder] Successfully rebuilt scene and baked NavMesh for: " + targetScenePath);
        }

        // Create the flag file to avoid repeated runs
        string flagPath = Path.Combine(Application.dataPath, "../Library/TropicalScenesRebuilt.txt");
        File.WriteAllText(flagPath, "Rebuilt");

        // Refresh database
        AssetDatabase.Refresh();
        Debug.Log("[SceneRebuilder] All scenes rebuilt and configured with original village environments!");
    }
}
