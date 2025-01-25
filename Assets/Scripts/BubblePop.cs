using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("bubble collided");

        if (other.gameObject.CompareTag("OilBubble"))
        {
            other.gameObject.GetComponent<OilBubble>().ShrinkOil();
        }

        StartCoroutine(PopBubble());
    }

    public IEnumerator PopBubble()
    {
        // some animation where the bubble pops

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
