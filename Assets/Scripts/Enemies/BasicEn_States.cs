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
    public Sprite _deadSprite;
    public SpriteRenderer _enRenderer;

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
           _enRenderer.sprite = _deadSprite;
        }else if(currentState == EnState.Sweeping)
        {
            BasicEnemy be = GetComponent<BasicEnemy>();
            StartCoroutine(be.sweepDetect());
        }
    }
}
