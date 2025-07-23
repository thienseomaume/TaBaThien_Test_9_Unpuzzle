using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Block : MonoBehaviour
{
    [SerializeField] BlockType blockType;
    [SerializeField] int stepToUnlock;
    [SerializeField] GameObject blockBound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public BlockType GetBlockType()
    {
        return blockType;
    }
    public void Lock(int stepLock)
    {
        stepToUnlock = stepLock;
        blockBound?.SetActive(true);
    }
    public Boolean IsLocked()
    {
        if (stepToUnlock > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DownStepUnlock()
    {
        stepToUnlock -= 1;
        if (stepToUnlock <= 0)
        {
            blockBound?.SetActive(false);
        }
    }
    public void MoveDirecton()
    {

    }
    public void MoveToPosition()
    {
        
    }
}
