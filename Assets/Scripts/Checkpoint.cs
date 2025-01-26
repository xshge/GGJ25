using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform currentCheckpoint;
    private void Start()
    {
        EventManager.saveCheckPoint += SaveCheckpoint;
    }
    void SaveCheckpoint(Vector3 checkpoint)
    {
        currentCheckpoint.position = checkpoint;
    }

    // called when player presses a button like "R"
    void ReturnToCheckpoint()
    {
        transform.position = currentCheckpoint.position;
    }
}
