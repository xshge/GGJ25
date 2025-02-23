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
    }

    public IEnumerator ChangeBubbleState(ShieldStates state)
    {
        shieldState = state;

        if(state == ShieldStates.Popped)
        {
            shieldState = ShieldStates.Regenerating;

            yield return new WaitForSeconds(3);

            shieldState = ShieldStates.Active;
        }

        yield return null;
    }
}
