using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class LevelData : ScriptableObject
{
    public int width;
    public int height;
    public BlockType[] layoutBlocks;
    public int[] unlockMove;
}
