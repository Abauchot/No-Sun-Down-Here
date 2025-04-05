using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float lifeTime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destroy the bullet after a certain time
      
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         Destroy(gameObject);
        
    }
}
