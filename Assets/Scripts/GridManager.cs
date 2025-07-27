using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    private static GridManager instance;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] float padding = 5;
    [SerializeField] Camera cam;
    [SerializeField] List<BlockEntry> blockPrefabsEntry;
    Dictionary<BlockType, GameObject> blockPrefabs;
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
        blockPrefabs = new Dictionary<BlockType, GameObject>();
        foreach (BlockEntry blockEntry in blockPrefabsEntry)
        {
            if (!blockPrefabs.ContainsKey(blockEntry.blockType))
            {
                blockPrefabs.Add(blockEntry.blockType, blockEntry.prefabBlock);
            }
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
        FitCamera();
        grid = new Tile[width, height];
        int sumOfBlock = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = new Tile();
                grid[x, y].SetCoordinateInGrid(new Vector2Int(x, y));
                BlockType blockType = levelData.layoutBlocks[width * y + x];
                if (blockType != BlockType.Empty)
                {
                    GameObject blockObject = Instantiate(blockPrefabs[blockType], new Vector3(x * cellSize, y * cellSize), Quaternion.identity);
                    Block block = blockObject.GetComponent<Block>();
                    grid[x, y].SetTile(block);
                    if (1 <= (int)blockType && (int)blockType <= 4)
                    {
                        block.SetLock(levelData.unlockMove[width * y + x]);
                        sumOfBlock += 1;
                    }
                }
            }
        }
        LevelManager.Instance().SetRemainingBlock(sumOfBlock);
    }
    public void ReloadLevel(LevelData levelData)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y].GetBlockType() != BlockType.Empty)
                {
                    grid[x, y].DestroyBlockObject();
                }
            }
        }
        GenerateLevel(levelData);
    }
    public bool IsInsideGrid(Vector2Int coordinateInGrid)
    {
        if (coordinateInGrid.x >= 0 && coordinateInGrid.x < width && coordinateInGrid.y >= 0 && coordinateInGrid.y < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnBlockTouched(Vector2Int touchedCoordinate, BlockType blockType)
    {
        LevelManager.Instance().MadeATouch();
        Vector2Int direction = Vector2Int.zero;
        switch (blockType)
        {
            case BlockType.ArrowLeft:
                direction = Vector2Int.left;
                break;
            case BlockType.ArrowRight:
                direction = Vector2Int.right;
                break;
            case BlockType.ArrowUp:
                direction = Vector2Int.up;
                break;
            case BlockType.ArrowDown:
                direction = Vector2Int.down;
                break;
        }
        Vector2Int coordinateCheck = touchedCoordinate;
        do
        {
            coordinateCheck += direction;
            if (IsInsideGrid(coordinateCheck))
            {
                if (grid[coordinateCheck.x, coordinateCheck.y].GetBlockType() == BlockType.DestroyOther)
                {
                    grid[touchedCoordinate.x, touchedCoordinate.y].MoveBlockDirection(direction);
                    Debug.Log("destroyer at " + coordinateCheck.ToString());
                    LevelManager.Instance().CollectPoint();
                    Debug.Log("can move direction will be destroy");
                    break;
                }
                if (grid[coordinateCheck.x, coordinateCheck.y].GetBlockType() != BlockType.Empty)
                {
                    Vector2Int coordinateCanMoveTo = coordinateCheck - direction;
                    if (coordinateCanMoveTo != touchedCoordinate)
                    {
                        grid[touchedCoordinate.x, touchedCoordinate.y].MoveBlockToPosition(new Vector2(coordinateCanMoveTo.x * cellSize, coordinateCanMoveTo.y * cellSize), grid[coordinateCanMoveTo.x, coordinateCanMoveTo.y]);
                        Debug.Log("move to position");
                    }
                    else
                    {
                        SoundManager.Instance().PlayErrorSound();
                        Debug.Log("cannot move");
                    }
                    break;
                }
            }
            else
            {
                grid[touchedCoordinate.x, touchedCoordinate.y].MoveBlockDirection(direction);
                LevelManager.Instance().CollectPoint();
                Debug.Log("can move direction");
            }
        } while (IsInsideGrid(coordinateCheck));

    }
    public void FitCamera()
    {
        if (!cam.orthographic)
        {
            Debug.LogWarning("Camera must be Orthographic!");
            return;
        }

        float totalWidth = width * cellSize;
        float totalHeight = height * cellSize;

        float aspect = cam.aspect;
        float sizeX = (totalWidth / aspect) * 0.5f;
        float sizeY = totalHeight * 0.5f;

        cam.orthographicSize = Mathf.Max(sizeX, sizeY) + padding;

        Vector3 center = new Vector3((width-1)* 0.5f * cellSize, (height-1) * 0.5f * cellSize, 0f);
        cam.transform.position = center + offset;
        cam.transform.rotation = Quaternion.identity;
    }
}
