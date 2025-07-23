using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSide;
    [SerializeField] Dictionary<BlockType, GameObject> blockPrefabs;
    Tile[,] grid;
    public static GridManager Instance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GenerateLevel(LevelData levelData)
    {
        width = levelData.width;
        height = levelData.height;
        grid = new Tile[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BlockType blockType = levelData.layoutBlocks[width * y + x];
                GameObject blockObject = Instantiate(blockPrefabs[blockType]);
            }
        }
    }
}
