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

    public CharacterController characterController;

    public GameObject shield2D;
    public GameObject shield3D;
    Animator _Danimation;
    Bubble_Shooter _shooter;
    // Start is called before the first frame update
    void Start()
    {
        _Danimation = GetComponent<Animator>();
        _shooter = GetComponent<Bubble_Shooter>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDaisyState(BubbleGirlState state)
    {
        daisyState = state;

        if(daisyState == BubbleGirlState.Falling)
        {
            characterController._pRB.drag = 0;
            characterController._pRB.velocity = new Vector3(characterController._pRB.velocity.x, -8, characterController._pRB.velocity.z);
        }
        else if (daisyState == BubbleGirlState.Idle )
        {
            if (_Danimation.GetCurrentAnimatorStateInfo(0).IsName("Daisy_Idle") != true)
            {
                for (int i = 0; i < _shooter.ids.Length; i++)
                {
                    _Danimation.SetBool(_shooter.ids[i], false);
                }
                _Danimation.Play("Daisy_Idle");
            }
        }
        else
        {
            characterController._pRB.drag = .4f;
        }
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
