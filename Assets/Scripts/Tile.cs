using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    Block block;
    public Vector2Int coordinateInGrid;
    public Tile()
    {
    }
    public void SetTile(Block block)
    {
        this.block = block;
        this.block?.SetTile(this);
    }
    public void ClearBlock()
    {
        block = null;
    }
    public void BlockTouched()
    {
        GridManager.Instance().OnBlockTouched(coordinateInGrid, block.GetBlockType());
    }
    public void SetCoordinateInGrid(Vector2Int coordinateInGrid)
    {
        this.coordinateInGrid = coordinateInGrid;
    }
    public BlockType GetBlockType()
    {
        if (block == null)
        {
            return BlockType.Empty;
        }
        else
        {
            return block.GetBlockType();
        }
    }
    public bool IsBlocked()
    {
        if (block != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void MoveBlockDirection(Vector2 direction)
    {
        block.MoveDirection(direction);
        block.ClearTile();
        ClearBlock();

    }
    public void MoveBlockToPosition(Vector2 position,Tile newTile)
    {
        block.MoveToPosition(position);
        newTile.SetTile(block);
        ClearBlock();
    }
    public void DestroyBlockObject()
    {
        block?.DestroyObject();
    }
}
