using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Vector3 currentCheckpoint;
    Rigidbody rb;
    Animator _animate;
    private void Start()
    {
        EventManager.saveCheckPoint += SaveCheckpoint;
        EventManager.restoreCheckPoint += ReturnToCheckpoint;
        rb = GetComponentInParent<Rigidbody>();
        _animate = GetComponent<Animator>();
        currentCheckpoint = transform.position;
    }
    void SaveCheckpoint(Vector3 checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    // called when player presses a button like "R"
    void ReturnToCheckpoint(SpriteRenderer ply)
    {
        _animate.SetBool("Hit", false);
        ply.enabled = true;
        transform.position = currentCheckpoint;
        rb.isKinematic = false;

    }
}
