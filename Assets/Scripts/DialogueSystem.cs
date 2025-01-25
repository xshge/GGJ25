using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    // trigger the checkpoint event 
    public GameObject dialogueCanvas;
    Rigidbody2D _daisy;
    bool released = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bubble") && !released)
        {
            Debug.Log("talking");
            //turn rigidbody kinematic;
            _daisy = collision.gameObject.GetComponentInParent<Rigidbody2D>();
            if( _daisy != null ) _daisy.bodyType = RigidbodyType2D.Kinematic;
            //start Coroutine:
            //mkae the texbox appear;
            //trigger aniamtion;
            //trigger checkpoint;
            StartCoroutine(TextBoxDialogue());
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
