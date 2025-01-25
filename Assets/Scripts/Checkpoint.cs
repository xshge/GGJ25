using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform currentCheckpoint;
    void SaveCheckpoint(GameObject checkpoint)
    {
        currentCheckpoint = checkpoint.transform;
    }

    // called when player presses a button like "R"
    void ReturnToCheckpoint()
    {
        transform.position = currentCheckpoint.position;
    }
}
