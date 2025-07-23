using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    Block block;
    Vector2Int coordinateInGrid;
    public Tile(BlockType blockType)
    {
    }
    public Tile(Block block)
    {
        this.block = block;
    }
    public void SetTile(Block block)
    {
        this.block = block;
    }
    public void ClearTile()
    {
        block = null;
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
    public void MoveBlockDirection()
    {

    }
    public void MoveBlockToPosition()
    {
        
    }

}
