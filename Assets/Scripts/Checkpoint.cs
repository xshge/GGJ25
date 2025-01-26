using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector3 currentCheckpoint;
    Rigidbody2D rb;
    private void Start()
    {
        EventManager.saveCheckPoint += SaveCheckpoint;
        EventManager.restoreCheckPoint += ReturnToCheckpoint;
        rb = GetComponent<Rigidbody2D>();
        currentCheckpoint = transform.position;
    }
    void SaveCheckpoint(Vector3 checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    // called when player presses a button like "R"
    void ReturnToCheckpoint(SpriteRenderer ply)
    {
        ply.enabled = true;
        transform.position = currentCheckpoint;
        rb.bodyType = RigidbodyType2D.Dynamic;

    }
}
