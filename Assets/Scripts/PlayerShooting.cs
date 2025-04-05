using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public int damage = 1;
    
    private PlayerLocomotionInput _playerLocomotion;
    private Camera _camera;


    private void Awake()
    {
        _playerLocomotion = GetComponent<PlayerLocomotionInput>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_playerLocomotion.FirePressed)
        {
            Shoot();
            _playerLocomotion.FirePressed = false;
            Debug.Log("Shooting");
            
        }
    }


    private void Shoot()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(_playerLocomotion.AimInput);
        mousePos.z = 0; 
        Vector3 shootDirection = (mousePos - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = shootDirection * bulletSpeed;

    }
}
