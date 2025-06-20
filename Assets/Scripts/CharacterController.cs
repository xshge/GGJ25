using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    Vector3 MoveVector;
    public Rigidbody _pRB;
    float timer = 0f;
    [SerializeField] private float _sideMoveForce;
    [SerializeField] private float _upwardForce;
    public bool isunderWater = false;

    public GameObject realDaisy;
    Animator _animate;
    public SpriteRenderer daisy;
    float levelChange = 1;
    SpriteRenderer _dSprite;

    public DaisyStates DaisyStateMachine;

    void Start()
    {
        _pRB = GetComponent<Rigidbody>();
        _animate = realDaisy.GetComponent<Animator>();
        _dSprite = realDaisy.GetComponent<SpriteRenderer>();
      
    }
    void Update()
    {
        timer -= Time.deltaTime;
        //handle flipping
        if (Input.GetKeyDown(KeyCode.D))
        {
            _dSprite.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _dSprite.flipX = false;
        }

        if (_pRB != null)
        {
            //calculate vector and updating it.
            MoveVector = new Vector3(Input.GetAxis("Horizontal"), 0).normalized;

        }
        if (Input.GetKeyDown(KeyCode.W) && timer <= 0f)
        {
            timer = 1f; // changed timer from .75f to 1f 
            _pRB.AddForce(new Vector3(0, 1) * _upwardForce * levelChange, ForceMode.Impulse); //added levelChange in case we decide to include that again
            _animate.SetBool("up", true);
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

        if(_pRB.velocity.x == 0 && _pRB.velocity.y == 0)
        {
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
        }

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
                    _pRB.AddForce(Vector3.up * 5f);
                }

            }
            _animate.SetBool("moving", true);
        }
        else
        {
            _animate.SetBool("moving", false);
           

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("name: " + collision.transform.name + "tag: " + collision.transform.tag);
        if (collision.gameObject.CompareTag("obstacle"))
        {   
           
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Dead);
            StartCoroutine(Dying());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LevelChange"))
        {
            levelChange += 0.25f;
            Debug.Log("v" + _pRB.velocity);
        }
    }
    IEnumerator Dying()
    {
        //play animation;
        _animate.SetBool("Hit", true);
        _pRB.useGravity = false;
        _pRB.isKinematic = true;

        //reset enemies states
        EventManager._resetsEn(EnState.Sweeping);

        yield return new WaitForSeconds(2f);
        daisy.enabled = false;
        yield return new WaitForEndOfFrame();
        EventManager._respawn(daisy);

        DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
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
