using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform _player;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate()
    {
        if(_player == null) return;
        
            Vector2 direction = ((Vector2)_player.position - _rigidbody.position).normalized;
            _rigidbody.MovePosition(_rigidbody.position + direction * (moveSpeed * Time.fixedDeltaTime));
        
    }


}
