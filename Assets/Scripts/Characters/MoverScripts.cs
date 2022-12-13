using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverScripts : MonoBehaviour
{
    public static void Flip(Transform transform) => transform.Rotate(0, 180, 0);
    public static void Die(bool alive, Animator animator, string animationKey) => animator.SetBool(animationKey, !alive);

}
