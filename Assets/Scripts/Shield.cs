using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public DaisyStates daisyStates;
    public ShieldStates shieldState;

    public GameObject daisyBubble;
    public GameObject daisy3D;

    public AudioSource shieldPopSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        shieldState = daisyStates.shieldState;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (shieldState == ShieldStates.Active)
        {
            if (other.gameObject != daisy3D)
            {
                shieldPopSound.Play();

                daisyStates.ShieldPopped();
                //Debug.Log(other.transform.name);
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (shieldState == ShieldStates.Active)
        {

            if (other.gameObject != daisyBubble)
            {
                shieldPopSound.Play();
                daisyStates.ShieldPopped();

                
            }
        }
    }
}
