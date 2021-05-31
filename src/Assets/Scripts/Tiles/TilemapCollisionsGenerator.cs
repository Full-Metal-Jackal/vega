using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapCollisionsGenerator : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        SetupColliders();
    }

    public HashSet<Vector3Int> GetOccupiedTileCells()
    {
        HashSet<Vector3Int> cells = new HashSet<Vector3Int>();

        const int zLevel = 0;
        for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
            for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
                if (!(tilemap.GetTile(new Vector3Int(x, y, zLevel)) is null))
                    cells.Add(new Vector3Int(x, y, zLevel));

        return cells;
    }

    public Vector3 CellToLocalPosition(Vector3Int cell)
	{
        Vector3 result = cell;
        result += new Vector3(.5f, .5f, -.5f);
        result.Scale(tilemap.layoutGrid.cellSize);

        return result;
    }

    public Vector3 CellToWorldPosition(Vector3Int cell) => transform.TransformPoint(CellToLocalPosition(cell));

    public void SetupColliders()
	{
        // This function has to be re-written with due optimization which is actually quite easy to implement but is not our current priority.
        foreach (Vector3Int cell in GetOccupiedTileCells())
		{
            if (!(gameObject.AddComponent(typeof(BoxCollider)) is BoxCollider collider))
                continue;
            collider.center = CellToLocalPosition(cell);
            collider.size = tilemap.layoutGrid.cellSize;
        }
    }
}