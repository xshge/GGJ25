using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    // trigger the checkpoint event 
    public GameObject dialogueCanvas;
    public OilBubble obbl;
    public int sizeCount = 1;
    Rigidbody2D _daisy;
    bool released = false;
    Animator _animator;

    void Start()
    {
        
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bubble") && !released)
        {
            
            //checking the sizeCount;
            if(obbl.HP <= 0)
            {   

                //turn rigidbody kinematic;
                GameObject parent = collision.gameObject.transform.parent.gameObject;
                _daisy = parent.GetComponent<Rigidbody2D>();
                _animator = parent.GetComponent<Animator>();
                //Debug.Log(collision.gameObject.transform.parent);
                if( _daisy != null ) _daisy.bodyType = RigidbodyType2D.Static;
                //start Coroutine:
                //mkae the texbox appear;
                //trigger aniamtion;
                //trigger checkpoint;
                StartCoroutine(TextBoxDialogue());

            }
          
        }
    }
    IEnumerator TextBoxDialogue()
    {
        yield return new WaitForSeconds(1f);

        dialogueCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (_daisy != null) _daisy.bodyType = RigidbodyType2D.Dynamic;
        dialogueCanvas.SetActive(false);
        //start leaving animation on the prefacb;
        Debug.Log(_animator.GetParameter(0));
        released = true;
        //trigger Chekpoint Event;
        EventManager._saving(transform.position);
        yield break;

    }
}
