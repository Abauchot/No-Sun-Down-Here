using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelGenerators : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase exitTile;

    [Header("Map Settings")]
    public int width = 50;
    public int height = 50;
    public int walkLength = 100;

    private HashSet<Vector2Int> _floorPositions = new HashSet<Vector2Int>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        tilemap.ClearAllTiles();
        _floorPositions.Clear();

        Vector2Int currentPos = new Vector2Int(width / 2, height / 2);
        _floorPositions.Add(currentPos);

        // Random walk
        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int direction = GetRandomDirection();
            currentPos += direction;

            // keep within bounds
            currentPos.x = Mathf.Clamp(currentPos.x, 1, width - 2);
            currentPos.y = Mathf.Clamp(currentPos.y, 1, height - 2);

            _floorPositions.Add(currentPos);
        }

        // setting tiles
        foreach (Vector2Int pos in _floorPositions)
        {
            tilemap.SetTile((Vector3Int)pos, floorTile);
        }

        // adding walls
        foreach (Vector2Int pos in _floorPositions)
        {
            foreach (Vector2Int dir in Directions)
            {
                Vector2Int neighbor = pos + dir;
                if (!_floorPositions.Contains(neighbor))
                {
                    tilemap.SetTile((Vector3Int)neighbor, wallTile);
                }
            }
        }

        // add exit from the last position by last position of the walk
        tilemap.SetTile((Vector3Int)currentPos, exitTile);

        // center the camera on the level
        Vector3 center = new Vector3(width / 2f, height / 2f, -10f);
        Camera.main.transform.position = center;
    }

    private Vector2Int GetRandomDirection()
    {
        Vector2Int[] dirs = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        return dirs[Random.Range(0, dirs.Length)];
    }

    private List<Vector2Int> Directions => new List<Vector2Int>
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };
}