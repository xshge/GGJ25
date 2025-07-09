using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image transitionScreen;
    float finAlpha = 1;
    void Start()
    {
        StartCoroutine(fadin());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator fadin()
    {   
        yield return new WaitForSeconds(1.5f);
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("EndScreen");
        sceneLoad.allowSceneActivation = false;
        float currAlpha = transitionScreen.color.a;

        while(currAlpha < finAlpha)
        {
            currAlpha = Mathf.MoveTowards(currAlpha, finAlpha, 1.0f * Time.deltaTime);
            Color img = new Color(transitionScreen.color.r, transitionScreen.color.g, transitionScreen.color.b, currAlpha);
            transitionScreen.color = img;
            yield return null;
        }

        sceneLoad.allowSceneActivation = true;
        yield break;
    }
}
