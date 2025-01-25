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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //tracks where the player is and where the mouse is
        playerPos = transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Xinyi can you do the input system stuff for me please and thank you
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBubble();
        }
    }

    void LaunchBubble()
    {
        //calculates the direction the bubble should travel in
        bubbleDirection = mousePos - playerPos;

        //summons the bubble
        GameObject newBubble = Instantiate(bubblePrefab, spawnerTransform.position, Quaternion.identity);

        //launches the bubble
        if (BubbleAimable)
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce(bubbleDirection * .5f, ForceMode2D.Impulse);
        }
        else // if the bubble needs to shoot straight up
        {
            newBubble.GetComponent<Rigidbody2D>().AddForce( new Vector2(0,launchForce), ForceMode2D.Impulse);
        }
    }
}
