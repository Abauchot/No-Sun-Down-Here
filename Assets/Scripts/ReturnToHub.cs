using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ReturnToHub : MonoBehaviour
{
    private Tilemap _tilemap;
    private TileBase _hubTile;
    private GameObject _interactPrompt;
    private PlayerLocomotionInput _input;

    private void Start()
    {
        _input = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        if (!_tilemap || !_hubTile) return;

        Vector3Int playerTilePos = _tilemap.WorldToCell(transform.position);
        bool isOnHubTile = _tilemap.GetTile(playerTilePos) == _hubTile;

        if (isOnHubTile)
        {
            if (_interactPrompt)
                _interactPrompt.SetActive(true);

            if (_input && _input.InteractPressed)
            {
                _input.InteractPressed = false;
                UIManager.Instance?.StartFadeOut();
                StartCoroutine(LoadHubAfterFade());
            }
        }
        else if (_interactPrompt)
        {
            _interactPrompt.SetActive(false);
        }
    }

    private System.Collections.IEnumerator LoadHubAfterFade()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Hub");
    }
    
    public void SetHubTile(TileBase tile) => _hubTile = tile;
    public void SetTilemap(Tilemap map) => _tilemap = map;
    public void SetInteractPrompt(GameObject prompt)
    {
        _interactPrompt = prompt;
        Debug.Log($"[ReturnToHub] InteractPrompt assigné : {_interactPrompt?.name}");
    }

}