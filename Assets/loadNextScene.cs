using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadNextScene : MonoBehaviour
{
    public Menu menu;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            menu.LoadScene(2);
        }
    }
}
