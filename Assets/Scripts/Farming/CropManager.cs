using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;




public class CropManager : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap;      // Base terrain tiles (dirt, grass, etc)
    public Tilemap tilledSoilTilemap;  // Tilemap to show tilled soil on top

    [Header("Tiles")]
    public TileBase[] tillableTilesArray;  // Assign all tillable ground tiles here
    public TileBase tilledSoilTile;         // The tile to show when soil is tilled

    // Internal set for quick lookup
    private HashSet<TileBase> tillableTiles;

    // Track tilled soil states
    private Dictionary<Vector3Int, bool> tilledSoilPositions = new();

    private void Awake()
    {
        tillableTiles = new HashSet<TileBase>(tillableTilesArray);
    }

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
}
