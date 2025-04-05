using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float meleeRange = 1f;
    public int meleeDamage = 1;
    public LayerMask enemyLayer;

    private PlayerLocomotionInput _playerLocomotion;
    
    private void Awake()
    {
        _playerLocomotion = GetComponent<PlayerLocomotionInput>();
    }
    
    private void Update()
    {
        if (_playerLocomotion.MeleePressed)
        {
            PerformMeleeAttack();
            _playerLocomotion.MeleePressed = false;
            Debug.Log("Melee Attack");
        }
    }
    
    private void PerformMeleeAttack()
    {
        Vector2 attackOrigin = (Vector2)transform.position + _playerLocomotion.MoveInput.normalized * meleeRange;
        Vector2 attackDirection = (Camera.main.ScreenToWorldPoint(_playerLocomotion.AimInput) 
                                   - transform.position).normalized;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin, meleeRange * 0.5f, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            (enemy.GetComponent<IDamageable>())?.TakeDamage(meleeDamage);
        }
    }
}
