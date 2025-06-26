using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTestingScript : MonoBehaviour
{
    public Transform plyr;

    void Start()
    {
        EventManager.saveCheckPoint += showChekP;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(plyr.position, Vector3.forward, - 20* Time.deltaTime);
    }
    void showChekP(Vector3 pos)
    {
        //Debug.Log("pos" + pos);
    }
}
