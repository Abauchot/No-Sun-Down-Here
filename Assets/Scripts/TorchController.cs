using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    [SerializeField] private Light2D torchLight;
    [SerializeField] private PlayerLocomotionInput playerLocomotionInput;


    private void Start()
    {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        torchLight.enabled = (currentSceneName != "Hub");
    }

    void Update()
    {
        if (playerLocomotionInput.ToggleLightsPressed)
        {
            ToggleTorch();
            playerLocomotionInput.ToggleLightsPressed = false;
        }
    }

    private void ToggleTorch()
    {
        torchLight.enabled = !torchLight.enabled;
        
    }
}