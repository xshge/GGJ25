using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : MonoBehaviour
{
    public GameObject _projectile;
    public Transform _origin;
    public float Speed = 1f;
    public float RotAngleZ = 45;
    public LayerMask _castLayer;
    public bool isAlive;

    [SerializeField] private Transform _gunpoint;
    [SerializeField] private BasicEn_States states;

    
    private Transform Target;
    private Coroutine LookCoroutine;
    private Coroutine Sweeping;
  

    void Start()
    {
        Sweeping = StartCoroutine(sweepDetect());

    }
    private IEnumerator sweepDetect()
    {
        bool _detectedPlayer = false;

        //detecting the player;
        while (!_detectedPlayer)
        {
            float rY = Mathf.SmoothStep(0, RotAngleZ, Mathf.PingPong(Time.time * Speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);

            //cast a circle cast to check for prescence
            RaycastHit2D result = Physics2D.CircleCast(_origin.position, 5f, transform.right, 1f,_castLayer);
            if (result)
            {
                
                _detectedPlayer = true;
                states.ChangeState(EnState.Shooting);
                StartRotating(result.transform);
            }
            yield return null;
        }
        
    }

    public void StartRotating(Transform player)
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(player));
    }

    private IEnumerator LookAt(Transform plyr)
    {
        //Debug.Log("lookinh");
        float remainingDistance, dist;
        float bulletSpeed = 3;
        float time = 0;

        Vector3 direction = plyr.position - transform.position;
        Quaternion initialRotation = transform.rotation;
        Quaternion lookRotation = Quaternion.LookRotation( transform.forward, plyr.position - transform.position);
       
        while (time < 1)
        {
           transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * 5;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //instantiate bullets;
        GameObject _blt = Instantiate(_projectile, _gunpoint.position, Quaternion.identity);
        //debug ray;
        Vector3 StartPos = _blt.transform.position;
        remainingDistance = Vector3.Distance(StartPos, plyr.position);
        dist = remainingDistance;

        //bullet traveling;
        while (remainingDistance > 0f)
        {
            _blt.transform.position = Vector3.Lerp(StartPos, plyr.position, 1 - (remainingDistance / dist));
            remainingDistance -= bulletSpeed * Time.deltaTime;
            yield return null;
        }

        if (states.currentState == EnState.Shooting)
        {
            StartCoroutine(LookAt(plyr));
        }


       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("bubble"))
        {
            states.ChangeState(EnState.Death);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_origin.position - transform.right * 1, 5f);
    }
}
