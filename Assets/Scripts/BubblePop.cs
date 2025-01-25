using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("bubble collided");

        StartCoroutine(PopBubble());
    }

    public IEnumerator PopBubble()
    {
        // some animation where the bubble pops

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
