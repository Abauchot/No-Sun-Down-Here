using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public readonly static int MaxHealth = 5;
    public int currentHealth = MaxHealth;
    public float invincibilityDuration = 1f;
    public float knockbackForce = 10f;
    public float flashInterval = 0.1f;
    
    private bool _isInvincible;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    
    [Header("Health UI")]
    [SerializeField] private List<Image> healthSegments;
    [SerializeField] private Sprite filledSegment;
    [SerializeField] private Sprite emptySegment;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = MaxHealth;
    }

    private void Start()
    {
        if (healthSegments is { Count: > 0 })
        {
            UpdateHealthUI();
        }
    }
    
    public void TakeDamage(int damage, Vector2 sourcePosition)
    {
        if (_isInvincible) return;
        currentHealth -= damage;
        Debug.Log($"[PLAYER] A pris {damage} dégâts. PV restants : {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        UpdateHealthUI();
        StartCoroutine(Invincibility());
        ApplyKnockback(sourcePosition);
    }
    
    private void ApplyKnockback(Vector2 sourcePosition)
    {
        Vector2 knockDirection = ((Vector2)transform.position - sourcePosition).normalized;
        _rigidbody2D.AddForce(knockDirection * knockbackForce, ForceMode2D.Impulse);
        Debug.Log("Knockback applied: " + knockDirection);
    }

    private System.Collections.IEnumerator Invincibility()
    {
        _isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        _spriteRenderer.enabled = true;
        _isInvincible = false;
    }
    
    private void UpdateHealthUI()
    {
        if (healthSegments == null || healthSegments.Count == 0) return;
    
        for (int i = 0; i < healthSegments.Count; i++)
        {
            healthSegments[i].sprite = i < currentHealth ? filledSegment : emptySegment;
        }
    }
    
    private static void Die()
    {
        Debug.Log("Player has died.");
    }
    
    public void InjectUI(List<Image> segments)
    {
        healthSegments = segments;
        UpdateHealthUI();
    }


}
