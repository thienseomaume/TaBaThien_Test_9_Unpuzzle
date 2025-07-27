using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class LevelData : ScriptableObject
{
    public int width;
    public int height;
    public int moveStep;
    public BlockType[] layoutBlocks;
    public int[] unlockMove;
    public void SetBlock(int x, int y, BlockType type)
    {
        layoutBlocks[y * width + x] = type;
    }
    public void SetLock(int x, int y, int lockStep)
    {
        unlockMove[y * width + x] = lockStep;
    }


    public void Resize(int w, int h)
    {
        width = w;
        height = h;
        layoutBlocks = new BlockType[w * h];
        unlockMove = new int[w * h];
    }
}
