using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitating : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _startPositionY;

    [SerializeField] private float _distance;

    private bool _movingForward;

    void Start()
    {
        _startPositionY = transform.position.y;
    }
    void Update()
    {
        if (transform.position.y > _startPositionY + _distance)
        {
            _movingForward = false;
        }
        else if (transform.position.y < _startPositionY - _distance)
        {
            _movingForward = true;
        }
        if (_movingForward)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + _speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - _speed * Time.deltaTime);
        }
    }
}
