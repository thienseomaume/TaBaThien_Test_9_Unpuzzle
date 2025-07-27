using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using UnityEditor;

[ExecuteInEditMode]
public class BlockPlaceHolder : MonoBehaviour
{
    [SerializeField] List<BlockEntry> blockPrefabsEntry;
    [SerializeField] TextMeshPro lockCountText;
    public int lockStep;
    public Vector2Int gridPos;
    public BlockType blockType = BlockType.Empty;

    private GameObject currentBlockInstance = null;
    private BlockType lastType = BlockType.Empty;

    private GameObject GetPrefab(BlockType blockType)
    {
        foreach (BlockEntry block in blockPrefabsEntry)
        {
            if (block.blockType == blockType)
            {
                return block.prefabBlock;
            }
        }
        return null;
    }

    private void OnValidate()
    {
        if (lockStep > 0)
        {
            lockCountText.gameObject.SetActive(true);
            lockCountText.text = lockStep.ToString();
        }
        else
        {
            lockCountText.gameObject.SetActive(false);
        }
        if (blockType == lastType) return;
        if (currentBlockInstance != null)
        {
#if UNITY_EDITOR
            var objToDestroy = currentBlockInstance;
            currentBlockInstance = null;

            EditorApplication.delayCall += () =>
            {
                if (objToDestroy != null)
                    DestroyImmediate(objToDestroy);
            };
#else
            Destroy(currentBlockInstance);
#endif
        }


        if (blockType != BlockType.Empty)
        {
            GameObject prefab = GetPrefab(blockType);
            if (prefab)
            {
#if UNITY_EDITOR
                currentBlockInstance = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, transform);
#else
                currentBlockInstance = Instantiate(prefab, transform);
#endif
                currentBlockInstance.transform.localPosition = Vector3.zero;
            }

        }

        lastType = blockType;

    }
}
