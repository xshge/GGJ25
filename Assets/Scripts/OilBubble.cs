using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilBubble : MonoBehaviour
{
    public int HP;

    public void ShrinkOil()
    {
        HP--;

        transform.localScale = transform.localScale * .5f;

        if (HP == 0) { Destroy(gameObject); }
    }
}
