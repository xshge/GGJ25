using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public DaisyStates daisyStates;
    public ShieldStates shieldState;

    public GameObject daisyBubble;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject != daisyBubble)
        {
            StartCoroutine(daisyStates.ChangeBubbleState(ShieldStates.Popped));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject != daisyBubble)
        {
            StartCoroutine(daisyStates.ChangeBubbleState(ShieldStates.Popped));
        }
    }
}
