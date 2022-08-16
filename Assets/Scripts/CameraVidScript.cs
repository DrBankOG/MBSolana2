using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVidScript : MonoBehaviour
{
    public Transform move1;
    public Transform move2;
    public Transform move3;

    public Transform look1;
    public Transform look2;
    public Transform look3;

    public float speed;
    float d1 = 10;
    float d2 = 10;
    float d3 = 10;
    float d4 = 10;

    void Update()
    {
        if (d1 > 0.5f)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, move1.position, step);
            transform.LookAt(look1);

            d1 = Vector3.Distance(transform.position, move1.transform.position);
        }
        else if (d2 > 0.5f)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, move2.position, step);
            transform.LookAt(look2);

            d2 = Vector3.Distance(transform.position, move2.transform.position);
        }
        else if (d3 > 0.5f)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, move3.position, step);
            transform.LookAt(look2);

            d3 = Vector3.Distance(transform.position, move3.transform.position);
        }



    }
}
