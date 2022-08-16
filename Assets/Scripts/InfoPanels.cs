using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanels : MonoBehaviour
{

    public manager m;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    float time;
    float i;

    private void Update()
    {
        if (i > 0)
        {

            var color = transform.GetComponent<Image>().color;
            color.a = i;
            transform.GetComponent<Image>().color = color;

            color = text1.color;
            color.a = i;
            text1.color = color;

            color = text2.color;
            color.a = i;
            text2.color = color;

            color = text3.color;
            color.a = i;
            text3.color = color;

            color = text4.color;
            color.a = i;
            text4.color = color;

            i += -Time.deltaTime;

        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        text2.text = m.populationOut.ToString() + " / " + m.MaxPopulation.ToString();
        text4.text = m.gold.ToString();
        i = 2.2f;
    }

    private void OnDisable()
    {

        i = 3f;

        var color = text1.color;
        color.a = 1;
        text1.color = color;

        color = text2.color;
        color.a = 1;
        text2.color = color;

        color = text3.color;
        color.a = 1;
        text3.color = color;

        color = text4.color;
        color.a = 1;
        text4.color = color;

        color = transform.GetComponent<Image>().color;
        color.a = 1;
        transform.GetComponent<Image>().color = color;

    }

}
