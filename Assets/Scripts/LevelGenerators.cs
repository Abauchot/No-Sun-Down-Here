using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.UI;

public class LevelGenerators : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase exitTile;
    public TileBase hubTile;
    [SerializeField] private GameObject interactPrompt;
    [Header("Map Settings")] public int width = 50;
    public int height = 50;
    public int walkLength = 100;

    private readonly HashSet<Vector2Int> _floorPositions = new HashSet<Vector2Int>();
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera virtualCamera;

    void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
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
        Vector2Int exitPos = currentPos;
        tilemap.SetTile((Vector3Int)exitPos, exitTile);

        // find a point the most far away from the player for exit
        Vector2Int playerSpawnPos = GetFurthestFloorPosition(exitPos);
        SpawnPlayer(playerSpawnPos);

        // putting the camera on the player
        if (Camera.main != null)
            Camera.main.transform.position = new Vector3(playerSpawnPos.x, playerSpawnPos.y, -10f);

        // add hub tile to the player spawn position
        tilemap.SetTile((Vector3Int)playerSpawnPos, hubTile);
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

    void SpawnPlayer(Vector2Int spawnPos)
    {
        Vector3 worldPos = new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f, 0f);
        GameObject playerInstance = Instantiate(playerPrefab, worldPos, Quaternion.identity);

        // Camera setup
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
            virtualCamera.LookAt = playerInstance.transform;
            virtualCamera.Lens.OrthographicSize = 5f;
        }

        // return to hub setup
        ReturnToHub returnScript = playerInstance.GetComponent<ReturnToHub>();
        if (returnScript != null)
        {
            returnScript.SetHubTile(hubTile);
            returnScript.SetTilemap(tilemap);
            returnScript.SetInteractPrompt(interactPrompt);
        }

        // health bar setup
        SetupHealthBar(playerInstance);
    }

    private void SetupHealthBar(GameObject playerInstance)
    {
        var healthScript = playerInstance.GetComponent<PlayerHealth>();
        var canvas = GameObject.Find("Canvas");

        if (healthScript == null || canvas == null) return;

        Transform container = canvas.transform.Find("HealthBarContainer");
        if (container == null) return;

        var segments = new List<Image>();
        for (int i = 1; i <= PlayerHealth.MaxHealth; i++)
        {
            var segment = container.Find($"Segment_{i}")?.GetComponent<Image>();
            if (segment != null)
                segments.Add(segment);
        }

        healthScript.InjectUI(segments);
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