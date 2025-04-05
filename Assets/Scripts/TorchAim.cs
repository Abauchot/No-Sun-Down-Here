using UnityEngine;

public class TorchAim : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerLocomotionInput playerLocomotionInput;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        if (playerTransform == null)
            playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (playerLocomotionInput == null && playerTransform != null)
            playerLocomotionInput = playerTransform.GetComponent<PlayerLocomotionInput>();
    }
    
    private void Update()
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(playerLocomotionInput.AimInput);
        mouseWorldPos.z = 0;

        Vector3 direction = (mouseWorldPos - playerTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
    
}
