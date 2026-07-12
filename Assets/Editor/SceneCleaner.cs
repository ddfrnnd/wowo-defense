using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class SceneCleaner
{
    static SceneCleaner()
    {
        // Delay execution until Unity is fully loaded and ready
        EditorApplication.delayCall += CleanScenes;
    }

    private static void CleanScenes()
    {
        string cleanedFlag = Path.Combine(Application.dataPath, "../Library/ScenesCleaned.txt");
        if (File.Exists(cleanedFlag))
        {
            return;
        }

        string[] scenes = new string[] {
            "Assets/UTS/Level1.unity",
            "Assets/UTS/Level2.unity"
        };

        GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/P_LPSP_FP_CH.prefab");
        if (playerPrefab == null)
        {
            Debug.LogError("[SceneCleaner] P_LPSP_FP_CH prefab not found in Assets/Resources!");
            return;
        }

        foreach (string scenePath in scenes)
        {
            if (!File.Exists(scenePath)) continue;

            // Open the scene
            var scene = EditorSceneManager.OpenScene(scenePath);
            Debug.Log("[SceneCleaner] Cleaning scene: " + scenePath);

            // Find the old player
            GameObject oldPlayer = GameObject.Find("Player");
            if (oldPlayer != null)
            {
                Vector3 spawnPos = oldPlayer.transform.position;
                Quaternion spawnRot = oldPlayer.transform.rotation;

                // Instantiate the new player prefab as a prefab instance in the scene
                GameObject newPlayer = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
                newPlayer.name = "Player";
                newPlayer.transform.position = spawnPos;
                newPlayer.transform.rotation = spawnRot;

                // Destroy the old player and its children (which includes old WeaponHolder and weapons)
                Undo.DestroyObjectImmediate(oldPlayer);

                // Save the scene
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                Debug.Log("[SceneCleaner] Successfully replaced player and deleted old weapons in " + scenePath);
            }
            else
            {
                Debug.Log("[SceneCleaner] No player named 'Player' found in " + scenePath);
            }
        }

        // Create the flag file so it only runs once
        File.WriteAllText(cleanedFlag, "Cleaned");

        // Clean up the temporary fallback script since scenes are permanently updated
        string fallbackScript = "Assets/Scenes/Scripts/FirstPersonController.cs";
        if (AssetDatabase.LoadAssetAtPath<MonoScript>(fallbackScript) != null)
        {
            AssetDatabase.DeleteAsset(fallbackScript);
            Debug.Log("[SceneCleaner] Deleted obsolete fallback script: " + fallbackScript);
        }

        // Clean up the old player prefab
        string oldPlayerPrefab = "Assets/Scenes/Prefab/Player.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(oldPlayerPrefab) != null)
        {
            AssetDatabase.DeleteAsset(oldPlayerPrefab);
            Debug.Log("[SceneCleaner] Deleted obsolete old Player prefab: " + oldPlayerPrefab);
        }

        // Force asset database refresh to update Editor view
        AssetDatabase.Refresh();
        Debug.Log("[SceneCleaner] All scenes cleaned and old assets deleted successfully!");
    }
}
