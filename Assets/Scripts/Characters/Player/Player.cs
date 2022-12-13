using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] public TMP_Text _moneyCount;
    [SerializeField] public TMP_Text _keysCount;

    [Header("Stats")]
    private bool _alive = true;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _maxStamina;
    public int _money = 0;
    private float _currentHp;
    private float _currentStamina;

    public bool alive => _alive;

    [Header("Movement")]
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsLadder;
    [SerializeField] public Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] private Transform _cellChecker;
    [SerializeField] private float _cellCheckerRadius;
    [SerializeField] private float _speed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _jumpPower;
    private float _startSpeed;
    private bool _facingRight = true;
    private int _fallDamage;

    [Header("Animations")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimationKey;
    [SerializeField] private string _crouchAnimationKey;
    [SerializeField] private string _jumpAnimationKey;
    [SerializeField] private string _fallAnimationKey;
    [SerializeField] private string _attack1AnimationKey;
    [SerializeField] private string _attack2AnimationKey;
    [SerializeField] private string _deathAnimationKey;
    SpriteRenderer sprite;


    [Header("Keys")]
    private int _keys = 0;
    [SerializeField] private int _keysMax;
    [SerializeField] private Sprite _keyImage;
    [SerializeField] private Image[] _keysUI;

    [Header("Attack")]
    [SerializeField] private float _damage1;
    [SerializeField] private float _damage2;
    private bool _nearEnemy = false;
    private bool attack = false;
    private bool takeHit = false;




    // Start is called before the first frame update
    void Start()
    {
        _startSpeed = _speed;

        _currentHp = _maxHp;
        _currentStamina = _maxStamina;

        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _currentHp;

        _staminaSlider.maxValue = _maxStamina;
        _staminaSlider.value = _currentStamina;

        _moneyCount.text = _money.ToString();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var grounded = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        var onLadder = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsLadder);
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = _rigidbody.velocity;

        _animator.SetBool(_fallAnimationKey, !Input.GetButtonDown("Jump") && !grounded && !onLadder);

        if (_alive)
        {
            if (grounded || onLadder)
            {
                _rigidbody.velocity = new Vector2(_speed * horizontalAxis, velocity.y);
                _animator.SetBool(_runAnimationKey, horizontalAxis != 0);
                _animator.SetBool(_crouchAnimationKey, !_headCollider.enabled);
                _animator.SetBool(_jumpAnimationKey, Input.GetButtonDown("Jump") && _currentStamina > 20);
                _animator.SetBool(_attack1AnimationKey, Input.GetButtonDown("Fire1") && _currentStamina > 30);
                _animator.SetBool(_attack2AnimationKey, Input.GetButtonDown("Fire2") && _currentStamina > 40);

                if (Input.GetButtonDown("Jump") && _currentStamina > 20)
                {
                    _rigidbody.velocity = new Vector2(velocity.x, _jumpPower);
                    TakeDamage("Stamina", 20);
                }
                if (Input.GetButtonDown("Fire1") && _currentStamina > 30)
                {
                    TakeDamage("Stamina", 30);
                }
                if (Input.GetButtonDown("Fire2") && _currentStamina > 40)
                {
                    TakeDamage("Stamina", 40);
                }
            }
            if (takeHit)
            {
                sprite.color = new Color(225, 0, 0, 225);
                Invoke("Hit", 0.3f);
            }

            if (horizontalAxis < 0 && _facingRight || horizontalAxis > 0 && !_facingRight)
            {
                _facingRight = !_facingRight;
                MoverScripts.Flip(transform);
            }

            bool cellAbove = Physics2D.OverlapCircle(_cellChecker.position, _cellCheckerRadius, _whatIsGround);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                _headCollider.enabled = false;
                _bodyCollider.offset = new Vector2(0, 0);
                _speed = _crouchSpeed;
            }
            else if (!cellAbove)
            {
                _headCollider.enabled = true;
                _bodyCollider.offset = new Vector2(0, -0.02570852f);
                ResetSpeed();
            }

            if (_currentStamina < _maxStamina)
            {
                StaminaRecover();
            }


            for (int i = 0; i < _keysUI.Length; i++)
            {
                if (i < _keys)
                {
                    _keysUI[i].enabled = true;
                }
                else
                {
                    _keysUI[i].enabled = false;
                }
            }

        }
        else
        {
            _currentStamina = 0;
            _staminaSlider.value = _currentStamina;
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            RefreshScene();
        }

    }
    private void Hit()
    {
        takeHit = false;
        sprite.color = new Color(225, 225, 225, 225);
    }
    public void StaminaRecover()
    {
        _currentStamina += 10 * Time.deltaTime;
        _staminaSlider.value = _currentStamina;
    }
    public void TakeDamage(string stats, float damage)
    {
        if (stats == "Hp" && _currentHp != 0)
        {
            takeHit = true;
            _currentHp -= damage;
            _hpSlider.value = _currentHp;

            if (_currentHp <= 0)
            {
                _alive = false;
                MoverScripts.Die(_alive, _animator, _deathAnimationKey);
                Invoke("RefreshScene", 2);
            }
        }
        if (stats == "Stamina" && _currentStamina != 0)
        {
            _currentStamina -= damage;
            _staminaSlider.value = _currentStamina;
        }

    }
    private void ResetSpeed()
    {
        _speed = _startSpeed;
    }

    private void Attack()
    {
        if (_nearEnemy)
            attack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_cellChecker.position, _cellCheckerRadius);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("MovingPlatform"))
        {
            this.transform.parent = collision.transform;
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (_alive && collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _nearEnemy = true;

            if (attack)
            {
                enemy.TakeDamage(_damage1);
                attack = false;
            }
            if (attack && Input.GetButtonDown("Fire2") && _currentStamina > 40)
            {
                enemy.TakeDamage(_damage2);
                attack = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("MovingPlatform"))
        {
            this.transform.parent = null;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var grounded = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        Vector2 velocity = _rigidbody.velocity;
        Key key = other.GetComponent<Key>();
        Coin coin = other.GetComponent<Coin>();
        Heart heart = other.GetComponent<Heart>();
        if (grounded && velocity.y < -20)
        {
            _fallDamage = (int)Math.Round(velocity.y / 10) * -20;
            TakeDamage("Hp", _fallDamage);

        }
        if (key != null)
        {
            _keys += 1;
            Destroy(key.gameObject);
        }
        if (coin != null)
        {
            _money += 1;
            _moneyCount.text = _money.ToString();
            Destroy(coin.gameObject);
        }
        if (heart != null)
        {
            _currentHp += heart.HpPoint;
            _hpSlider.value = _currentHp;

            Destroy(heart.gameObject);
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Chest chest = other.GetComponent<Chest>();

        if (chest != null && _keys > 0 && Input.GetKey(KeyCode.E))
        {
            chest.Animator.SetBool(chest.AnimationKey, _keys > 0);
            _money += chest.Money;
            _moneyCount.text = _money.ToString();
            _keys -= 1;
        }
    }

    private void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
