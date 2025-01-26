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
    void FixedUpdate()
    {
        if (_pRB != null)
        {
            _pRB.AddForce(MoveVector * _sideMoveForce);
        }
       
    }
    public void MovementInput(InputAction.CallbackContext context)
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
