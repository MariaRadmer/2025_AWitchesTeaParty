
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{

    public static GridInfo instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public bool hasGrid;

    public List<InfoRow> theGrid;


    public void CreateGrid()
    {
        hasGrid = true;

        for(int y = 0; y < GridController.instance.blockRows.Count; y++)
        {
            theGrid.Add(new InfoRow());

            for(int x = 0; x< GridController.instance.blockRows[y].blocks.Count;x++ ) 
            {
                theGrid[y].blocks.Add(new BlockInfo());
            }
        }
    }

}

[System.Serializable]
public class BlockInfo
{
    public bool isWatered;
    public GrowBlock.GrowthStage currentStage;
}

[System.Serializable]
public class InfoRow
{
    public List<BlockInfo> blocks = new List<BlockInfo>();
}