using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Oculus.Interaction;


public class GM : MonoBehaviour
{

    public int raceNum;
    public Color c;
    public Color c2;
    public TMP_Text lastt;
    public manager managerscript;

    public static GameObject[] fight1 = new GameObject[2];
    public GameObject[] fight11 = new GameObject[2];
    public static GameObject[] fight2 = new GameObject[2];
    public GameObject[] fight22 = new GameObject[2];
    public static GameObject[] fight3 = new GameObject[2];
    public GameObject[] fight33 = new GameObject[2];

    private void Start()
    {
        raceNum = 0;
        lastt = null;
    }



    public void RaceChosen(int Race)
    {
        raceNum = Race;
        print("Race Choosen: " + Race);
    }

    public void RaceChosen2(TMP_Text t)
    {
        if (lastt != null)
        {
            lastt.color = c2;
        }
        lastt = t;
        t.color = c;
        print("Color changed for: " + t);
    }

    public void RefreshSHop()
    {
        managerscript.MakeTable2();
    }

    public void XPShop()
    {
        managerscript.LevelUpButton();
    }

}
