using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDepth : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private bool _playerInZone;
    private PlayerLocomotionInput _playerLocomotionInput;
    [SerializeField] private GameObject interactPrompt;



    private void Update()
    {
        if (_playerInZone && _playerLocomotionInput != null && _playerLocomotionInput.InteractPressed)
        {
            Debug.Log("Interaction détectée !");
            _playerLocomotionInput.InteractPressed = false; 
            Debug.Log("Texte activé !");
            StartCoroutine(LoadNextScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInZone = true;
            _playerLocomotionInput = other.GetComponent<PlayerLocomotionInput>();

            if (interactPrompt != null)
                interactPrompt.SetActive(true);

            Debug.Log("ON Depth");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInZone = false;
            _playerLocomotionInput = null;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }
    }

    private System.Collections.IEnumerator LoadNextScene()
    {
        UIManager.Instance.StartFadeOut();
        yield return new WaitForSeconds(1f);
        Debug.Log("CHANGEMENT DE SCÈNE");
        SceneManager.LoadScene("Depth_1");
    }

}
