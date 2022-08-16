using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CPScript : MonoBehaviourPunCallbacks
{

    //GameObject[] t = new GameObject[4];
    public GameObject Placements;
    public GameObject Bench;
    public GameObject RaceChoice;
    public GameObject[] PricePlacemnets;
    public InfoPanels InfoPanel;
    public GameObject[] PowerUps = new GameObject[23];
    public GameObject powerupParent;
    public Transform[] PowerUpsPosition;
    public GameObject[] PowerInfos;
    public GM gm;
    public Transform PortalPos2;
    public GameObject SecondPlaceToMove;
    public GameObject ThirdPlaceToMove;
    public GameObject Deck;
    public GameObject SecondDeckMove;
    public Transform CanvasTimerPosition;

    public Text GoldText;
    public Text PopulationText;
    public Image HealthImage;
    public Image LevelImage;
    public Text LevelText;

    public GameObject DeckOriginalPosRot;
    public GameObject FireDragonEffect;
    public string PlayerLayer;
    public Transform automaticPlace;

    public GameObject CommonCardsParent;






    private void Awake()
    {
        Placements.SetActive(false);
        Bench.SetActive(false);
        RaceChoice.SetActive(true);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!photonView.IsMine) return;
        if (other.tag == "Player")
        {
            print("Player Touchj");
            // GameObject t = other.transform.GetChild(2).gameObject;
            GameObject t = other.transform.gameObject;
            int a = transform.childCount - 1;
            //t = other.transform.GetChild(2).gameObject.GetComponent<manager>().Table;
            for (int i = 0; i <= a; i++)
            {
                t.GetComponent<manager>().Table[i] = transform.GetChild(i).gameObject;
                //t.GetComponent<manager>().CostTexts[i] = transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>();
                t.GetComponent<manager>().CostTexts[i] = PricePlacemnets[i].GetComponent<Text>();
            }
            //t.GetComponent<manager>().MakeTable();
            t.GetComponent<manager>().Placements = Placements;
            t.GetComponent<manager>().Bench = Bench;
            t.GetComponent<manager>().RaceChoice = RaceChoice;
            t.GetComponent<manager>().PortalPos2 = PortalPos2;
            InfoPanel.m = t.GetComponent<manager>();
            t.GetComponent<manager>().InfoPanel = InfoPanel;



            int a1 = powerupParent.transform.childCount - 1;
            for (int i = 0; i <= a1; i++)
            {
                PowerUps[i] = powerupParent.transform.GetChild(i).gameObject;
                PowerUps[i].SetActive(false);
                //t.GetComponent<manager>().PowerUps[i] = powerupParent.transform.GetChild(i).gameObject;
            }
            t.GetComponent<manager>().PowerUps = PowerUps;
            t.GetComponent<manager>().powerInfo = PowerInfos;
            t.GetComponent<manager>().powerUpsPosition = PowerUpsPosition;
            
            gm.managerscript = t.GetComponent<manager>();
            t.GetComponent<manager>().gm = gm;
            t.GetComponent<manager>().SecondPlaceToMove = SecondPlaceToMove;
            t.GetComponent<manager>().ThirdPlaceToMove = ThirdPlaceToMove;
            t.GetComponent<manager>().Deck = Deck;
            t.GetComponent<manager>().SecondDeckMove = SecondDeckMove;
            t.GetComponent<manager>().CanvasTimerPosition = CanvasTimerPosition;

            t.GetComponent<manager>().GoldText = GoldText;
            t.GetComponent<manager>().PopulationText = PopulationText;
            t.GetComponent<manager>().HealthImage = HealthImage;
            t.GetComponent<manager>().LevelImage = LevelImage;
            t.GetComponent<manager>().LevelText = LevelText;
            t.GetComponent<manager>().DeckOriginalPosRot = DeckOriginalPosRot;
            t.GetComponent<manager>().FireDragonEffect = FireDragonEffect;
            t.GetComponent<manager>().PlayerLayer = PlayerLayer;
            t.GetComponent<manager>().automaticPlace = automaticPlace;
            t.GetComponent<manager>().CommonCardsParent = CommonCardsParent;
            //t.GetComponent<manager>().canv.position = CanvasTimerPosition;


            this.enabled = false;

        }
    }
}
