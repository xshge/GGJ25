using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    GameObject[] musicManagers;

    void Awake()
    {
        musicManagers = GameObject.FindGameObjectsWithTag("MusicManager"); // checking if there are duplicate music managers

        if (musicManagers.Length > 1) //if there are multiple music managers, this destroys it
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); //continuous music/ambient ocean noise
        }

        SceneManager.sceneLoaded += onSceneLoad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onSceneLoad(Scene scene, LoadSceneMode mode)
    {   
        AudioSource bgm = transform.GetChild(0).GetComponent<AudioSource>();
        if(scene.buildIndex == 0)
        {
            bgm.Stop();
            
        }else if(scene.buildIndex == 1)
        {
            if(bgm.isPlaying == false)
            {
                bgm.Play();
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= onSceneLoad;    
    }
}
