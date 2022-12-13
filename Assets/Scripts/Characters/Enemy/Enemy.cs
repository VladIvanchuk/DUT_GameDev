using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _maxHp;
    private float _currentHp;

    [Header("Movement")]
    [SerializeField] private Transform _player;
    [SerializeField] public Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] private float _speed;
    private float _startSpeed;
    [SerializeField] private int _patrolDistance;
    [SerializeField] private int _stopDistance;
    private Vector2 _startPosition;
    private bool _movingRight = true;

    [Header("Animations")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _walkAnimationKey;
    [SerializeField] private string _hitAnimationKey;
    [SerializeField] private string _attack1AnimationKey;
    [SerializeField] private string _deathAnimationKey;

    [Header("State")]
    private bool dead = false;
    private bool patrol = false;
    private bool pursuit = false;
    private bool goBack = false;
    private bool attack = false;
    private bool takeHit = false;

    [Header("Attack")]
    [SerializeField] private float _attackColdown;
    [SerializeField] private float _damage;
    private float _coldownTimer = Mathf.Infinity;
    private bool _nearPlayer = false;



    void Start()
    {
        _currentHp = _maxHp;
        _startPosition = transform.position;
        _startSpeed = _speed;
    }

    void Update()
    {
        _coldownTimer += Time.deltaTime;
        if (!dead)
        {
            if (Vector2.Distance(transform.position, _startPosition) < _patrolDistance && !pursuit)
            {
                patrol = true;
                goBack = false;
            }
            if (Vector2.Distance(transform.position, _player.position) < _stopDistance)
            {
                pursuit = true;
                patrol = false;
                goBack = false;
            }
            if (Vector2.Distance(transform.position, _player.position) > _stopDistance)
            {
                goBack = true;
                pursuit = false;
            }
            if (attack)
            {
                pursuit = false;
                patrol = false;
                goBack = false;
            }
            if (takeHit)
            {
                attack = false;
                pursuit = false;
                patrol = false;
                goBack = false;
            }

            if (patrol)
            {
                Patrol();
            }
            else if (pursuit)
            {
                Pursuit();
            }
            else if (goBack)
            {
                GoBack();
            }           

            _animator.SetBool(_attack1AnimationKey, _nearPlayer);
            _animator.SetBool(_hitAnimationKey, takeHit);
            _animator.SetBool(_walkAnimationKey, patrol || pursuit || goBack);
        }
        _animator.SetBool(_deathAnimationKey, dead);
    }
    private void Patrol()
    {
        if (transform.position.x > _startPosition.x + _patrolDistance)
        {
            MoverScripts.Flip(transform);
            _movingRight = false;
        }
        else if (transform.position.x < _startPosition.x - _patrolDistance)
        {
            MoverScripts.Flip(transform);
            _movingRight = true;
        }
        if (_movingRight)
        {
            transform.position = new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - _speed * Time.deltaTime, transform.position.y);
        }
    }

    private void Pursuit()
    {
        if (_movingRight)
        {
            MoverScripts.Flip(transform);
            _movingRight = false;
        }
        transform.position = Vector2.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
        _speed = _startSpeed + 2;
    }
    private void GoBack()
    {
        if (!_movingRight)
        {
            MoverScripts.Flip(transform);
            _movingRight = true;
        }
        transform.position = Vector2.MoveTowards(transform.position, _startPosition, _speed * Time.deltaTime);
        _speed = _startSpeed;
    }

    private void Attack()
    {
        if (_nearPlayer)
            attack = true;
    }
    private void Death()
    {
        Destroy(gameObject);
    }
    private void Hit()
    {
        takeHit = false;
    }


    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        takeHit = true;
        if (_currentHp <= 0)
        {
            dead = true;
            Invoke("Death", 1.5f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (!dead && collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            _nearPlayer = true;
            if (attack && player.alive && _coldownTimer >= _attackColdown)
            {
                _coldownTimer = 0;
                player.TakeDamage("Hp", _damage);
            }
            else if (player.alive == false)
            {
                _nearPlayer = false;
            }

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            attack = false;
            _nearPlayer = false;
        }
    }


}
