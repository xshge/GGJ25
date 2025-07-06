using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
