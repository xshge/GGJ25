using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject _projectile;
    public Transform _origin;
    public float Speed = 1f;
    public float RotAngleZ = 45;
    public LayerMask _castLayer;
    public bool isActive;
    
    private Transform Target;
    private Coroutine LookCoroutine;
    private Coroutine Sweeping;

    void Start()
    {
        Sweeping = StartCoroutine(sweepDetect());

    }
    private IEnumerator sweepDetect()
    {
        //detecting the player;
        while (true)
        {
            float rY = Mathf.SmoothStep(0, RotAngleZ, Mathf.PingPong(Time.time * Speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);

            //cast a circle cast to check for prescence
            RaycastHit2D result = Physics2D.CircleCast(_origin.position, 5f, transform.right, 1f,_castLayer);
            if (result)
            {
                Debug.Log(result.collider);
            }
            yield return null;
        }
        
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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_origin.position - transform.right * 1, 5f);
    }
}
