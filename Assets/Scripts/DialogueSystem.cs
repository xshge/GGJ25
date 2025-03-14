using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    // trigger the checkpoint event 
    public GameObject dialogueCanvas;
    public OilBubble obbl;

    [SerializeField] private TextAsset _sceneScript;
    [SerializeField] int numberOfSpeaker;
    
    public int sizeCount = 1;
    bool released = false;
    Animator _animator;
    List<Array> _lines = new List<Array>();

    void Start()
    {
        //TODO:parse the txt into individual lines;
        string[] data = _sceneScript.text.Split(new string[] { "\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);

        //TODO: parse each line into a dict, key: Speaker, Value:Line;
        //so it can be read through once by running through the dictionary entirely.

        data.ToList().ForEach(x =>
        {
            string[] _line = x.Split(":", StringSplitOptions.RemoveEmptyEntries);
            _lines.Add(_line);

        });

        //Debug.Log("parsed break");
    }

    void startScene()
    {
        StartCoroutine(TextBoxDialogue());
    }
    IEnumerator TextBoxDialogue()
    {
        yield return new WaitForEndOfFrame();

        dialogueCanvas.SetActive(true);
        yield return new WaitForSeconds(2f);
        dialogueCanvas.SetActive(false);
        
      
        released = true;
        //trigger Chekpoint Event;
        EventManager._saving(transform.position);
        yield break;

    }
}
