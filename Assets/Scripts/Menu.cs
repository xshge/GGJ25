using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Starting()
    {
        StartCoroutine(StartUp());
    }

    public IEnumerator StartUp()
    {
        yield return new WaitForSeconds(11f);

        LoadScene(1);
    }
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
