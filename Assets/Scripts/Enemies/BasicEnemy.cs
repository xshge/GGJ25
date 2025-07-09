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
    public float detectCircSize = 8f;
    public float RotAngleZ = 45;
    public LayerMask _castLayer;
    public bool isAlive;
    public Sprite _destroyedSprite;

    [SerializeField] private Transform _leftArm, _rigtArm;
    [SerializeField] private BasicEn_States states;
    // [SerializeField] private Bullet bltScrpt;

    private Transform Target;
    private Coroutine LookCoroutine;
    private Coroutine Sweeping;
    private GameObject levelSpawnPoint;
   

    public AudioSource ambientIshSound;
    public AudioSource hurtDeath;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public ParticleSystem oilLeaks;
    public static float xOffset = 3;

    void Start()
    {
        states.ChangeState(EnState.Sweeping);
        //note for later: change this assignment to a level loader;
        levelSpawnPoint = GameObject.FindWithTag("spawn");
        

    }
    public IEnumerator sweepDetect()
    {
        bool _detectedPlayer = false;

        //detecting the player;
        while (!_detectedPlayer && states.currentState != EnState.Talking)
        {
            float rY = Mathf.SmoothStep(0, RotAngleZ, Mathf.PingPong(Time.time * Speed, 1));
            _origin.rotation = Quaternion.Euler(0, 0, rY);

            //cast a circle cast to check for prescence
            RaycastHit2D result = Physics2D.CircleCast(_origin.position, detectCircSize, transform.right, 1f, _castLayer);
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
        float remainingDistance;
        float time = 0;
        //determine if the current shooting arm is assigned and wether the same;
        //determine which arm is closer;
        Vector3 leftLoc = _leftArm.GetChild(0).position;
        Vector3 rightLoc = _rigtArm.GetChild(0).position;

        //distance
        float _l = (leftLoc - plyr.position).magnitude;
        float _r = (rightLoc - plyr.position).magnitude;

        //keeping track of the arm
        Vector3 closerGunPoint = Vector3.zero;
        Vector3 lastArm = Vector3.zero;
        Transform currJoint = null;
        Transform currentArm = null;

        if (_l > _r)
        {
            closerGunPoint = rightLoc;
            Transform Joint = _rigtArm.parent.transform;
            currJoint = Joint;
            currentArm = _rigtArm;
        }
        else if (_r > _l)
        {
            closerGunPoint = leftLoc;
            Transform Joint = _leftArm.parent.transform;
            currJoint = Joint;
            currentArm = _leftArm;
        }
        else
        {
            if (closerGunPoint == Vector3.zero)
            {
                Transform Joint = null;
                if (transform.position.x > levelSpawnPoint.transform.position.x)
                {
                    closerGunPoint = rightLoc;
                    Joint = _rigtArm.parent.transform;
                    currentArm = _rigtArm;
                }
                else
                {
                    closerGunPoint = leftLoc;
                    Joint = _leftArm.parent.transform;
                    currentArm = _leftArm;
                }

                currJoint = Joint;
            }
        }

        //set to the joint rotation, joints are set when the arms are set so there is no need to keep track of switching;
        Quaternion initialRotation = transform.rotation;
        if (currJoint != null)
        {
            initialRotation = currJoint.rotation;
        }

        //determine the direction from the player to the gunpoint;
        Vector3 direction = plyr.position - closerGunPoint;
        float _offset = 90;

        Quaternion jointRotation = Quaternion.LookRotation(currJoint.forward, direction.normalized);

        //potential angle and axis;
        float angle = 0f;
        Vector3 axis = Vector3.zero;


        jointRotation.ToAngleAxis(out angle, out axis);

        //restrict when it needs to turn counterclockwise
        if (currJoint.position.x > _origin.position.x && angle > 160)
        {
            _offset = -90;

        }

        Vector3 offsetAngles = new Vector3(0, 0, angle - _offset);
        //create an offset for the rotation;
        jointRotation.eulerAngles = offsetAngles;

        /* Debug.Log("og Angle:" + angle);
         Debug.Log(jointRotation.eulerAngles);*/
        //direction to lool,      new direction of upward
        //Quaternion lookRotation = Quaternion.LookRotation( transform.forward, plyr.position - transform.position);

        // rotatearound 
        while (time < 1)
        {
            // transform.rotation = Quaternion.Slerp(initialRotation, lookRotation, time);
            currJoint.rotation = Quaternion.Slerp(initialRotation, jointRotation, time);
            // currentArm.RotateAround(currJoint.position, transform.forward, 20 * Time.deltaTime);
            time += Time.deltaTime * 5f;

            yield return null;
        }

        yield return new WaitForSeconds(2f);
        //instantiate bullets;
        GameObject _blt = Instantiate(_projectile, closerGunPoint, Quaternion.identity);
        lastArm = closerGunPoint;
        Bullet bScript = _blt.AddComponent<Bullet>();
        //debug ray;
        Vector3 StartPos = _blt.transform.position;
        remainingDistance = Vector3.Distance(StartPos, plyr.position);
        bScript.bulletMovement(remainingDistance, StartPos, plyr.position);

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
        _leftArm.gameObject.SetActive(false);
        _rigtArm.gameObject.SetActive(false);
        hurtDeath.clip = deathSound;
        hurtDeath.Play();
        ambientIshSound.Stop();
        oilLeaks.Pause();

        if(transform.name == "EndingBot")
        {
            Ending end = GetComponent<Ending>();
            end.enabled = true; 
        }

    }
    [YarnCommand("enter")]   
    public IEnumerator changePosition(GameObject player)
    {   
       /* GameObject enObj = GameObject.FindGameObjectWithTag("story");
        enObj.SetActive(true);*/
        states.ChangeState(EnState.Talking);
       //determine which side is closer to player
       float dist = Vector3.Distance(_origin.localPosition, player.transform.position);
       
        if(dist > 115)
        {
            xOffset = xOffset * -1;
        }
        Vector3 storyDestination = player.transform.position + new Vector3(xOffset,4,22.9f);
        float traveltime = 5f;
        while (traveltime > 0)
        {
            
            float step = 25f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, storyDestination,step);

            traveltime -= Time.deltaTime;
            yield return null;

        }
        ambientIshSound.volume = 0.4f;
        yield break;
    }
    [YarnCommand("resetRobot")]
    public void resetSelf()
    {
        ambientIshSound.volume = 0.8f;
        states.ChangeState(EnState.Sweeping);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_origin.position - transform.right * 1, 5f);
    }
}
