using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CropData", menuName = "Farming/CropData")]
public class CropData : ScriptableObject
{
    public string cropName;
    public int growthDaysTotal;
    public int[] stageDays;
    public Sprite[] stageSprites;
    public bool regrows;
    public Season[] growableSeasons;
}
