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

    [SerializeField] private Transform _leftArm, _rigtArm;
    [SerializeField] private BasicEn_States states;
   // [SerializeField] private Bullet bltScrpt;
    
    private Transform Target;
    private Coroutine LookCoroutine;
    private Coroutine Sweeping;
    private GameObject levelSpawnPoint; 

    void Start()
    {
        Sweeping = StartCoroutine(sweepDetect());
        //note for later: change this assignment to a level loader;
        levelSpawnPoint = GameObject.FindWithTag("spawn");

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
        //determine if the current shooting arm is assigned and wether the same;
        //determine which arm is closer;
        Vector3 leftLoc = _leftArm.GetComponentInChildren<Transform>().position;
        Vector3 rightLoc = _rigtArm.GetComponentInChildren<Transform>().position;

        float _l = (leftLoc - plyr.position).magnitude;
        float _r = (rightLoc - plyr.position).magnitude;
        Vector3 closerArm = Vector3.zero;

        if(_l > _r)
        {
            closerArm = rightLoc;
        }else if( _r > _l)
        {
            closerArm = leftLoc;
        }
        else
        {
            if(closerArm == Vector3.zero)
            {
                if(transform.position.x > levelSpawnPoint.transform.position.x)
                {
                    closerArm = rightLoc;
                }
                else
                {
                    closerArm = leftLoc;
                }

            }
        }
        
       
        Vector3 direction = plyr.position - transform.position;
        Quaternion initialRotation = transform.rotation;
        Quaternion lookRotation = Quaternion.LookRotation( transform.forward, plyr.position - transform.position);
       
        while (time < 1)
        {
           transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * 5f;

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        //instantiate bullets;
        GameObject _blt = Instantiate(_projectile, closerArm, Quaternion.identity);
        Bullet bScript = _blt.AddComponent<Bullet>();
        //debug ray;
        Vector3 StartPos = _blt.transform.position;
        remainingDistance = Vector3.Distance(StartPos, plyr.position);
        bScript.bulletMovement(remainingDistance,StartPos,plyr.position);
  
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
    public void stop()
    {
        StopAllCoroutines();
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_origin.position - transform.right * 1, 5f);
    }
}
