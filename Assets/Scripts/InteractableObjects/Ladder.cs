using UnityEngine;

public class Ladder : MonoBehaviour
{
    private float _vertical;
    private float _speed = 8;
    private bool _isLadder;
    private bool _isClimbing;

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        _vertical = Input.GetAxisRaw("Vertical");

        if (_isLadder && _vertical > 0)
        {
            _isClimbing = true;
            rb.gravityScale = 0;
        }
    }

    private void FixedUpdate()
    {
        if (_isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, _vertical * _speed);
        }
        else
        {
            rb.gravityScale = 4;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _isLadder = false;
            _isClimbing = false;
        }
    }
}
