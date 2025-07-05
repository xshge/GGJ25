using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class DialogueSystem : MonoBehaviour
{
    // trigger the checkpoint event 
    public GameObject dialogueCanvas;
    public OilBubble obbl;

    [SerializeField] private TextAsset _sceneScript;
    [SerializeField] int numberOfSpeaker;
    [SerializeField] DaisyStates _dStateMachine;
    bool released = false;
    DialogueRunner dialogueRunner;
    Animator _animator;

    private void Awake()
    {
         dialogueRunner = dialogueCanvas.GetComponent<DialogueRunner>();
    }
    void Start()
    {

        EventManager.StartDialogue += startScene;
        dialogueRunner.AddCommandHandler(
            "releaseDaisy",
            endingScene
            );
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
        _dStateMachine.ChangeDaisyState(BubbleGirlState.Talking);
      
        released = true;
        //trigger Chekpoint Event;
        EventManager._saving(transform.position);
        yield break;

    }

    void endingScene()
    {
        _dStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
        Debug.Log("reset Daosy");
    }
}
