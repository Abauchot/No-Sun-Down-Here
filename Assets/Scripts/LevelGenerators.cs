using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.Cinemachine;

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
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera _virtualCamera;

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
        // Placement de la sortie
        Vector2Int exitPos = currentPos;
        tilemap.SetTile((Vector3Int)exitPos, exitTile);

        // Trouve le point le plus éloigné de la sortie pour le spawn joueur
        Vector2Int playerSpawnPos = GetFurthestFloorPosition(exitPos);
        SpawnPlayer(playerSpawnPos);

        // Positionne la caméra principale si besoin
        Camera.main.transform.position = new Vector3(playerSpawnPos.x, playerSpawnPos.y, -10f);

    }
    
    private Vector2Int GetFurthestFloorPosition(Vector2Int from)
    {
        Vector2Int furthest = from;
        float maxDist = 0f;

        foreach (var pos in _floorPositions)
        {
            float dist = Vector2Int.Distance(pos, from);
            if (dist > maxDist)
            {
                maxDist = dist;
                furthest = pos;
            }
        }

        return furthest;
    }
    
    private void SpawnPlayer(Vector2Int spawnPos)
    {
        Vector3 worldPos = new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f, 0);
        GameObject player = Instantiate(playerPrefab, worldPos, Quaternion.identity);
        
        if (_virtualCamera != null)
        {
            _virtualCamera.Follow = player.transform;
        }
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