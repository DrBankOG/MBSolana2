using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus.Interaction;


public class NextLevel : MonoBehaviour
{
    public void LoadLevel1(ToggleDeselect butt)
    {
        if (!butt.isOn) return;
        SceneManager.LoadScene("TS4", LoadSceneMode.Single);
    }
}
