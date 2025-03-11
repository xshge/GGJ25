using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BubbleGirlState
{
    Shooting,
    Idle,
    Floating,
    Falling,
    Dead,
    Talking
}

public enum ShieldStates
{
    Active,
    Regenerating,
    Popped
}

public class DaisyStates : MonoBehaviour
{

    public BubbleGirlState daisyState;
    public ShieldStates shieldState;

    public GameObject shield2D;
    public GameObject shield3D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDaisyState(BubbleGirlState state)
    {
        daisyState = state;

        //can we call the events somewhere here?
    }


    #region ShieldStuff

    public void ShieldPopped()
    {
        shieldState = ShieldStates.Popped;
        StartCoroutine(ChangeBubbleState());
    }

    public IEnumerator ChangeBubbleState()
    {

            shieldState = ShieldStates.Regenerating;
            shield2D.SetActive(false); // will probably also have an animation for the 2D bubble. the 3D bubble is just a collider so she can bump into walls. feel free to scale both accordingly
            shield3D.SetActive(false);

            Debug.Log("popping da shield");

            yield return new WaitForSeconds(3);

            shield2D.SetActive(true);
            shield3D.SetActive(true);
        shieldState = ShieldStates.Active;


        Debug.Log("setting shields active");
    }
    #endregion
}
