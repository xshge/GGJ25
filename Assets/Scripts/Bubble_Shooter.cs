using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble_Shooter : MonoBehaviour
{
    // instantiation parameters
    public GameObject bubblePrefab;
    public Transform spawnerTransform;

    public Vector3 directionBasedSpawner;

    //Vectors
    private Vector2 bubbleDirection;

    private Vector2 playerPos;
    private Vector2 mousePos;

    [SerializeField] private Vector3[] sliderPositions;

    //bubble launch force
    public float launchForce;

    //checking how long the button has been held
    public float timer;
    public float timerMax;
    public Slider slideCharger;

    //UI
    public GameObject UISlider;

    Animator _animate;

    DaisyStates DaisyStateMachine;
    private void Start()
    {
        _animate = GetComponent<Animator>();
        DaisyStateMachine = GetComponent<DaisyStates>();
        UISlider.SetActive(false);
        slideCharger.maxValue = timerMax;
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
            UISlider.SetActive(false);
        }

        //checks if the left mouse button is being held down
        if (Input.GetMouseButton(0))
        {
            UISlider.SetActive(true);
            DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Shooting);
            timer += Time.deltaTime;
            slideCharger.value = timer;
            _animate.SetBool("Bubbling", true);
            CalculateDirection();
            //slideCharger.gameObject.transform.position = directionBasedSpawner;

            Debug.Log("mouse is held");
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

    void CalculateDirection()
    {
        //adjusting mouse position vector, since the camera's a little messed up
        mousePos += new Vector2(-.2f, 5.35f);

        //calculates the direction the bubble should travel in
        bubbleDirection = mousePos - playerPos;

        //Debug.Log("mousePos: " + mousePos);

        Vector2 bDNormalized = bubbleDirection.normalized;

        directionBasedSpawner = new Vector3((float)(bDNormalized.x * 4.64), (float)(bDNormalized.y * 4.64), 0);

        Debug.Log("bubble Direction: " + bubbleDirection);

        //calculating the angle the mouse is based on the center of daisy
        double radians = System.Math.Atan2(bDNormalized.x, bDNormalized.y);
        double angle = radians * (180 / System.Math.PI);

        //adjusting the slider's position based on where the bubble will launch
        // dont judge that im not using a for loop, the rotation on UI objects is like. off by 90 degrees and flipped over the horizontal access :skull:

        if(angle >= 0 && angle < 60)
        {
            UISlider.transform.localEulerAngles = new Vector3(0, 0, 60);
            UISlider.transform.localPosition = sliderPositions[0];
        }
        else if (angle >= 60 && angle < 120)
        {
            UISlider.transform.localEulerAngles = new Vector3(0, 0, 0);
            UISlider.transform.localPosition = sliderPositions[1];
        }
        else if (angle >= 120 && angle <= 180)
        {
            UISlider.transform.localEulerAngles = new Vector3(0, 0, 300);
            UISlider.transform.localPosition = sliderPositions[2];
        }

        else if (angle >= -60 && angle < 0)
        {
            UISlider.transform.localEulerAngles = new Vector3(0, 0, 120);
            UISlider.transform.localPosition = sliderPositions[3];
        }
        else if (angle >= -120 && angle < -60)
        {
            UISlider.transform.localEulerAngles = new Vector3(0, 0, 180);
            UISlider.transform.localPosition= sliderPositions[4];
        }
        else if (angle >= -180 && angle < -120)
        {
            UISlider.transform.localEulerAngles = new Vector3(0, 0, 240);
            UISlider.transform.localPosition= sliderPositions[5];
        }
    }

    void LaunchBubble()
    {
        //maxes out the time a bubble can be held for at 3 seconds
        if (timer > timerMax)
        {
            timer = timerMax;
        }

        CalculateDirection();

        ////adjusting mouse position vector, since the camera's a little messed up
        //mousePos += new Vector2(-.2f, 5.35f);

        ////calculates the direction the bubble should travel in
        //bubbleDirection = mousePos - playerPos;

        //Debug.Log("mousePos: " + mousePos);

        //Vector2 bDNormalized = bubbleDirection.normalized;

        //directionBasedSpawner = new Vector3((float)(bDNormalized.x * 4.64), (float)(bDNormalized.y * 4.64), 0);

        //Debug.Log("bubble Direction: " + bubbleDirection);

        //summons the bubble
        GameObject newBubble = Instantiate(bubblePrefab, spawnerTransform.position + directionBasedSpawner, Quaternion.identity, transform);

        //launches the bubble
         newBubble.GetComponent<Rigidbody2D>().AddForce(bubbleDirection.normalized * launchForce * timer, ForceMode2D.Impulse);

        //stop animation;
        _animate.SetBool("Bubbling", false);
        //resets timer
        timer = 0;
        slideCharger.value = 0;

        DaisyStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
    }
}
