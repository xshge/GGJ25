using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyEvents : MonoBehaviour
{
    public static event Action<float, Vector3, Vector3> startBullet;

 }
