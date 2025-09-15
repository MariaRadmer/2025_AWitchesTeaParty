using UnityEngine;

public class GridController : MonoBehaviour
{
    public Transform minPoint,maxPoint;
    public GrowBlock baseGridBlock;

    private Vector2Int gridSize; 

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
            for (int x = 0; x < gridSizeX; x++)
            {
                GrowBlock growBlock = Instantiate(baseGridBlock, startpoint + new Vector3(x, y, 0f), Quaternion.identity);
                growBlock.transform.SetParent(transform);
            }
            
        }

        baseGridBlock.gameObject.SetActive(false);
    }
}
