using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private int _HpPoint;

    public int HpPoint => _HpPoint;
}
