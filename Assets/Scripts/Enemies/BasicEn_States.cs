using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnState
{
    Sweeping,Shooting, Death,
}
public class BasicEn_States : MonoBehaviour
{   
    public EnState currentState;

    void Start()
    {
        
    }

    public void ChangeState(EnState state)
    {
        currentState = state;

        if(currentState == EnState.Death)
        {
            //trigger event manager.
            //play death animation;
            gameObject.SetActive(false);
        }
    }
}
