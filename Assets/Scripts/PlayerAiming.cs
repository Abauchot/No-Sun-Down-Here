using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    private Camera _camera;
    private PlayerLocomotionInput _playerLocomotionInput;

    private void Awake()
    {
        _camera = Camera.main;
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }
    
    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(_playerLocomotionInput.AimInput);
        Vector3 aimDirection = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        // Rotate the player towards the aim direction
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    
}
