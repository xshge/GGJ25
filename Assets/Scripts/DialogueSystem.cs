using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class DialogueSystem : MonoBehaviour
{
    // trigger the checkpoint event 
    public GameObject dialogueCanvas;
    public OilBubble obbl;
    public AudioClip[] talkingSounds;
    [SerializeField] private TextAsset _sceneScript;
    [SerializeField] int numberOfSpeaker;
    [SerializeField] DaisyStates _dStateMachine;
    [SerializeField] CharacterController _characterController;
    TMP_Text characterName;
    AudioSource _audioSource;
    bool released = false;
    DialogueRunner dialogueRunner;
    Animator _animator;

    private void Awake()
    {
         dialogueRunner = dialogueCanvas.GetComponent<DialogueRunner>();
        _audioSource = GetComponent<AudioSource>();
        characterName = dialogueRunner.transform.Find("Canvas/Line View/Character Name").GetComponent<TMP_Text>();
    }
    void Start()
    {

        EventManager.StartDialogue += startScene;
        dialogueRunner.AddCommandHandler(
            "releaseDaisy",
            endingScene
            );
        dialogueRunner.AddCommandHandler(
           "flipDaisy",
           facing
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
        
        yield break;

    }

    void facing()
    {
        if(_characterController.daisy.flipX == false)
        {
            _characterController.daisy.flipX = true;
        }
    }
    void endingScene()
    {
        _dStateMachine.ChangeDaisyState(BubbleGirlState.Idle);
        _characterController.enabled = true;
        Debug.Log("reset Daosy");
    }

    public void animalTalk()
    {
        switch (characterName.text)
        {
            case "Dolphin":
                _audioSource.clip = talkingSounds[0];
                break;
            case "Daisy":
                _audioSource.clip = talkingSounds[1];
                break;
            case "Robot":
                _audioSource.clip = talkingSounds[2];
                break;
        }
        _audioSource.Play();
    }

   
}
