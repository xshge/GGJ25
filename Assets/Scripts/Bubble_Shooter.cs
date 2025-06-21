using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble_Shooter : MonoBehaviour
{
    //TOMORROW: ADD COLLISION DETECTION ON BUBBLES, THE CLEANING MECHANIC, ETC


    //for testing purposes, whether we're having the bubble shoot straight up or based on where the player aims
    public bool BubbleAimable;

    // instantiation parameters
    public GameObject bubblePrefab;
    public Transform spawnerTransform;

    public Vector3 directionBasedSpawner;

    //Vectors
    private Vector2 bubbleDirection;

    private Vector2 playerPos;
    private Vector2 mousePos;

    //bubble launch force
    public float launchForce;

    //checking how long the button has been held
    public float timer;
    public Slider slideCharger;

    Animator _animate;

    DaisyStates DaisyStateMachine;
    private void Start()
    {
        _animate = GetComponent<Animator>();
        DaisyStateMachine = GetComponent<DaisyStates>();
    }
    void Update()
    {
        //tracks where the player is and where the mouse is
        playerPos = transform.position;
        mousePos = GetWorldPositionOnPlane(Input.mousePosition,0);

        // Xinyi can you do the input system stuff for me please and thank you
        if (Input.GetMouseButtonUp(0))
        {
            LaunchBubble();
        }

        //checks if the left mouse button is being held down
        if (Input.GetMouseButton(0))
        {
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Shooting);
            timer += Time.deltaTime;
            slideCharger.value = timer;
            _animate.SetBool("Bubbling", true);
        }
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    void LaunchBubble()
    {
        //maxes out the time a bubble can be held for at 3 seconds
        if (timer > 3)
        {
            timer = 3;
        }

        //adjusting mouse position vector, since the camera's a little messed up
        mousePos += new Vector2(-.2f, 5.35f);

        //calculates the direction the bubble should travel in
        bubbleDirection = mousePos - playerPos;

        //Debug.Log("mousePos: " + mousePos);

        Vector2 bDNormalized = bubbleDirection.normalized;

        directionBasedSpawner = new Vector3((float)(bDNormalized.x * 4.64), (float)(bDNormalized.y * 4.64), 0);

        Debug.Log("bubble Direction: " + bubbleDirection);

        //summons the bubble
        GameObject newBubble = Instantiate(bubblePrefab, spawnerTransform.position + directionBasedSpawner, Quaternion.identity, transform);

        //launches the bubble
        if (BubbleAimable)
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce(bubbleDirection.normalized * launchForce * timer, ForceMode2D.Impulse);
        }
        else // if the bubble needs to shoot straight up
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce( new Vector2(0,launchForce * timer), ForceMode2D.Impulse);
        }
        //stop animation;
        _animate.SetBool("Bubbling", false);
        //resets timer
        timer = 0;
        slideCharger.value = 0;

        DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
    }
}
