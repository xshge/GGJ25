using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<Vector3> saveCheckPoint;
    public static event Action<SpriteRenderer> restoreCheckPoint;
    public static event Action startScene;
    void Start()
    {
        
    }

    public static void _saving(Vector3 animalPos)
    {
        saveCheckPoint?.Invoke(animalPos);
    }
    public static void _respawn(SpriteRenderer ply)
    {
        restoreCheckPoint?.Invoke(ply);
    }
}
