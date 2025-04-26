using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<Vector3> saveCheckPoint;
    public static event Action<SpriteRenderer> restoreCheckPoint;
    public static event Action<EnState> resetEnemies;
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
    public static void _resetsEn(EnState st)
    {
        resetEnemies?.Invoke(st);
    }
}
