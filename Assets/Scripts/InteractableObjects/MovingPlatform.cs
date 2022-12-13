using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _startPositionX;
    private float _startPositionY;

    [SerializeField] private GameObject _obj;

    [SerializeField] private bool _horizontal;
    [SerializeField] private bool _vertical;

    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    [SerializeField] private bool _movingForward;

    void Start()
    {
        _startPositionX = transform.position.x;
        _startPositionY = transform.position.y;
    }
    void Update()
    {
        Lever lever = _obj.GetComponent<Lever>();
        bool _leverActive = lever._isLeverActive;

        if(_leverActive && _horizontal)
        {
            if (transform.position.x > _startPositionX + _maxDistance)
            {
                _movingForward = false;
            }
            else if (transform.position.x < _startPositionX - _minDistance)
            {
                _movingForward = true;
            }
            if (_movingForward)
            {
                transform.position = new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - _speed * Time.deltaTime, transform.position.y);
            }
        }
        if (_leverActive && _vertical)
        {
            if (transform.position.y > _startPositionY + _maxDistance)
            {
                _movingForward = false;
            }
            else if (transform.position.y < _startPositionY - _minDistance)
            {
                _movingForward = true;
            }
            if (_movingForward)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + _speed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector2( transform.position.x, transform.position.y - _speed * Time.deltaTime);
            }
        }
    }
}
