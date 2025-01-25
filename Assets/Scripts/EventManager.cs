using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<Vector3> saveCheckPoint;
    void Start()
    {
        
    }

    public static void _saving(Vector3 animalPos)
    {
        saveCheckPoint?.Invoke(animalPos);
    }
}
