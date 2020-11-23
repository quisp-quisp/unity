using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TerrainEditor : Editor
{
    [MenuItem("Terrain/Terrain/Add Layers")]
    private static void AddLayers()
    {
        string folderPath = EditorUtility.OpenFolderPanel("Add Layers", "Assets/", "");

        // selection window closed
        if (folderPath == "")
        {
            return;
        }

        // confirm selection
        if (!EditorUtility.DisplayDialog("Confirmation", "Add " + folderPath + " to " + Selection.activeGameObject.name + "?", "Yes", "No"))
        {
            return;
        }

        // outside folder selected
        if (folderPath.IndexOf(Application.dataPath) == -1)
        {
            Debug.LogWarning("Selected folder not in Assets");
            return;
        }

        ClearConsole();

        TerrainData terrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        List<TerrainLayer> terrainLayers = new List<TerrainLayer>(terrainData.terrainLayers);
        string[] filePaths = System.IO.Directory.GetFiles(folderPath);
        int numLayers = 0;

        // only add terrain layers in folder
        for (int i = 0; i < filePaths.Length; ++i)
        {
            string relativePath = filePaths[i].Substring(filePaths[i].IndexOf("Assets/"));
            TerrainLayer terrainLayer = AssetDatabase.LoadAssetAtPath<TerrainLayer>(relativePath);
            if (terrainLayer != null)
            {
                // check for duplicate prefabs
                bool isDuplicate = false;
                for (int j = 0; j < terrainLayers.Count; ++j)
                {
                    if (Equals(terrainLayer, terrainLayers[j]))
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                // only add unique prefabs
                if (!isDuplicate)
                {
                    terrainLayers.Add(terrainLayer);
                    ++numLayers;
                }
            }
        }

        // add terrain layers to terrain data
        terrainData.terrainLayers = terrainLayers.ToArray();
        Selection.activeGameObject.GetComponent<Terrain>().Flush();
        terrainData.RefreshPrototypes();

        // display how many trees were added
        EditorUtility.DisplayDialog("Confirmation", "Added " + numLayers + " terrain layers.", "OK");
    }

    [MenuItem("Terrain/Terrain/Clear Layers")]
    private static void ClearLayers()
    {
        ClearConsole();

        // clear trees from terrain data
        TerrainData terrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        int numLayers = terrainData.terrainLayers.Length;
        terrainData.terrainLayers = new TerrainLayer[0];
        Selection.activeGameObject.GetComponent<Terrain>().Flush();
        terrainData.RefreshPrototypes();

        // display how many trees were removed
        EditorUtility.DisplayDialog("Confirmation", "Removed " + numLayers + " terrain layers.", "OK");
    }

    [MenuItem("Terrain/Trees/Add Trees")]
    private static void AddTrees()
    {
        string folderPath = EditorUtility.OpenFolderPanel("Add Trees", "Assets/", "");

        // selection window closed
        if (folderPath == "")
        {
            return;
        }

        // confirm selection
        if (!EditorUtility.DisplayDialog("Confirmation", "Add " + folderPath + " to " + Selection.activeGameObject.name + "?", "Yes", "No"))
        {
            return;
        }

        // outside folder selected
        if (folderPath.IndexOf(Application.dataPath) == -1)
        {
            Debug.LogWarning("Selected folder not in Assets");
            return;
        }

        ClearConsole();

        TerrainData terrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        List<TreePrototype> treePrototypes = new List<TreePrototype>(terrainData.treePrototypes);
        string[] filePaths = System.IO.Directory.GetFiles(folderPath);
        int numTrees = 0;

        // only add prefabs in folder
        for (int i = 0; i < filePaths.Length; ++i)
        {
            TreePrototype tree = new TreePrototype();
            string relativePath = filePaths[i].Substring(filePaths[i].IndexOf("Assets/"));
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePath);
            if (prefab != null)
            {
                // check for duplicate prefabs
                bool isDuplicate = false;
                for (int j = 0; j < treePrototypes.Count; ++j)
                {
                    if (Equals(prefab, treePrototypes[j].prefab))
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                // only add unique prefabs
                if (!isDuplicate)
                {
                    tree.prefab = prefab;
                    treePrototypes.Add(tree);
                    ++numTrees;
                }
            }
        }

        // add trees to terrain data
        terrainData.treePrototypes = treePrototypes.ToArray();
        Selection.activeGameObject.GetComponent<Terrain>().Flush();
        terrainData.RefreshPrototypes();

        // display how many trees were added
        EditorUtility.DisplayDialog("Confirmation", "Added " + numTrees + " trees.", "OK");
    }

    [MenuItem("Terrain/Trees/Clear Trees")]
    private static void ClearTrees()
    {
        ClearConsole();

        // clear trees from terrain data
        TerrainData currentTerrainData = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
        int numTrees = currentTerrainData.treePrototypes.Length;
        currentTerrainData.treePrototypes = new TreePrototype[0];
        Selection.activeGameObject.GetComponent<Terrain>().Flush();
        currentTerrainData.RefreshPrototypes();

        // display how many trees were removed
        EditorUtility.DisplayDialog("Confirmation", "Removed " + numTrees + " trees.", "OK");
    }

    [MenuItem("Terrain/Details/Add Textures")]
    private static void AddTextures()
    {

    }

    [MenuItem("Terrain/Details/Add Meshes")]
    private static void AddMeshes()
    {

    }

    [MenuItem("Terrain/Details/Clear Textures")]
    private static void ClearTextures()
    {

    }

    [MenuItem("Terrain/Details/Clear Meshes")]
    private static void ClearMeshes()
    {

    }

    [MenuItem("Terrain/Terrain/Add Layers", true)]
    private static bool ValidateAddLayers()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Terrain/Clear Layers", true)]
    private static bool ValidateClearLayers()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Trees/Add Trees", true)]
    private static bool ValidateAddTrees()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Trees/Clear Trees", true)]
    private static bool ValidateClearTrees()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Details/Add Textures", true)]
    private static bool ValidateAddTextures()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Details/Add Meshes", true)]
    private static bool ValidateAddMeshes()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Details/Clear Textures", true)]
    private static bool ValidateClearTextures()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    [MenuItem("Terrain/Details/Clear Meshes", true)]
    private static bool ValidateClearMeshes()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("No Terrain selected");
            return false;
        }
        return true;
    }

    private static void ClearConsole()
    {
        // call internal clear method through reflection
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
