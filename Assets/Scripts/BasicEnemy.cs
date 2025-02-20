using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject _projectile;
    public float Speed = 1f;
    
    private Transform Target;
    private Coroutine LookCoroutine;

    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Target = collision.transform;
        StartRotating();

    }

    public void StartRotating()
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Debug.Log("lookinh");
        yield return null;
    }
}
