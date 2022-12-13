using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _Money;
    [SerializeField] private string _chestAnimationKey;

    public int Money => _Money;
    public Animator Animator => _animator;
    public string AnimationKey => _chestAnimationKey;

}
