using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleScript : MonoBehaviour
{

    public Image im1;
    float i;

    private void Update()
    {
        if (i < 2)
        {


            var color = im1.color;
            color.a = i;
            im1.color = color;

            i += +Time.deltaTime;

        }
        //else
        //{
        //    gameObject.SetActive(false);
        //}
    }

    private void OnEnable()
    {
        i = -7;
    }

    //private void OnDisable()
    //{

    //    var color = im1.color;
    //    color.a = 0;
    //    im1.color = color;

    //}
}
