using UnityEngine;




public class GrowBlock : MonoBehaviour
{

    public enum CropStage{
        Empty,
        Plowed,
        Planted,
        Growing,
        Ripe
    }

    public CropStage current;

    public int[] growthStages = new int[] { 1, 2, 3 }; 
    public int currentGrowthStageIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (growthStages.Length >= 1)
        {
            currentGrowthStageIndex = 0;
        }

        AdvanceStage(); // plowed
        AdvanceStage(); // planting
        AdvanceStage(); // Growing
        AdvanceStage(); // 1
        AdvanceStage(); // 2
        AdvanceStage(); // 3
        AdvanceStage(); // Ripe
        AdvanceStage(); // Empty
        AdvanceStage(); // Plowed
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceStage()
    {
        CropStage newGrowthStage = current + 1; 
        switch (current)
        {
            case CropStage.Growing:
                //Debug.Log("CropStage.Growing");
                if (currentGrowthStageIndex != growthStages.Length - 1)
                {
                    currentGrowthStageIndex++;
                    //Debug.Log("currentGrowthStageIndex " + currentGrowthStageIndex);
                }
                else
                {
                    current = newGrowthStage;
                }
               
                break;
            case CropStage.Ripe:
                //Debug.Log("CropStage.Ripe");
                current = CropStage.Empty;
                if (growthStages.Length >= 1)
                {
                    currentGrowthStageIndex = 0;
                }
                break;
            default:
                //Debug.Log("current "+ current);
                current = newGrowthStage;
                break;
        }
       

    }
}
