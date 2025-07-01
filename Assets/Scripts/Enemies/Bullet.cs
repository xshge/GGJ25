using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
    private void OnEnable()
    {
        //EnemyEvents.startBullet += bulletMovement;
    }

   public void bulletMovement(float dist, Vector3 start, Vector3 Target)
    {
        StartCoroutine(_flying(dist, start, Target));
    }

    IEnumerator _flying(float dist, Vector3 start, Vector3 Target)
    {
        float remains = dist;
        float bulletSpeed = 4; 
        //bullet traveling;
        while (remains > 0f)
        {
           transform.position = Vector3.Lerp(start, Target, 1 - (remains / dist));
            remains -= bulletSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
