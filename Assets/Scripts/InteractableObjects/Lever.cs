using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    private bool _nearPlayer = false;
    [SerializeField] public bool _isLeverActive = false;
    public Animator _animator;
    public string _leverAnimationKey;

    void Update()
    {
        if (_nearPlayer && Input.GetKeyDown(KeyCode.E))
        {
            _isLeverActive = !_isLeverActive;
            _animator.SetBool(_leverAnimationKey, _isLeverActive);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        _nearPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        _nearPlayer = false;
    }
}
