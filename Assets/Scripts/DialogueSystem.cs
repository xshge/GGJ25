using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    // trigger the checkpoint event 
    public GameObject dialogueCanvas;
    public int sizeCount = 1;
    Rigidbody2D _daisy;
    bool released = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bubble") && !released)
        {
            sizeCount--;
            //checking the sizeCount;
            if(sizeCount <= 0)
            {   
                //oil bubble renderer get turn off;
                SpriteRenderer spR = GetComponent<SpriteRenderer>();
                spR.enabled = false;

                //turn rigidbody kinematic;
                GameObject parent = collision.gameObject.transform.parent.gameObject;
                _daisy = parent.GetComponent<Rigidbody2D>();
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
        dialogueCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (_daisy != null) _daisy.bodyType = RigidbodyType2D.Dynamic;
        dialogueCanvas.SetActive(false);
        //start leaving animation on the prefacb;
        released = true;
        //trigger Chekpoint Event;
        EventManager._saving(transform.position);
        yield break;

    }
}
