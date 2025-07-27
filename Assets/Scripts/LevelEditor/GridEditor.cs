using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridEditor : EditorWindow
{
    private LevelData levelData;
    private GameObject gridRoot;
    private GameObject placeholderPrefab;
    private float cellSize;

    [MenuItem("Tools/Level Grid Editor")]
    public static void Open()
    {
        GetWindow<GridEditor>("Level Editor");
    }
    void OnGUI()
    {
        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);
        gridRoot = (GameObject)EditorGUILayout.ObjectField("Grid Root", gridRoot, typeof(GameObject), true);
        placeholderPrefab = (GameObject)EditorGUILayout.ObjectField("Placeholder Prefab", placeholderPrefab, typeof(GameObject), false);
        cellSize = (float)EditorGUILayout.FloatField("cell size", cellSize);

        if (GUILayout.Button("Generate Grid Placeholders"))
        {
            GeneratePlaceholders();
        }

        if (GUILayout.Button("Save To Level Data"))
        {
            SaveGridToData();
        }

    }
    private void GeneratePlaceholders()
    {
        if (levelData == null || gridRoot == null || placeholderPrefab == null)
        {
            return;
        }

        ClearChildren(gridRoot.transform);

        for (int y = 0; y < levelData.height; y++)
        {
            for (int x = 0; x < levelData.width; x++)
            {
                GameObject placeholder = (GameObject)PrefabUtility.InstantiatePrefab(placeholderPrefab, gridRoot.transform);
                placeholder.transform.localPosition = new Vector3(x * cellSize,y * cellSize, 0);

                var ph = placeholder.GetComponent<BlockPlaceHolder>();
                if (ph != null)
                {
                    ph.gridPos = new Vector2Int(x, y);
                    ph.blockType = BlockType.Empty;
                }
            }
        }
    }


    private void SaveGridToData()
    {
        if (levelData == null || gridRoot == null) return;

        levelData.Resize(levelData.width, levelData.height);

        foreach (Transform child in gridRoot.transform)
        {
            var ph = child.GetComponent<BlockPlaceHolder>();
            if (ph != null)
            {
                levelData.SetBlock(ph.gridPos.x, ph.gridPos.y, ph.blockType);
                levelData.SetLock(ph.gridPos.x, ph.gridPos.y, ph.lockStep);
            }
        }

        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
        Debug.Log("Level data saved!");
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }
}
