using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int health = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // FX, loot, etc.
        Destroy(gameObject);
    }
}