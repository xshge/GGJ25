using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    Vector2 MoveVector;
    Rigidbody2D _pRB;

    [SerializeField] private float _sideMoveForce;
    [SerializeField] private float _upwardForce;



    void Start()
    {
        _pRB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (_pRB != null)
        {
            MoveVector = new Vector2(Input.GetAxis("Horizontal"), 0).normalized;
           
        }
       
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.A) )
            {
            // Debug.Log(MoveVector);
            _pRB.velocity = MoveVector * _sideMoveForce;
            }
        if(Input.GetKeyDown(KeyCode.Space)) _pRB.AddForce(new Vector2(0, 1) * _upwardForce, ForceMode2D.Impulse);
    }
    Vector2 calcMove()
    {
        Vector2 moveVector = Vector2.left * Input.GetAxis("Horizontal");
        Debug.Log(moveVector);
        return moveVector;
    }
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
}
