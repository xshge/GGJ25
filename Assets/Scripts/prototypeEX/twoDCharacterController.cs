using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoDCharacterController : MonoBehaviour
{
    Vector3 MoveVector;
    Rigidbody2D _pRB;
    float timer = 0f;
    [SerializeField] private float _sideMoveForce;
    [SerializeField] private float _upwardForce;
    public bool isunderWater = false;

    public GameObject realDaisy;
    Animator _animate;
    public SpriteRenderer daisy;
    float levelChange = 1;
    void Start()
    {
        _pRB = GetComponent<Rigidbody2D>();
        _animate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (_pRB != null)
        {
            //calculate vector and updating it.
            MoveVector = new Vector3(Input.GetAxis("Horizontal"), 0).normalized;

        }
        if (Input.GetKeyDown(KeyCode.W) && timer <= 0f)
        {
            timer = 0.75f;
            _pRB.AddForce(new Vector3(0, 1) * _upwardForce, ForceMode2D.Impulse);
            _animate.SetBool("up", true);
        }
        else
        {
            _animate.SetBool("up", false);
        }

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            // Debug.Log(MoveVector); all the actuall physic movement
         
            if (isunderWater)
                {
                    // _pRB.velocity = MoveVector * levelChange * (_sideMoveForce/2);
                    _pRB.AddForce(MoveVector * levelChange * (_sideMoveForce), ForceMode2D.Force);
                    _pRB.AddForce(Vector3.up * 5f);
                }

       
            _animate.SetBool("moving", true);
        }
        else
        {
            _animate.SetBool("moving", false);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
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
        _pRB.isKinematic = true;
        yield return new WaitForSeconds(1f);
        daisy.enabled = false;
        yield return new WaitForSeconds(1.5f);
        EventManager._respawn(daisy);
        yield break;
    }
}
