using UnityEditor;
using UnityEngine;
using System.IO;

public static class PrintSprites
{
    [MenuItem("Tools/List 2D Shooter Sprites")]
    public static void ListSprites()
    {
        string path = Path.Combine(Application.dataPath, "Resources/2DGuiShooter");
        if (!Directory.Exists(path))
        {
            Debug.LogError("Resources/2DGuiShooter folder not found!");
            return;
        }

        string[] files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
        Debug.Log("Found " + files.Length + " sprites in 2DGuiShooter pack:");

        foreach (string file in files)
        {
            string relativePath = "Assets" + file.Substring(Application.dataPath.Length).Replace('\\', '/');
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
            if (sprite != null)
            {
                Debug.Log(string.Format("Sprite: {0} | Dimensions: {1}x{2} | Rect: {3}", 
                    Path.GetFileName(file), 
                    sprite.texture.width, 
                    sprite.texture.height, 
                    sprite.rect));
            }
        }
    }
}
