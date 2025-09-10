using UnityEngine;

public class GridController : MonoBehaviour
{
    public Transform minPoint,maxPoint;
    public GrowBlock baseGridBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid ()
    {
        minPoint.position = new Vector3(Mathf.Round(minPoint.position.x), Mathf.Round(minPoint.position.y), 0.0f);
        maxPoint.position = new Vector3(Mathf.Round(maxPoint.position.x), Mathf.Round(maxPoint.position.y), 0.0f);

        Vector3 startpoint = minPoint.position + new Vector3(0.5f, 0.5f, 0.0f);

        Instantiate(baseGridBlock, startpoint, Quaternion.identity);
    }
}
