using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    Vector3 MoveVector,ogCamPos;
    string currPos, lastPos;
    public Rigidbody _pRB;
    private GameObject _camera;
    float timer = 0f;
    [SerializeField] private float _sideMoveForce;
    [SerializeField] private float _upwardForce;
    public bool isunderWater = false;
    private Vector3 camVelocity = Vector3.zero;
    public GameObject realDaisy;
    Animator _animate;
    public SpriteRenderer daisy;
    float levelChange = 1;
    SpriteRenderer _dSprite;
    Camera Cam;

    public DaisyStates DaisyStateMachine;

    public AudioSource deathSound;

    void Start()
    {
        _pRB = GetComponent<Rigidbody>();
        _animate = realDaisy.GetComponent<Animator>();
        _dSprite = realDaisy.GetComponent<SpriteRenderer>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        ogCamPos = _camera.transform.localPosition;
        Cam = _camera.GetComponent<Camera>();
        deathSound = GetComponent<AudioSource>();
    }
    void Update()
    {
        timer -= Time.deltaTime;
      
        //handle flipping
        if (Input.GetKeyDown(KeyCode.D))
        {
           currPos = KeyCode.D.ToString();
            _dSprite.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currPos = KeyCode.A.ToString();
            _dSprite.flipX = false;
        }

        if (_pRB != null)
        {
            //calculate vector and updating it.
            MoveVector = new Vector3(Input.GetAxis("Horizontal"), 0).normalized;


        }


        if (Input.GetKeyDown(KeyCode.W) && timer <= 0f)
        {
            timer = 0.75f; // changed timer from .75f to 1f 
            _pRB.AddForce(new Vector3(0, 1) * _upwardForce * levelChange, ForceMode.Impulse); //added levelChange in case we decide to include that again
            _animate.SetBool("up", true);
            currPos = KeyCode.W.ToString();
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Floating);


           
        }
        else
        {
            _animate.SetBool("up", false);
            
        }

        if(_pRB.velocity.y < 0 && DaisyStateMachine.daisyState != BubbleGirlState.Falling)
        {
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Falling);
            
        }

       
        

        //updating last direction 
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            lastPos = currPos;

            //This replace the previous reset to Idle state code with 0 velocity, as that code was conflicting with the animation states. 
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
        }


        moveCamera();
    }
    private void FixedUpdate()
    {

      
     
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
           
            // Debug.Log(MoveVector); all the actuall physic movement
            //Debug.Log("pressing: " +  currentKey + Enum.GetName(typeof(KeyCode), currentKey));
            if (_pRB.useGravity)
            {
                if (isunderWater)
                {
                    // _pRB.velocity = MoveVector * levelChange * (_sideMoveForce/2);
                    _pRB.AddForce(MoveVector * levelChange * (_sideMoveForce), ForceMode.Force);
                    //_pRB.AddForce(Vector3.up * 10f);
                }

            }
            
        }
        else
        {
           

        }


        
        //Debug.Log("mag" + _pRB.velocity.magnitude);
     
     
        
    }

    void moveCamera()
    {
        float lerpTime = 0.65f;
      /* Plane[] CameraPlanes = GeometryUtility.CalculateFrustumPlanes(Cam);
        bool isInView = GeometryUtility.TestPlanesAABB(CameraPlanes, daisy.bounds);*/

        Vector3 direction = determineDirection();
        float dist = _pRB.velocity.magnitude * 0.25f;
        Vector3 newPos = realDaisy.transform.localPosition + (direction * dist);
        newPos = new Vector3(newPos.x, newPos.y, ogCamPos.z);
        if (DaisyStateMachine.daisyState == BubbleGirlState.Falling)
        {
           newPos = new Vector3(realDaisy.transform.localPosition.x, realDaisy.transform.localPosition.y, ogCamPos.z);
        }       
        _camera.transform.localPosition = Vector3.SmoothDamp(_camera.transform.localPosition, newPos, ref camVelocity, lerpTime);
    }
    Vector3 determineDirection()
    {   
        Vector3 dit = Vector3.zero;
        switch (currPos)
        {
            case "A":
                dit = - Vector3.right;
                break;
            case "D":
                dit = Vector3.right;
                break;
            case "W":
                if(timer <= 0) dit = Vector3.up;
                break;
            default: return Vector3.zero;
        }

        return dit;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("name: " + collision.transform.name + "tag: " + collision.transform.tag);
        if (collision.gameObject.CompareTag("obstacle"))
        {
            //play animation;
            _animate.SetBool("Hit", true);
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Dead);
            StartCoroutine(Dying());
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("LevelChange"))
        {
            levelChange += 0.25f;
            Debug.Log("v" + _pRB.velocity);
        };

    }
    void DialogueState()
    {
        DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Talking);

        _pRB.isKinematic = true;
        //TODO: set bool for idle animation;




    }
    IEnumerator Dying()
    {
        deathSound.Play(); 
      
        _pRB.useGravity = false;
        _pRB.isKinematic = true;

        //reset enemies states
        EventManager._resetsEn(EnState.Sweeping);
        DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Dead);
        yield return new WaitForSeconds(1f);
        daisy.enabled = false;
        yield return new WaitForEndOfFrame();
        EventManager._respawn(daisy);
        yield return new WaitForEndOfFrame();
        _animate.SetBool("Hit", false);
        //DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
        yield break;
    }
    #region Redacted InputSystem code
    private void MovementInput(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector3>();
    }
    public void FloatUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _pRB.AddForce(new Vector3(0, 1) * _upwardForce, ForceMode.Impulse);
        }
    }
    #endregion
}
