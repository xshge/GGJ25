using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("bubble collided");

        if (findComponent(other.gameObject, out OilBubble ob))
        {
            ob.ShrinkOil();
        }

        StartCoroutine(PopBubble());
    }
    //edited by Xinyi, for finding child components;
    bool findComponent(GameObject parent, out OilBubble _oilB)
    {
        bool foundChild = false;
        OilBubble bb = null;
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("OilBubble"))
            {
                bb = child.GetComponent<OilBubble>();
                foundChild = true;
            }
        }
        _oilB = bb;
        return foundChild;
    }
    public IEnumerator PopBubble()
    {
        // some animation where the bubble pops
        gameObject.GetComponent<Animator>().Play("bubble_pops");
        gameObject.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }
}
