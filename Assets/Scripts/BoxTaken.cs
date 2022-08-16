using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTaken : MonoBehaviour
{
    public bool taken;
    public string lastName;
    public int MonsterID;
    public GameObject firstCard;

    private void Start()
    {
        taken = false;
        lastName = "1";
    }

    private void Update()
    {
        //if (CurrenCads[2] != null)
        //{
        //    CurrenCads[0].transform.GetChild(0).gameObject.SetActive(false);
        //    CurrenCads[0].transform.GetChild(1).gameObject.SetActive(false);
        //    CurrenCads[1].transform.GetChild(0).gameObject.SetActive(false);
        //    CurrenCads[1].transform.GetChild(1).gameObject.SetActive(false);
        //}
        //else if (CurrenCads[1] != null && CurrenCads[2] == null)
        //{
        //    CurrenCads[0].transform.GetChild(0).gameObject.SetActive(false);
        //    CurrenCads[0].transform.GetChild(1).gameObject.SetActive(false);
        //}
    }

}
