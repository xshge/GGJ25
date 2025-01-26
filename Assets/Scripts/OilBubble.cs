using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBubble : MonoBehaviour
{
    public int HP;
    public Transform animalTransform;
    public void ShrinkOil()
    {
        HP--;
        Vector3 newScale = transform.localScale * .5f;
        if (newScale.x > animalTransform.localScale.x)
        {
            transform.localScale = newScale;
        }
        

        if (HP == 0) { StartCoroutine(PopOil()); }
    }

    public IEnumerator PopOil()
    {
        gameObject.GetComponent<Animator>().Play("OilBubble");
        gameObject.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }
}
