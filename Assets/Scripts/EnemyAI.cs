using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRadius = 6f;       
    public float senseRadius = 2f;           
    public float fieldOfView = 120f;
    public float memoryDuration = 10f;
    public int damage = 1;
    public float attackCooldown = 1f;

    private Transform _player;
    private Rigidbody2D _rigidbody;

    private bool _hasDetectedPlayer;
    private float _timeSinceLastSeen;
    private float _lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        Vector2 toPlayer = _player.position - transform.position;
        float distance = toPlayer.magnitude;
        float angle = Vector2.Angle(transform.right, toPlayer.normalized);

        bool inFOV = distance <= detectionRadius && angle <= fieldOfView / 2f;
        bool inSenseRange = distance <= senseRadius;

        bool canSeeOrSensePlayer = inFOV || inSenseRange;

        if (canSeeOrSensePlayer)
        {
            _hasDetectedPlayer = true;
            _timeSinceLastSeen = 0f;
        }
        else if (_hasDetectedPlayer)
        {
            _timeSinceLastSeen += Time.fixedDeltaTime;
            _hasDetectedPlayer = _timeSinceLastSeen < memoryDuration;
        }

        if (_hasDetectedPlayer)
        {
            Vector2 direction = toPlayer.normalized;
            _rigidbody.MovePosition(_rigidbody.position + direction * (moveSpeed * Time.fixedDeltaTime));
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= _lastAttackTime + attackCooldown)
            {
                PlayerHealth player = collision.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damage, transform.position);
                    _lastAttackTime = Time.time;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vision zone
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Sensory zone
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, senseRadius);

        // FOV lines
        Vector3 left = Quaternion.Euler(0, 0, -fieldOfView / 2) * transform.right;
        Vector3 right = Quaternion.Euler(0, 0, fieldOfView / 2) * transform.right;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + right * detectionRadius);
    }
}
