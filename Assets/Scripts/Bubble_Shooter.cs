using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_Shooter : MonoBehaviour
{
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

    // Update is called once per frame
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
        GameObject newBubble = Instantiate(bubblePrefab, spawnerTransform.position, Quaternion.identity);

        //launches the bubble
        if (BubbleAimable)
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce(bubbleDirection * .5f * timer, ForceMode2D.Impulse);
        }
        else // if the bubble needs to shoot straight up
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce( new Vector2(0,launchForce * timer), ForceMode2D.Impulse);
        }

        //resets timer
        timer = 0;
    }
}
