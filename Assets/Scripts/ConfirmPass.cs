using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmPass : MonoBehaviour
{

    public GameObject ValidPass;
    public GameObject NotValidPass;
    //public GameObject NotMatchText;

    public TMP_InputField text1;
    public TMP_InputField text2;

    public TMP_Text InvalidText;

    void Awake()
    {
        ValidPass.SetActive(false);
        NotValidPass.SetActive(true);
    }

    public void CheckIfMatch()
    {
        if (text1.text != "")
        {
            if (text1.text == text2.text)
            {
                ValidPass.SetActive(true);
                NotValidPass.SetActive(false);
            }
            else
            {
                ValidPass.SetActive(false);
                NotValidPass.SetActive(true);
            }
        }
        else
        {
            ValidPass.SetActive(false);
            NotValidPass.SetActive(true);
        }
    }


    public void NotValidPress()
    {
        InvalidText.gameObject.SetActive(true);
        if (text1.text == "" && text2.text == "")
        {
            InvalidText.text = "Enter Password";
        }
        else if (text2.text == "")
        {
            InvalidText.text = "Confrim Password!";
        }
        else if (text1.text != text2.text)
        {
            InvalidText.text = "Password doesn't match!";
        }
        else if (text1.text == text2.text)
        {
            ValidPass.SetActive(true);
            NotValidPass.SetActive(false);
        }
    }


}
