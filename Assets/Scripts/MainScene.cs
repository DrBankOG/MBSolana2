using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    public GameObject WalletPage;
    public GameObject FirstPage;
    public GameObject ReceivePage;

    public void ExitMain()
    {
        WalletPage.SetActive(!WalletPage.activeSelf);
        FirstPage.SetActive(!FirstPage.activeSelf);
    }

    public void ReceiveButton()
    {
        WalletPage.SetActive(!WalletPage.activeSelf);
        ReceivePage.SetActive(!ReceivePage.activeSelf);
    }

}
