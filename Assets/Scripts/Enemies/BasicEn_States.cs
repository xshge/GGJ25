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
        EventManager.resetEnemies += ChangeState;
    }

    public void ChangeState(EnState state)
    {
        currentState = state;

        if(currentState == EnState.Death)
        {
            //trigger event manager.
            BasicEnemy be = GetComponent<BasicEnemy>();
            be.stop();
            EventManager.resetEnemies -= ChangeState;
            //play death animation;
            gameObject.SetActive(false);
        }else if(currentState == EnState.Sweeping)
        {
            BasicEnemy be = GetComponent<BasicEnemy>();
            StartCoroutine(be.sweepDetect());
        }
    }
}
