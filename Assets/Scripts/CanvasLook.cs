using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLook : MonoBehaviour
{

    public Transform cam;
    public Vector3 pos;
    public Vector3 pos2;


    void Update()
    {
        //var lookPos = cam.position -(- transform.position + pos);// + pos;
        //lookPos.y = 0;
        //var rotation = Quaternion.LookRotation(lookPos);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        Vector3 targetPostition = new Vector3(cam.position.x * pos.x,
                                       this.transform.position.y * pos.y,
                                       cam.position.z * pos.z);
        transform.LookAt(targetPostition, cam.rotation * pos2);
        //transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.down);
    }
}
