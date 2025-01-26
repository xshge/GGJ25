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
    private void Start()
    {
        _animate = GetComponent<Animator>();
    }
    void Update()
    {
        //tracks where the player is and where the mouse is
        playerPos = transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Xinyi can you do the input system stuff for me please and thank you
        if (Input.GetMouseButtonUp(0))
        {
            LaunchBubble();
        }

        //checks if the left mouse button is being held down
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            slideCharger.value = timer;
            _animate.SetBool("Bubbling", true);
        }
    }

    void LaunchBubble()
    {
        //maxes out the time a bubble can be held for at 3 seconds
        if (timer > 3)
        {
            timer = 3;
        }

        //calculates the direction the bubble should travel in
        bubbleDirection = mousePos - playerPos;

        //if we end up chosing to aim the bubble, i'm gonna max out the x and y values of the bubbleDirection vector at like 2 & scale it so they're at the same speed rather than speed being impacted by how far away from the player the mouse is clicked
        Debug.Log(bubbleDirection);

        //summons the bubble
        GameObject newBubble = Instantiate(bubblePrefab, spawnerTransform.position, Quaternion.identity, transform);

        //launches the bubble
        if (BubbleAimable)
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce(bubbleDirection * .5f * timer, ForceMode2D.Impulse);
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
    }
}
