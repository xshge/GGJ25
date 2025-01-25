using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    Vector2 MoveVector;
    Rigidbody2D _pRB;
    float timer = 0f;
    [SerializeField] private float _sideMoveForce;
    [SerializeField] private float _upwardForce;
    public bool isunderWater = false;


    void Start()
    {
        _pRB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {   
        timer -= Time.deltaTime;
        
        if (_pRB != null)
        {   
            //calculate vector and updating it.
            MoveVector = new Vector2(Input.GetAxis("Horizontal"), 0).normalized;
           
        }
        if (Input.GetKeyDown(KeyCode.W) && timer <= 0f)
        {   
            timer = 0.75f;
            _pRB.AddForce(new Vector2(0, 1) * _upwardForce, ForceMode2D.Impulse);
            
        }

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.A) )
            {
                // Debug.Log(MoveVector); all the actuall physic movement
                if(_pRB.bodyType == RigidbodyType2D.Dynamic)
            {
                if (isunderWater)
                {
                     _pRB.velocity = MoveVector * (_sideMoveForce/2);
                }
                else
                {
                    _pRB.velocity = MoveVector * _sideMoveForce;
                }
               
            }
               
            }
        
    }
    void UnderWaterMove()
    {

    }

    #region Redacted InputSystem code
    private void MovementInput(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();
    }
    public void FloatUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _pRB.AddForce(new Vector2(0, 1) * _upwardForce, ForceMode2D.Impulse);
        }
    }
    #endregion
}
