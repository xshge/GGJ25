using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePop : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bubble collided");

        //Once I get this working, it'll probably have a short animation where it "pops" and then the object gets destroyed
    }
}
