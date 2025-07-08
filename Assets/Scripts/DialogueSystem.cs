using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
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
    [SerializeField]Sprite[] _backings = new Sprite[2];
    TMP_Text characterName;
    Image textBackground;
    AudioSource _audioSource;
    bool released = false;
    DialogueRunner dialogueRunner;
    Animator _animator;

    private void Awake()
    {
         dialogueRunner = dialogueCanvas.GetComponent<DialogueRunner>();
        _audioSource = GetComponent<AudioSource>();
        characterName = dialogueRunner.transform.Find("Canvas/Line View/Character Name").GetComponent<TMP_Text>();
        textBackground = dialogueRunner.transform.Find("Canvas/Line View/Background").GetComponent<Image>();
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
        dialogueRunner.AddCommandHandler(
           "swapBubble",
           ToggleBackground
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
        _characterController.wasInStory = true;
        Debug.Log("reset Daosy");
    }
    void ToggleBackground()
    {
        if(characterName.text == "Robot")
        {
            textBackground.sprite = _backings[1];

        }
        else
        {
            textBackground.sprite = _backings[0];
        }
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
