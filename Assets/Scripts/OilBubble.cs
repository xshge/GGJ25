using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBubble : MonoBehaviour
{
    public int HP;
    public Transform animalTransform;
    public Sprite cleanAnim;
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

        yield return new WaitForSeconds(.15f);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Transform p = transform.parent;
        SpriteRenderer ren= p.GetComponent<SpriteRenderer>();
        ren.sprite = cleanAnim;

        yield return new WaitForSeconds(.4f);

        Destroy(gameObject);
    }
}
