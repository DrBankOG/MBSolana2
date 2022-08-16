using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoweInfoScripts : MonoBehaviour
{

    public GM gm;
    public Text txt;
    public string firstText;
    public string firstText2;
    public bool isPop;
    public bool a;
    // Start is called before the first frame update
    void Start()
    {
        if (isPop)
        {
            txt.text = firstText;
            return;
        }

        if (gm.raceNum == 0)
        {
            txt.text = firstText + "\n" + firstText2 + " Villains";
        }
        else if (gm.raceNum == 1)
        {
            txt.text = firstText + "\n" + firstText2 + " Demons";
        }
        else if (gm.raceNum == 2)
        {
            txt.text = firstText + "\n" + firstText2 + " Wild";
        }
        else if (gm.raceNum == 3)
        {
            txt.text = firstText + "\n" + firstText2 + " Lizards";
        }
        else if (gm.raceNum == 4)
        {
            txt.text = firstText + "\n" + firstText2 + " Undead";
        }
        else if (gm.raceNum == 5)
        {
            txt.text = firstText + "\n" + firstText2 + " Mythic";
        }
    }

    private void Update()
    {

        if (a)
        {
            a = false;
            gameObject.SetActive(false);
        }
    }
}
