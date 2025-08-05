using UnityEngine;
using UnityEngine.InputSystem;




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
    public SpriteRenderer spriteRenderer;
    public Sprite tilled; 


    void Start()
    {
        if (growthStages.Length >= 1)
        {
            currentGrowthStageIndex = 0;
        }

    }

    void Update()
    {
        /*
        if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            AdvanceStage();
            Debug.Log("E Pressed");
        }*/
    }

    public void AdvanceStage()
    {
        CropStage newGrowthStage = current + 1; 
        switch (current)
        {
            case CropStage.Empty:
                current = newGrowthStage;
                break;
            case CropStage.Growing:
                if (currentGrowthStageIndex != growthStages.Length - 1)
                {
                    currentGrowthStageIndex++;
                }
                else
                {
                    current = newGrowthStage;
                }
               
                break;
            case CropStage.Ripe:
                current = CropStage.Empty;
                if (growthStages.Length >= 1)
                {
                    currentGrowthStageIndex = 0;
                }
                break;
            default:
               
                current = newGrowthStage;
                break;
        }

        SetSoilSprite();

    }


    public void SetSoilSprite()
    {

        if (current == CropStage.Empty)
        {
            spriteRenderer.sprite = null;
        }
        else
        {
            spriteRenderer.sprite = tilled;
        }
        
    }

    public void PloughSoil()
    {
        if(current == CropStage.Empty)
        {
            current = CropStage.Plowed;
            SetSoilSprite();
        }
    }
}
