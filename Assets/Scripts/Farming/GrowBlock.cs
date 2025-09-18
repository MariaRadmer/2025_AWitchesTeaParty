using UnityEngine;
using UnityEngine.InputSystem;




public class GrowBlock : MonoBehaviour
{

    public enum GrowthStage{
        Empty,
        Plowed,
        Planted,
        Growing1,
        Growing2,
        Ripe
    }

    public GrowthStage currentStage;

    //public int[] growthStages = new int[] { 1, 2, 3 }; 
    //public int currentGrowthStageIndex;

    public SpriteRenderer spriteRenderer;
    public Sprite tilled;

    public SpriteRenderer cropSprite;
    public Sprite cropPlanted, cropGrowing1, cropGrowing2, cropRipe;

    public Sprite wateredTile;
    public bool isWatered = false;

    public bool preventUse = false;

    
    void Start()
    {
        /*
        if (growthStages.Length >= 1)
        {
            currentGrowthStageIndex = 0;
        }*/


    }

    void Update()
    {
        /*
        if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            AdvanceStage();
            Debug.Log("E Pressed");
        }*/

#if UNITY_EDITOR
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            AdvanceCrop();
        }
#endif
    }

    public void AdvanceStage()
    {
        GrowthStage newGrowthStage = currentStage + 1; 
        switch (currentStage)
        {
            case GrowthStage.Empty:
                currentStage = newGrowthStage;
                break;
            case GrowthStage.Growing1:

                currentStage = newGrowthStage;

                /*
                if (currentGrowthStageIndex != growthStages.Length - 1)
                {
                    currentGrowthStageIndex++;
                }
                else
                {
                   
                }*/
               
                break;
            case GrowthStage.Ripe:
                currentStage = GrowthStage.Empty;
                /*
                if (growthStages.Length >= 1)
                {
                    currentGrowthStageIndex = 0;
                }*/
                break;
            default:
               
                currentStage = newGrowthStage;
                break;
        }

        SetSoilSprite();

    }


    public void SetSoilSprite()
    {

        if (currentStage == GrowthStage.Empty)
        {
            spriteRenderer.sprite = null;
        }
        else
        {
            if(isWatered)
            {
                spriteRenderer.sprite = wateredTile;
            }
            else
            {
                spriteRenderer.sprite = tilled;
            }
            
        }
        
    }

    public void PloughSoil()
    {
        if (preventUse) return; 
        if(currentStage == GrowthStage.Empty)
        {
            currentStage = GrowthStage.Plowed;
            SetSoilSprite();
        }
    }

    public void WaterSoil()
    {
        if (preventUse) return;
        isWatered = true;
        SetSoilSprite();
    }

    public void PlantCrop()
    {
        if (preventUse) return;
        if (currentStage  == GrowthStage.Plowed && isWatered)
        {
            currentStage = GrowthStage.Planted;
        }
        UpdateCropSprite();
    }

    void UpdateCropSprite()
    {
        switch (currentStage)
        {
            case GrowthStage.Planted:
                
                cropSprite.sprite = cropPlanted;
                break;

            case GrowthStage.Growing1:
                cropSprite.sprite = cropGrowing1;
                break;
            case GrowthStage.Growing2:
                cropSprite.sprite = cropGrowing2;
                break;

            case GrowthStage.Ripe:
                cropSprite.sprite = cropRipe;
                break;
        }
    }


    public void AdvanceCrop()
    {
        if (preventUse) return;
        if (isWatered)
        {
            if(currentStage == GrowthStage.Planted ||  currentStage == GrowthStage.Growing1 || currentStage == GrowthStage.Growing2)
            {
                currentStage++;

                isWatered = false;
                SetSoilSprite();
                UpdateCropSprite();
            }
        }
    }

    public void HarvestCrop()
    {
        if (preventUse) return;
        if (currentStage == GrowthStage.Ripe)
        {
            currentStage = GrowthStage.Plowed;
            SetSoilSprite();
            cropSprite.sprite = null;
        }
    }
}
