using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;




public class CropManager : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap;      // Base terrain tiles (dirt, grass, etc)
    public Tilemap tilledSoilTilemap;  // Tilemap to show tilled soil on top
    public Tilemap wateredSoilTilemap;  // Tilemap to show tilled soil on top

    [Header("Tiles")]
    public TileBase[] tillableTilesArray;  // Assign all tillable ground tiles here
    public TileBase tilledSoilTile;         // The tile to show when soil is tilled
    public TileBase wateredSoilTile;         // The tile to show when soil is watered

    // Internal set for quick lookup
    private HashSet<TileBase> tillableTiles;

    // Track tilled soil states
    private Dictionary<Vector3Int, bool> tilledSoilPositions = new();
    // Track watered soil states
    private Dictionary<Vector3Int, bool> wateredSoilPositions = new();

    private void Awake()
    {
        tillableTiles = new HashSet<TileBase>(tillableTilesArray);
    }


    public void tillOrWaterSoil(Vector3Int tilePos)
    {
        if (TillSoil(tilePos))
        {
            Debug.Log($"Tilled at {tilePos}");
            return;
        }

        if (WaterSoil(tilePos))
        {
            Debug.Log($"Watered at {tilePos}");
            return;
        }

        Debug.Log($"No action at {tilePos} (already tilled and watered)");
    }

    /*
     * TILLING
     */
    public bool CanTill(Vector3Int tilePos)
    {
        TileBase groundTile = groundTilemap.GetTile(tilePos);
        if (groundTile == null) return false;
        return tillableTiles.Contains(groundTile);
    }

    public bool TillSoil(Vector3Int tilePos)
    {
        if (!CanTill(tilePos)) return false;

        if (tilledSoilPositions.ContainsKey(tilePos) && tilledSoilPositions[tilePos]) return false; // Already tilled

        tilledSoilTilemap.SetTile(tilePos, tilledSoilTile);
        tilledSoilPositions[tilePos] = true;
        Debug.Log("TillSoil true");
        return true;
    }

    public bool IsTilled(Vector3Int tilePos)
    {
        return tilledSoilPositions.ContainsKey(tilePos) && tilledSoilPositions[tilePos];
    }


    public void RemoveTilledSoil(Vector3Int tilePos)
    {
        tilledSoilTilemap.SetTile(tilePos, null);
        if (tilledSoilPositions.ContainsKey(tilePos)) tilledSoilPositions[tilePos] = false;
    }

    /*
     * WATERING
     */

    public bool WaterSoil(Vector3Int tilePos)
    {
        if (!IsTilled(tilePos) || IsWatered(tilePos)) return false;
        wateredSoilPositions[tilePos] = true;
        wateredSoilTilemap.SetTile(tilePos, wateredSoilTile);
        Debug.Log("Watered soil at: " + tilePos);
        return true;
    }

    public bool IsWatered(Vector3Int tilePos)
    {
        return wateredSoilPositions.ContainsKey(tilePos) && wateredSoilPositions[tilePos];
    }

    public void DrySoil(Vector3Int tilePos)
    {
        wateredSoilTilemap.SetTile(tilePos, null);
        if (wateredSoilPositions.ContainsKey(tilePos))
            wateredSoilPositions[tilePos] = false;
    }

}
