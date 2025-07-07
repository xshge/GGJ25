using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class BasicEnemy : MonoBehaviour
{
    public GameObject _projectile;
    public Transform _origin;
    public float Speed = 1f;
    public float RotAngleZ = 45; //changed in the inspector; 
    public LayerMask _castLayer;
    public bool isAlive;

    [SerializeField] private Transform _gunpoint;
    [SerializeField] private BasicEn_States states;
    
  
    private Coroutine LookCoroutine;
    private Coroutine Sweeping;
    private GameObject levelSpawnPoint;

    public AudioSource ambientIshSound;
    public AudioSource hurtDeath;
    public AudioClip hurtSound;
    public AudioClip deathSound;
  
    void Start()
    {
        //Sweeping = StartCoroutine(sweepDetect());
        states.ChangeState(EnState.Sweeping);
        //note for later: change this assignment to a level loader;
        levelSpawnPoint = GameObject.FindWithTag("spawn");

    }
    private IEnumerator sweepDetect()
    {
        bool _detectedPlayer = false;

        //detecting the player;
        while (!_detectedPlayer && states.currentState == EnState.Sweeping)
        {
            float rY = Mathf.SmoothStep(0, RotAngleZ, Mathf.PingPong(Time.time * Speed, 1));
            transform.rotation = Quaternion.Euler(0, 0, rY);

            //cast a circle cast to check for prescence
            RaycastHit2D result = Physics2D.CircleCast(_origin.position, 5f, transform.right, 1f,_castLayer);
            if (result)
            {
                
                _detectedPlayer = true;
                states.ChangeState(EnState.Shooting);
                //coroutine for tracking player movements and face toward them while rotating on the z-axis;
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
      
        float remainingDistance;
        float time = 0;

        //determine if the current shooting arm is assigned and wether the same;
        //determine which arm is closer;
        Vector3 leftLoc = _leftArm.GetChild(0).position;
        Vector3 rightLoc = _rigtArm.GetChild(0).position;

        //Vector3 direction = plyr.position - transform.position;
        Quaternion initialRotation = transform.rotation;
        Quaternion lookRotation = Quaternion.LookRotation( transform.forward, plyr.position - transform.position);
       
        //roate to look;
        while (time < 1)
        {
           transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);

            time += Time.deltaTime * 5;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //instantiate bullets;
        GameObject _blt = Instantiate(_projectile, _gunpoint.position, Quaternion.identity);
        Bullet bScript = _blt.AddComponent<Bullet>();
     
        Vector3 StartPos = _blt.transform.position;
        remainingDistance = Vector3.Distance(StartPos, plyr.position);
        bScript.bulletMovement(remainingDistance,StartPos,plyr.position);
  
        //check to see if still alive, if it is then keep on shooting;
        if (states.currentState == EnState.Shooting)
        {
            StartCoroutine(LookAt(plyr));
        }


       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        //determeine whether Daisy's bubble have hit yet. 
        if (collision.transform.CompareTag("bubble"))
        {
            states.ChangeState(EnState.Death);
        }
    }
    public void stop()
    {
        StopAllCoroutines();
        _leftArm.gameObject.SetActive(false);
        _rigtArm.gameObject.SetActive(false);
        hurtDeath.clip = deathSound;
        hurtDeath.Play();
        ambientIshSound.Stop();
        
    }
    [YarnCommand("enter")]   
    public IEnumerator changePosition(GameObject player)
    {
        states.ChangeState(EnState.Talking);
        Vector3 storyDestination = player.transform.position + new Vector3(-2,-20,22.9f);
        float traveltime = 5f;
        while (traveltime > 0)
        {
            
            float step = 10f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, storyDestination,step);

            traveltime -= Time.deltaTime;
            yield return null;

        }
       
        yield break;
    }
    [YarnCommand("resetRobot")]
    public void resetSelf()
    {
        states.ChangeState(EnState.Sweeping);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_origin.position - transform.right * 1, 5f);
    }
}
