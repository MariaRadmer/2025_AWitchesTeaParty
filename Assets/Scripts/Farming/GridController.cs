using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController instance;

    public Transform minPoint,maxPoint;
    public GrowBlock baseGridBlock;
    public List<BlockRow> blockRows = new List<BlockRow>();

    public LayerMask gridBlockers;

    private Vector2Int gridSize;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        
    }

    void GenerateGrid ()
    {
        minPoint.position = new Vector3(Mathf.Round(minPoint.position.x), Mathf.Round(minPoint.position.y), 0.0f);
        maxPoint.position = new Vector3(Mathf.Round(maxPoint.position.x), Mathf.Round(maxPoint.position.y), 0.0f);

        Vector3 startpoint = minPoint.position + new Vector3(0.5f, 0.5f, 0.0f);

        //Instantiate(baseGridBlock, startpoint, Quaternion.identity);

        int gridSizeX = Mathf.RoundToInt(maxPoint.position.x - minPoint.position.x);
        int gridSizeY = Mathf.RoundToInt(maxPoint.position.y - minPoint.position.y);

        gridSize = new Vector2Int(gridSizeX, gridSizeY);

        for(int y = 0; y < gridSizeY; y++)
        {
            BlockRow currentRow = new BlockRow();
            blockRows.Add(currentRow);
            for (int x = 0; x < gridSizeX; x++)
            {
                GrowBlock growBlock = Instantiate(baseGridBlock, startpoint + new Vector3(x, y, 0f), Quaternion.identity);
                growBlock.transform.SetParent(transform);
                growBlock.spriteRenderer.sprite = null;
                currentRow.blocks.Add(growBlock);
               

                if (Physics2D.OverlapBox(growBlock.transform.position, new Vector2(.9f, .9f), .0f, gridBlockers))
                {
                    growBlock.spriteRenderer.sprite = null;
                    growBlock.preventUse = true;
                    
                }
            }
            
        }

        baseGridBlock.gameObject.SetActive(false);
    }

    public GrowBlock GetBlock(float x , float y)
    {
        int intX = Mathf.FloorToInt(x - minPoint.position.x);
        int intY = Mathf.FloorToInt(y - minPoint.position.y);

        if (intX < gridSize.x && intY < gridSize.y)
        {
            Debug.Log($"Click at world pos ({x}, {y}) -> grid ({intX}, {intY})");
            return blockRows[intY].blocks[intX];
        }

        

        return null;
    }
}

[System.Serializable]
public class BlockRow
{
    public List<GrowBlock> blocks = new List<GrowBlock>();
}
