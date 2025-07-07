using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadNextScene : MonoBehaviour
{
    public Menu menu;
    private MusicManager musicManager;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
            musicManager.GetComponent<AudioSource>().Play();
            menu.LoadScene(2);
        }
    }
}
