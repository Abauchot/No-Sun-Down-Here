using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    [SerializeField] private Light2D torchLight;
    [SerializeField] private PlayerLocomotionInput playerLocomotionInput;

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
        Debug.Log("Lampe torche " + (torchLight.enabled ? "allumée" : "éteinte"));
    }
}