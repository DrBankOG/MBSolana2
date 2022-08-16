using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
//using Chiligames.MetaAvatarsPun;

public class manager : MonoBehaviourPunCallbacks, IOnEventCallback, IPunObservable
{
    public static int matchLangth;
    public Text timerText;
    public Text timerText2;
    public int currentMatchTime;
    private Coroutine timerCoroutine;
    public Transform canv;
    public Transform pos1;
    public Transform pos2;

    public int PlayerNum;
    public GameObject[] Table = new GameObject[5];
    public Text[] CostTexts = new Text[5];
    //PhotonView pv;
    public GameObject CommonCardsParent;
    public GameObject[] CommonCardsSubParent;
    PhotonView pv;
    public GM gm;
    private int chosenRace;
    public GameObject Placements;
    public GameObject Bench;
    public GameObject RaceChoice;

    public static int phase;

    public int level;
    public int gold;

    public int phase2;
    public int phase3;

    public int MaxPopulation;
    public int populationOut;
    public InfoPanels InfoPanel;

    public GameObject[] PowerUps = new GameObject[23];
    public int powerUpIncrement;
    public Transform[] powerUpsPosition = new Transform[6];

    public float ADC;
    public float HPC;
    public bool gotcard;
    public bool gotcard2;


    public List<GameObject> CardsInshop = new List<GameObject>();
    public GameObject[] CardsInshop2 = new GameObject[6];
    public List<GameObject> MonstersChosen = new List<GameObject>();
    public GameObject[] powerInfo = new GameObject[3];

    public GameObject WorldSpawnPointRef;
    public static bool started;
    public bool started2;
    public bool started3;
    public GameObject StartButton;

    public bool TimerFinished;
    public GameObject OVR;
    public GameObject BlackHole;
    public Vector3 bhV;
    public GameObject BlackImage;
    public Transform PortalPos;
    public Transform PortalPos2;

    public static List<GameObject> PlayersInRoom = new List<GameObject>();
    public List<GameObject> PlayersInRoom2 = new List<GameObject>();

    public bool fightTime;
    public bool willMove;
    public GameObject[] PlayersInRoomWithoutNull;
    public GameObject SecondPlaceToMove;
    public GameObject PlaceToMove;
    public GameObject ThirdPlaceToMove;
    public GameObject PlaceToWatch;
    public GameObject Deck;
    public GameObject DeckPlaceToMove; // Changes where deck needs to Move
    public GameObject SecondDeckMove; // Reference to a position in the player world
    public GameObject CanvasTimer;
    public Transform CanvasTimerPosition;

    public List<GameObject> PlayersInRoomWithoutNull2 = new List<GameObject>();
    public static List<GameObject> PlayersInRoom3 = new List<GameObject>();

    public float health; 
    public Text GoldText;
    public Text PopulationText;
    public Image HealthImage;
    public Image LevelImage;
    public Text LevelText;
    public float LevelExp;
    public float LevelExpprogress;
    //public GameObject RefreshButton;
    public float prog2;

    public bool finished;

    public float postomovex;
    public float postomovey;
    public float postomovez;

    public float decktomovex;
    public float decktomovey;
    public float decktomovez;

    public GameObject DeckOriginalPosRot;
    public GameObject FireDragonEffect;

    public string PlayerLayer;
    public Transform automaticPlace;




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync health
        if (stream.IsWriting)
        {
            try
            {
                stream.SendNext(health);
                stream.SendNext(willMove);

                stream.SendNext(postomovex);
                stream.SendNext(postomovey);
                stream.SendNext(postomovez);

                stream.SendNext(decktomovex);
                stream.SendNext(decktomovey);
                stream.SendNext(decktomovez);

                stream.SendNext(finished);
            }
            catch
            {
                print("Error writing");
            }
        }
        else
        {
            try
            {
                this.health = (float)stream.ReceiveNext();
                this.willMove = (bool)stream.ReceiveNext();

                this.postomovex = (float)stream.ReceiveNext();
                this.postomovey = (float)stream.ReceiveNext();
                this.postomovez = (float)stream.ReceiveNext();

                this.decktomovex = (float)stream.ReceiveNext();
                this.decktomovey = (float)stream.ReceiveNext();
                this.decktomovez = (float)stream.ReceiveNext();

                this.finished = (bool)stream.ReceiveNext();
            }
            catch
            {
                print("Error recieving");
            }
        }
    }

    public enum EventCodes : byte
    {
        RefreshTimer
    }


    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (started)
        {
            pv.RPC("IntializeMatchTimer", RpcTarget.AllBuffered, currentMatchTime);

            IntializeTimer();
        }

        print("Changed");
    }

    public void LevelUpButton()
    {
        if (gold >= 4)
        {
            gold += -4;
            LevelExpprogress += 4;

            prog2 = LevelExpprogress / LevelExp;
            if (prog2 >= 1)
            {
                level++;
                MaxPopulation++;

                if (LevelExpprogress > LevelExp)
                {
                    LevelExpprogress = LevelExpprogress - LevelExp;
                }
                else
                {
                    LevelExpprogress = 0;
                }
                LevelExp = LevelExp * 2;

                print("Print");
                print("1st: " + prog2);
                prog2 = LevelExpprogress / LevelExp;
                print("2nd: " + prog2 + ". What do: " + LevelExpprogress + " / " + LevelExp + ". Total: " + (LevelExpprogress/LevelExp));

            }
            //prog2 = LevelExpprogress / LevelExp;
            LevelImage.fillAmount = LevelExpprogress / LevelExp;
            LevelText.text = "Level " + level;
            GoldText.text = gold + " Gold";

        }
    }

    public void UpdateSideText()
    {
        if (photonView.IsMine)
        {
            GoldText.text = gold + " Gold";
            PopulationText.text = populationOut + " / " + MaxPopulation + " Population";
            HealthImage.fillAmount = health / 100;
            if (LevelExpprogress == 0)
            {
                LevelImage.fillAmount = 0.02f;
            }
            else
            {
                float prog = LevelExpprogress / LevelExp;
                if (prog >= 1)
                {
                    level++;
                    MaxPopulation++;

                    if (LevelExpprogress > LevelExp)
                    {
                        LevelExpprogress = LevelExpprogress - LevelExp;
                    }
                    else
                    {
                        LevelExpprogress = 0;
                    }
                    LevelExp = LevelExp * 2;

                    print("Print");
                    print("1st: " + prog);
                    prog = LevelExpprogress / LevelExp;
                    print("2nd: " + prog + ". What do: " + LevelExpprogress + " / " + LevelExp + ". Total: " + (LevelExpprogress / LevelExp));

                }
                LevelImage.fillAmount = prog;
            }
            LevelText.text = "Level " + level;
        }
    }

    private void Awake()
    {
        finished = false;
        gold = 10;
        LevelExp = 2;
        health = 100;
        LevelExpprogress = 0;
        PlaceToMove = null;
        phase2 = 0;
        phase3 = 0;
        TimerFinished = false;
        started = false;
        started2 = true;
        WorldSpawnPointRef = null;
        gotcard = false;
        gotcard2 = false;
        powerUpIncrement = 0;
        populationOut = 0;
        level = 1;
        MaxPopulation = 1;
        pv = GetComponent<PhotonView>();
        matchLangth = 10;
        if (photonView.IsMine)
        {
            pv.RPC("SetPlayers", RpcTarget.AllBuffered);


        }

        //InitializeUi();
        //IntializeTimer();


        //pv = GetComponent<PhotonView>();
    }

    private void OnDestroy()
    {
        try
        {
            if (photonView.IsMine)
            {
                pv.RPC("RemovePlayers", RpcTarget.AllBuffered);
            }
        }
        catch
        {
            print("RemovePlayer");
        }
        print("Destroyed");
    }

    private void Update()
    {

        if (PhotonNetwork.CountOfPlayers >= 2)
        {
            //Start game here
            started = true;
        }


        started3 = started;
        //PlayersInRoom2 = PlayersInRoom;
        //print("players: " + PhotonNetwork.CurrentRoom.Players);
        //print("players count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        //print("players at 0: " + PhotonNetwork.CurrentRoom.Players.Values);



        print("1: " + phase);
        print("2: " + phase2);

        try
        {
            if (started)
            {
                if (phase == 0)
                {
                    //Turn on BlackHole
                    if (phase2 == 0)
                    {
                        //CommonCardsParent = GameObject.Find("/Cards/Common").gameObject;


                        PlayersInRoom2 = PlayersInRoom;

                        pv.RPC("StartedI", RpcTarget.AllBuffered, true);
                        NewMatch_R2();
                        phase2 = 1;
                    }
                }
                else if (phase == 1)
                {
                    // Count Down For Choosing Race + Teleport to position
                    if (phase2 == 1)
                    {
                        //PlayersInRoom3 = PlayersInRoom;
                        BlackHole.SetActive(false);
                        BlackImage.SetActive(false);
                        IntializeTimer();
                        if (photonView.IsMine)
                        {
                            OVR.transform.position = WorldSpawnPointRef.transform.position;
                            OVR.transform.rotation = WorldSpawnPointRef.transform.rotation;
                        }
                        //else
                        //{
                        //    gameObject.transform.position = WorldSpawnPointRef.transform.position;
                        //    gameObject.transform.rotation = WorldSpawnPointRef.transform.rotation;
                        //}
                        phase2 = 0;
                    }
                }
                else if (phase == 2)
                {


                    //Reset here
                    if (phase3 == 1)
                    {
                        willMove = false;
                        Deck.transform.position = DeckOriginalPosRot.transform.position;
                        Deck.transform.rotation = DeckOriginalPosRot.transform.rotation;

                        if (photonView.IsMine)
                        {
                            OVR.transform.position = WorldSpawnPointRef.transform.position;
                            OVR.transform.rotation = WorldSpawnPointRef.transform.rotation;
                        }
                        else
                        {
                            gameObject.transform.position = WorldSpawnPointRef.transform.position;
                            gameObject.transform.rotation = WorldSpawnPointRef.transform.rotation;
                        }

                        for (int i = 0; i < MonstersChosen.Count; i++)
                        {
                            MonstersChosen[i].GetComponent<MonsterScripts>().CardReference.Relocate(true);
                            MonstersChosen[i].GetComponent<MonsterScripts>().ResetMonster();
                        }

                        BlackHole.SetActive(false);
                        BlackImage.SetActive(false);
                        phase3 = 0;

                        if (!finished)
                        {
                            health += -10f;
                        }
                        gold += 4;
                    }


                    // Count Down For Selecting the Cards / Make new Table
                    if (phase2 == 0)
                    {


                        print("entered phase");
                        //Update Gold, Population, Level, Health
                        UpdateSideText();

                        NewMatch_R();
                        phase2 = 1;
                    }
                }
                else if (phase == 3)
                {
                    // Turn On BlackHole indicate to fight
                    if (phase2 == 1)
                    {
                        print("entered phase2");

                        NewMatch_R3();
                        phase2 = 0;

                        if (populationOut == 0)
                        {
                            CardsInshop[0].GetComponent<PlacementsScript>().Placeautomatic(automaticPlace);
                        }
                    }
                }
                else if (phase == 4)
                {
                    // Teleport or wait for players
                    if (phase2 == 0)
                    {
                        print("entered phase3");

                        if (photonView.IsMine)
                        {
                            FireDragonEffect.SetActive(false);
                            FireDragonEffect.SetActive(true);
                        }
                        NewMatch_R4();
                        phase2 = 1;
                    }
                }
                else if (phase == 5)
                {
                    //Change Layer to fight
                    if (phase2 == 1)
                    {
                        print("entered phase4");

                        print("Will Move Of: " + gameObject.name + " is : " + willMove);
                        if (PlaceToMove != null && photonView.IsMine)
                        {
                            for (int i = 0; i < MonstersChosen.Count; i++)
                            {
                                pv.RPC("ChangeLayer", RpcTarget.AllBuffered, i, 9);
                            }
                        }
                        IntializeTimer();
                        phase2 = 0;
                    }
                }
                else if (phase == 6)
                {
                    if (phase3 == 0)
                    {
                        print("Entered 6 second");
                        NewMatch_R3();
                        phase3 = 1;
                        //IntializeTimer();
                        phase2 = 0;
                    }
                }

            }
        }
        catch
        {
            print("Err on Update");
        }
    }

    private void InitializeUi()
    {
        //if (!photonView.IsMine) return;
        //{
        //gm = GameObject.Find("/Networking/NetworkManager").GetComponent<GM>();
        //gm.managerscript = this;
        //CommonCardsParent = GameObject.Find("/Cards/Common").gameObject;
        //int a = CommonCardsParent.transform.childCount - 1;
        //CommonCardsSubParent = new GameObject[a + 1];
        //for (int i = 0; i <= a; i++)
        //{
        //    CommonCardsSubParent[i] = CommonCardsParent.transform.GetChild(i).gameObject;
        //}
        timerText = GameObject.Find("/Canvas/Timer").GetComponent<Text>();
        timerText2 = GameObject.Find("/Canvas/PrimaryText").GetComponent<Text>();
        //CanvasTimer = GameObject.Find("/Canvas/PrimaryText").gameObject;
        canv = GameObject.Find("/Canvas").transform;
        pos1 = GameObject.Find("/Positions/pos1").transform;
        pos2 = GameObject.Find("/Positions/pos2").transform;
        //BlackHole = GameObject.Find("Effect5").gameObject;
        //BlackHole.SetActive(false);

        timerText2.text = "Choose & Wait";

        GameObject t1 = GameObject.Find("InputOVR").gameObject;
        t1.SetActive(false);
        t1.SetActive(true);

        //if (photonView.IsMine)
        //{
        //    canv.position = CanvasTimerPosition.position;
        //    canv.rotation = CanvasTimerPosition.rotation;
        //}
        //}
    }

    [PunRPC]
    public void RPC_MakeTable6(int i)
    {
        try
        {
            CommonCardsSubParent[i] = CommonCardsParent.transform.GetChild(i).gameObject;
        }
        catch
        {
            print("Err MT6");
        }
    }

    public void MakeTable()
    {
        if (CommonCardsSubParent.Length <= 0)
        {
            int a = CommonCardsParent.transform.childCount - 1;
            CommonCardsSubParent = new GameObject[a + 1];
            for (int i = 0; i <= a; i++)
            {
                pv.RPC("RPC_MakeTable6", RpcTarget.AllBuffered, i);
                //CommonCardsSubParent[i] = CommonCardsParent.transform.GetChild(i).gameObject;
            }
        }

        if (photonView.IsMine)
        {
            pv.RPC("RPC_MakeTable5", RpcTarget.AllBuffered);

            //int i2 = 0;
            //try
            //{
            //    while (i2 < CardsInshop.Count)
            //    {
            //        if (CardsInshop.Count > 0)
            //        {
            //            Remove cards in deck
            //            pv.RPC("RPC_MakeTable3", RpcTarget.AllBuffered, false, i2);
            //            i2++;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}
            //catch
            //{
            //    print("CardsInShopErr");
            //}
            //CardsInshop.Clear();

            int i1 = 0;
            while (i1 < 5)
            {
                print("I is: " + i1);
                int r = Random.Range(0, CommonCardsParent.transform.childCount);
                if (CommonCardsSubParent[r].activeSelf)// && !CommonCardsSubParent[r].GetComponent<PlacementsScript>().beingUsed)
                {
                    continue;
                }
                //CommonCardsSubParent[r].transform.position = Table[i1].transform.position;
                if (PlayerNum == 0)
                {
                    CommonCardsSubParent[r].GetComponent<PlacementsScript>().SideUp = true;
                }
                else
                {
                    CommonCardsSubParent[r].GetComponent<PlacementsScript>().SideUp = false;
                }

                // Reference money
                //CommonCardsSubParent[r].GetComponent<PlacementsScript>().managerScript = this;

                //pv.RPC("RPC_MakeTable42", RpcTarget.AllBuffered);
                pv.RPC("RPC_MakeTable4", RpcTarget.AllBuffered, r);

                //CardsInshop.Add(CommonCardsSubParent[r]);
                CostTexts[i1].text = CommonCardsSubParent[r].GetComponent<PlacementsScript>().price.ToString() + " GOLD";
                pv.RPC("RPC_MakeTable2", RpcTarget.AllBuffered, Table[i1].transform.position.x, Table[i1].transform.position.y, Table[i1].transform.position.z, r);
                if (PlayerNum == 0) pv.RPC("RPC_MakeTable", RpcTarget.AllBuffered, true, r, 8);
                else pv.RPC("RPC_MakeTable", RpcTarget.AllBuffered, true, r, 9);
                i1++;

            }
        }
    }

    public void MakeTable2()
    {
        if (photonView.IsMine)
        {
            if (gold < 2) return;
            gold += -2;
            pv.RPC("RPC_MakeTable5", RpcTarget.AllBuffered);

            //int i2 = 0;
            //try
            //{
            //    while (i2 < CardsInshop.Count)
            //    {
            //        if (CardsInshop.Count > 0)
            //        {
            //            // Remove cards in deck
            //            pv.RPC("RPC_MakeTable3", RpcTarget.AllBuffered, false, i2);
            //            i2++;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}
            //catch
            //{
            //    print("CardsInShopErr");
            //}
            //CardsInshop.Clear();


            //pv.RPC("RPC_MakeTable4", RpcTarget.AllBuffered);
            //int r = Random.Range(0, CommonCardsParent.transform.childCount);

            int i1 = 0;
            while (i1 < 5)
            {
                print("I is: " + i1);
                int r = Random.Range(0, CommonCardsParent.transform.childCount);
                if (CommonCardsSubParent[r].activeSelf)// && !CommonCardsSubParent[r].GetComponent<PlacementsScript>().beingUsed)
                {
                    continue;
                }
                //CommonCardsSubParent[r].transform.position = Table[i1].transform.position;
                if (PlayerNum == 0)
                {
                    CommonCardsSubParent[r].GetComponent<PlacementsScript>().SideUp = true;
                }
                else
                {
                    CommonCardsSubParent[r].GetComponent<PlacementsScript>().SideUp = false;
                }

                // Reference money
                //CommonCardsSubParent[r].GetComponent<PlacementsScript>().managerScript = this;


                GoldText.text = gold + " Gold";
               // pv.RPC("RPC_MakeTable42", RpcTarget.AllBuffered);
                pv.RPC("RPC_MakeTable4", RpcTarget.AllBuffered, r);
                //CardsInshop.Add(CommonCardsSubParent[r]);
                CostTexts[i1].text = CommonCardsSubParent[r].GetComponent<PlacementsScript>().price.ToString() + " GOLD";
                pv.RPC("RPC_MakeTable2", RpcTarget.AllBuffered, Table[i1].transform.position.x, Table[i1].transform.position.y, Table[i1].transform.position.z, r);
                if (PlayerNum == 0) pv.RPC("RPC_MakeTable", RpcTarget.AllBuffered, true, r, 8);
                else pv.RPC("RPC_MakeTable", RpcTarget.AllBuffered, true, r, 9);
                i1++;

            }
        }
    }

    [PunRPC]
    public void RPC_MakeTable(bool b, int r, int lay)
    {
        if (CommonCardsSubParent.Length == 0) return;
        //if (CommonCardsSubParent.Length <= r) return;
        CommonCardsSubParent[r].SetActive(b);
        CommonCardsSubParent[r].GetComponent<PlacementsScript>().enabled = true;
        //CommonCardsSubParent[r].GetComponent<PlacementsScript>().ActualCard.layer = LayerMask.NameToLayer(PlayerLayer);
        CommonCardsSubParent[r].GetComponent<PlacementsScript>().managerScript = this;
        //CommonCardsSubParent[r].GetComponent<PlacementsScript>().SideUp = true;
    }

    [PunRPC]
    public void StartedI(bool i)
    {
        started = i;
    }

    [PunRPC]
    public void RPC_MakeTable2(float x, float y, float z, int r)
    {
        if (CommonCardsSubParent.Length > 0)
        {
            if (CommonCardsSubParent.Length <= r) return;
            CommonCardsSubParent[r].transform.position = new Vector3(x, y, z);
        }
    }

    //[PunRPC]
    //public void RPC_MakeTable42()
    //{
    //    //if (CardsInshop.Count > 0)
    //    //    CardsInshop.Clear();
    //}

    [PunRPC]
    public void RPC_MakeTable4(int r)
    {
        try
        {
            CardsInshop.Add(CommonCardsSubParent[r]);
        }
        catch
        {
            print("Err Manager.cs make table 4");
        }

        //foreach (GameObject a in CardsInshop2)
        //{
        //    CardsInshop.Add(CommonCardsSubParent[r]);
        //}
    }

    [PunRPC]
    public void RPC_MakeTable5()
    {
        if (photonView.IsMine)
        {
            //int i2 = 0;
            try
            {
                //while (i2 < CardsInshop.Count)
                //{
                //    if (CardsInshop.Count > 0)
                //    {
                //        // Remove cards in deck
                //        CardsInshop[i2].SetActive(false);
                //        //pv.RPC("RPC_MakeTable3", RpcTarget.AllBuffered, false, i2);
                //        i2++;
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                if (CardsInshop.Count > 0)
                {
                    foreach (GameObject a in CardsInshop)
                    {
                        a.SetActive(false);
                    }
                }
            }
            catch
            {
                print("CardsInShop Err");
            }
            CardsInshop.Clear();
        }
        
        
    }

    [PunRPC]
    public void RPC_MakeTable3(bool b)
    {
        try
        {
            //CardsInshop[r].SetActive(b);
            if (CardsInshop.Count > 0)
            {
                foreach (GameObject a in CardsInshop)
                {
                    a.SetActive(b);
                }
            }
        }
        catch
        {
            print("Err line 287 Manager.cs");
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code >= 200) return;

        EventCodes e = (EventCodes)photonEvent.Code;
        object[] o = (object[])photonEvent.CustomData;

        switch (e)
        {
            case EventCodes.RefreshTimer:
                RefreshTimer_R(o);
                break;
        }
    }


    [PunRPC]
    public void IntializeTimer2(int x, int p)
    {
        phase = p;
        //phase2 = p;
        //MonsterScripts.canStart = true;
        matchLangth = x;
        //NewMatch_R(x);
    }

    [PunRPC]
    public void PhaseI(int p)
    {
        phase = p;
    }

    [PunRPC]
    public void IntializeMatchTimer(int x)
    {
        matchLangth = x;
    }

    [PunRPC]
    public void SetPlayers()
    {
        PlayersInRoom.Add(gameObject);
    }

    [PunRPC]
    public void RemovePlayers()
    {
        PlayersInRoom.Remove(gameObject);
    }

    [PunRPC]
    public void RPC_Test(string nam)
    {
       // if (!PhotonNetwork.IsMasterClient) return;
        
            print("Testing the players: " + nam);
            //try
            //{
            //    GameObject.Find(nam).GetComponent<manager>().willMove = true;
            //}
            //catch
            //{
            //    try
            //    {
            //        GameObject.Find(nam).gameObject.transform.parent.GetComponent<manager>().willMove = true;
            //    }
            //    catch
            //    {
            //        print("Err Can't Find manager script");
            //    }
            //}
        


    }

    [PunRPC]
    public void RPC_Test2()
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        PlayersInRoomWithoutNull = new GameObject[PlayersInRoom.Count];//PlayersInRoom;

        int i2 = 0;

        for (int i = 0; i < PlayersInRoom.Count; i++)
        {
            if (PlayersInRoom[i] == null) continue;
            PlayersInRoomWithoutNull[i2] = PlayersInRoom[i];
            i2++;
        }
    }

    [PunRPC]
    public void RPC_Test3(int i, float x1, float y1, float z1, float x2, float y2, float z2)
    {
        // if (!PhotonNetwork.IsMasterClient) return;

        manager np = null;
        try
        {
            np = PlayersInRoom[i].GetComponent<manager>();
            np.willMove = true;
        }
        catch
        {
            try
            {
                np = PlayersInRoom[i].transform.parent.GetComponent<manager>();
                //np = GameObject.Find(nam).gameObject.transform.parent.GetComponent<manager>();
                np.willMove = true;
            }
            catch
            {
                print("Err Can't Find manager script");
            }
        }
        if (np == null)
        {
            print("Err null");
            return;
        }
        print("NP: " + np.gameObject.name);
        print("NP pos: " + x1 + ", " + y1 + ", " + z1 + ", ");
        print("Deck pos: " + x2 + ", " + y2 + ", " + z2 + ", ");
        np.postomovex = x1;
        np.postomovey = y1;
        np.postomovez = z1;
        np.decktomovex = x2;
        np.decktomovey = y2;
        np.decktomovez = z2;
    }

    [PunRPC]
    public void MatchMake()
    {
        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            int fightInc = 0;

            print("MatchMake");
            ////MonsterScripts.canStart = true;

            //PlayersInRoomWithoutNull = new GameObject[PlayersInRoom.Count];//PlayersInRoom;

            //int i2 = 0;

            //for (int i = 0; i < PlayersInRoom.Count; i++)
            //{
            //    if (PlayersInRoom[i] == null) continue;
            //    PlayersInRoomWithoutNull[i2] = PlayersInRoom[i];
            //    i2++;
            //}
            pv.RPC("RPC_Test2", RpcTarget.AllBuffered);

            //for (int i = 0; i < PlayersInRoomWithoutNull.Count; i++)
            //{
            //    if (PlayersInRoomWithoutNull[i] == null)
            //    {
            //        PlayersInRoomWithoutNull.RemoveAt(i);
            //    }
            //}
            //PlayersInRoomWithoutNull2 = PlayersInRoomWithoutNull;
            print("Test1: " + PlayersInRoomWithoutNull.Length);
            print("Test3: " + PlayersInRoomWithoutNull);

            //a2 = new GameObject[PlayersInRoomWithoutNull.Length];
            //int i3 = 0;
            //int i4 = 0;

            PlayersInRoom3.Clear();
            for (int i = 0; i < PlayersInRoomWithoutNull.Length; i++)
            {
                if (PlayersInRoomWithoutNull[i] != null)
                {
                    PlayersInRoom3.Add(PlayersInRoomWithoutNull[i]);
                }
            }


            while (PlayersInRoom3.Count >= 1)
            {
                //Just for solo
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    print("Only one Person!");
                    break;

                }

                //This Player Just Watch
                if (PlayersInRoom3.Count == 1)
                {
                    while (true)
                    {
                        int r2 = Random.Range(0, PlayersInRoom.Count);

                        if (PlayersInRoom == null || PlayersInRoom[r2] == PlayersInRoom3[0])
                        {
                            print("Continue");
                        }
                        else
                        {
                            PlayersInRoom3[0].GetComponent<manager>().PlaceToWatch = PlayersInRoom[r2].GetComponent<manager>().ThirdPlaceToMove;
                            break;
                        }
                    }
                    break;
                }

                //Get Two Players to fight
                int r = Random.Range(0, PlayersInRoom3.Count);

                //PlayersInRoom3[r].GetComponent<manager>().fightTime = true;
                //PlayersInRoom3[r].GetComponent<manager>().willMove = true;
                GameObject newPlay = PlayersInRoom3[r];
                pv.RPC("RPC_Test", RpcTarget.AllBuffered, newPlay.name);


                PlayersInRoom3.Remove(newPlay);
                print("Player1: " + newPlay);

                if (PlayersInRoom3.Count == 1)
                {
                    if (fightInc == 0)
                    {
                        GM.fight1[0] = newPlay;
                        GM.fight1[0] = PlayersInRoom3[0];
                    }
                    else if (fightInc == 1)
                    {
                        GM.fight2[1] = newPlay;
                        GM.fight2[1] = PlayersInRoom3[0];
                    }
                    else if (fightInc == 2)
                    {
                        GM.fight3[2] = newPlay;
                        GM.fight3[2] = PlayersInRoom3[0];
                    }
                    //PlayersInRoom3[0].GetComponent<manager>().fightTime = true;
                    //PlayersInRoom3[0].GetComponent<manager>().willMove = false;
                    Vector3 SeconddPLayerPos = PlayersInRoom3[0].GetComponent<manager>().SecondPlaceToMove.transform.position;
                    Vector3 SeconddPLayerPos2 = PlayersInRoom3[0].GetComponent<manager>().SecondDeckMove.transform.position;

                    int i = 0;
                    foreach (GameObject p in PlayersInRoom)
                    {
                        if (newPlay == p)
                        {
                            print("found this fucker: " + i);
                            break;
                        }
                        i++;
                    }
                    pv.RPC("RPC_Test3", RpcTarget.AllBuffered, i, SeconddPLayerPos.x, SeconddPLayerPos.y, SeconddPLayerPos.z, SeconddPLayerPos2.x, SeconddPLayerPos2.y, SeconddPLayerPos2.z);

                    //newPlay.GetComponent<manager>().postomovex = SeconddPLayerPos.x;
                    //newPlay.GetComponent<manager>().postomovey = SeconddPLayerPos.y;
                    //newPlay.GetComponent<manager>().postomovez = SeconddPLayerPos.z;
                    //newPlay.GetComponent<manager>().PlaceToMove = PlayersInRoom3[0].GetComponent<manager>().SecondPlaceToMove;

                    //SeconddPLayerPos = PlayersInRoom3[0].GetComponent<manager>().SecondDeckMove.transform.position;
                    //newPlay.GetComponent<manager>().decktomovex = SeconddPLayerPos.x;
                    //newPlay.GetComponent<manager>().decktomovey = SeconddPLayerPos.y;
                    //newPlay.GetComponent<manager>().decktomovez = SeconddPLayerPos.z;
                    //newPlay.GetComponent<manager>().DeckPlaceToMove = PlayersInRoom3[0].GetComponent<manager>().SecondDeckMove;
                    print("Player2: " + PlayersInRoom3[0]);

                    PlayersInRoom3.Remove(PlayersInRoom3[0]);
                }
                else
                {
                    r = Random.Range(0, PlayersInRoom3.Count);
                    if (fightInc == 0)
                    {
                        GM.fight1[0] = newPlay;
                        GM.fight1[0] = PlayersInRoom3[r];
                    }
                    else if (fightInc == 1)
                    {
                        GM.fight2[1] = newPlay;
                        GM.fight2[1] = PlayersInRoom3[r];
                    }
                    else if (fightInc == 2)
                    {
                        GM.fight3[2] = newPlay;
                        GM.fight3[2] = PlayersInRoom3[r];
                    }
                    //PlayersInRoom3[r].GetComponent<manager>().fightTime = true;
                    Vector3 SeconddPLayerPos = PlayersInRoom3[r].GetComponent<manager>().SecondPlaceToMove.transform.position;
                    Vector3 SeconddPLayerPos2 = PlayersInRoom3[r].GetComponent<manager>().SecondDeckMove.transform.position;
                    int i = 0;
                    foreach (GameObject p in PlayersInRoom)
                    {
                        if (newPlay == p)
                        {
                            print("found this fucker: " + i);
                            break;
                        }
                        i++;
                    }
                    pv.RPC("RPC_Test3", RpcTarget.AllBuffered, i, SeconddPLayerPos.x, SeconddPLayerPos.y, SeconddPLayerPos.z, SeconddPLayerPos2.x, SeconddPLayerPos2.y, SeconddPLayerPos2.z);
                    //newPlay.GetComponent<manager>().postomovex = SeconddPLayerPos.x;
                    //newPlay.GetComponent<manager>().postomovey = SeconddPLayerPos.y;
                    //newPlay.GetComponent<manager>().postomovez = SeconddPLayerPos.z;
                    //newPlay.GetComponent<manager>().PlaceToMove = PlayersInRoom3[r].GetComponent<manager>().SecondPlaceToMove;

                    //SeconddPLayerPos = PlayersInRoom3[r].GetComponent<manager>().SecondDeckMove.transform.position;
                    //newPlay.GetComponent<manager>().decktomovex = SeconddPLayerPos.x;
                    //newPlay.GetComponent<manager>().decktomovey = SeconddPLayerPos.y;
                    //newPlay.GetComponent<manager>().decktomovez = SeconddPLayerPos.z;
                    //newPlay.GetComponent<manager>().DeckPlaceToMove = PlayersInRoom3[r].GetComponent<manager>().SecondDeckMove;
                    print("Player2: " + PlayersInRoom3[r]);

                    PlayersInRoom3.Remove(PlayersInRoom3[r]);
                }
                fightInc++;
                if (PlayersInRoom3.Count == 0)
                {
                    break;
                }
            }
        }

    }

    private void IntializeTimer()
    {
        currentMatchTime = matchLangth;
        RefreshTimerUI();
        if (PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    private void RefreshTimerUI()
    {
        //if (!photonView.IsMine) return;

        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");
        timerText.text = $"{minutes}:{seconds}";

        if (photonView.IsMine && CanvasTimerPosition != null)
        {
            canv.position = CanvasTimerPosition.position;
            canv.rotation = CanvasTimerPosition.rotation;
        }
        //if (photonView.IsMine)
        //{
        //    if (transform.position.x > -10f)
        //    {
        //        canv.transform.position = pos1.position;
        //        canv.transform.rotation = pos1.rotation;
        //        PlayerNum = 0;
        //    }
        //    else
        //    {
        //        PlayerNum = 1;
        //        canv.transform.position = pos2.position;
        //        canv.transform.rotation = pos2.rotation;
        //    }
        //}
    }

    private void EndGame()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentMatchTime = 0;
        RefreshTimerUI();

        //DisableRoom
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        //Show end game UI 
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1;
        if (currentMatchTime <= 0)
        {
            //timerCoroutine = null;
            print("Choosing Finished");
            //UpdatePlayers_S((int)GameState.Ending, playerInfo);
            //if (phase == 0)
            //{
            //    phase = 1;
            //    pv.RPC("IntializeTimer3", RpcTarget.AllBuffered, 20, 1, true);
            //}
            //else

            if (photonView.IsMine)
            {
                if (phase == 0)
                {
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 10, 1);
                }
                else if (phase == 1)
                {
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 30, 2);
                }
                else if (phase == 2)
                {
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 10, 3);

                }
                else if (phase == 3)
                {
                    //Match Make Players MatchMake / Timer to open black hole /  dark screen and players go to their positions
                    pv.RPC("MatchMake", RpcTarget.AllBuffered);
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 5, 4);
                }
                else if (phase == 4)
                {


                    //They Start Fight
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 30, 5);

                }
                else if (phase == 5)
                {
                    // Timer to End Game and turn on black hole to go back
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 10, 6);
                    //pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 10, 1);

                }
                else if (phase == 6)
                {
                    pv.RPC("IntializeTimer2", RpcTarget.AllBuffered, 30, 2);

                }
            }
            //RefreshTimer_S();
            //timerCoroutine = StartCoroutine(Timer());
        }
        else
        {
            if (currentMatchTime % 2 == 0 && phase == 5)
            {
                print("Checking here");
                int qe = 0;
                print("Players In Room3: " + PlayersInRoom3.Count);
                foreach (GameObject a in PlayersInRoom3)
                {
                    manager b = a.GetComponent<manager>();
                    if (b != null)
                    {
                        if (b.finished)
                        {
                            qe++;
                        }
                    }
                }
                if (qe >= (PhotonNetwork.CurrentRoom.PlayerCount / 2))
                {
                    print("Game Finished");
                    //Switch Phase Get ready to tp back
                }
                print("qe: " + qe);
            }
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
        }
    }

    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime };

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.RefreshTimer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
            );
    }

    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0];
        RefreshTimerUI();
    }

    public void NewMatch_R()
    {
        //IntializeTimer();
        //if (!photonView.IsMine) return;
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    pv.RPC("PhaseI", RpcTarget.AllBuffered, 2);

        //}
        //gold += 10;
        timerText2.text = "Planning Phase";
        chosenRace = gm.raceNum;
        Placements.SetActive(true);
        Bench.SetActive(true);
        RaceChoice.SetActive(false);
        MakeTable();
        IntializeTimer();
        //phase2 = 3;

        GetPowerUp();
    }


    public void NewMatch_R2()
    {

        InitializeUi();

        //Turn on black hole
        BlackHole.transform.position = PortalPos.position;
        BlackHole.SetActive(false);

        BlackHole.SetActive(true);

        //Turn on black Screen => increment transparancy 
        BlackImage.SetActive(true);

        IntializeTimer();
    }

    public void NewMatch_R3()
    {

        //Turn on black hole
        if (photonView.IsMine)
        {
            timerText2.text = "Getting Ready";

            BlackHole.transform.position = PortalPos2.position;
            BlackHole.transform.rotation = PortalPos2.rotation;
            BlackHole.SetActive(false);

            BlackHole.SetActive(true);

            //Turn on black Screen => increment transparancy 
            BlackImage.SetActive(true);
        }

        IntializeTimer();
    }

    public void NewMatch_R4()
    {
        //Test
        //MonstersChosen[0].GetComponent<MonsterScripts>().CardReference.Relocate();
        //Stop from moving Cards or buy or sell
        // Move or wait
        // if (PhotonNetwork.IsMasterClient)
        // {
        timerText2.text = "Fighting Phase";

       // if (photonView.IsMine)
      //  {
            if (willMove)
            {
                for (int i = 0; i < MonstersChosen.Count; i++)
                {
                    pv.RPC("RPC_DeActivate", RpcTarget.AllBuffered, i, false);

                }

                //Move whole ass deck & Rotate
                //Deck.transform.position = DeckPlaceToMove.transform.position;
                //Deck.transform.rotation = DeckPlaceToMove.transform.rotation;
                Deck.transform.position = new Vector3(decktomovex, decktomovey, decktomovez);
                Deck.transform.rotation = SecondDeckMove.transform.rotation;

                //Move Monsters and cards => make relocate function on placementsScripts
                for (int i = 0; i < MonstersChosen.Count; i++)
                {
                    MonstersChosen[i].GetComponent<MonsterScripts>().CardReference.Relocate(false);
                }

                //Move player
                if (photonView.IsMine)
                {
                    //OVR.transform.position = PlaceToMove.transform.position;
                    //OVR.transform.rotation = PlaceToMove.transform.rotation;
                    OVR.transform.position = new Vector3(postomovex, postomovey, postomovez);
                    OVR.transform.rotation = SecondDeckMove.transform.rotation;
                }
                //else
                //{
                //    //gameObject.transform.position = PlaceToMove.transform.position;
                //    //gameObject.transform.rotation = PlaceToMove.transform.rotation;
                //    gameObject.transform.position = new Vector3(postomovex, postomovey, postomovez);
                //    gameObject.transform.rotation = SecondDeckMove.transform.rotation;
                //}


            }
            else
            {
                //for (int i = 0; i < MonstersChosen.Count; i++)
                //{
                //    pv.RPC("RPC_DeActivate", RpcTarget.AllBuffered, i, false);
                print("I'll stay here: " + gameObject);
            }

        // }
        //}
        pv.RPC("RPC_MakeTable3", RpcTarget.AllBuffered, false);
        //foreach (GameObject a in CardsInshop)
        //{
        //    a.SetActive(false);
        //}
        pv.RPC("RPC_StopMovementofCards", RpcTarget.AllBuffered, false);

        //Turn off black hole
        BlackHole.SetActive(false);


        //Turn off black Screen
        BlackImage.SetActive(false);

        //TurnOff UI
        //Placements.SetActive(false);
        Bench.SetActive(false);

        IntializeTimer();
    }


    public void GetPowerUp()
    {
        if (photonView.IsMine)
        {
            if (powerUpIncrement >= 5)
                return;

            while (true)
            {
                int ii3 = Random.Range(0, 23);
                if (PowerUps[ii3].activeSelf)
                    continue;

                int typeofcard = PowerUps[ii3].GetComponent<PowerUpScript>().CardType;
                if (typeofcard == 0)
                {
                    //Add Attack
                    gotcard = true;
                    ADC += 10;
                    powerInfo[0].SetActive(true);

                }
                else if (typeofcard == 1)
                {
                    //Add Health
                    gotcard2 = true;
                    HPC += 100;
                    powerInfo[1].SetActive(true);

                }
                else if (typeofcard == 2)
                {
                    //Add MaxPopulation
                    MaxPopulation += 1;
                    PopulationText.text = populationOut + " / " + MaxPopulation + " Population";
                    powerInfo[2].SetActive(true);
                }

                int i2 = 0;
                while (i2 < MonstersChosen.Count)
                {
                    if (MonstersChosen.Count > 0)
                    {
                        // add stats to existing monsters
                        pv.RPC("RPC_AddStats", RpcTarget.AllBuffered, i2, ADC, HPC);
                        i2++;
                    }
                    else
                    {
                        break;
                    }
                }
                pv.RPC("ActivatePowerUps", RpcTarget.AllBuffered, ii3, true, powerUpIncrement);
                powerUpIncrement++;
                break;
            }
        }

    }

    [PunRPC]
    public void RPC_StopMovementofCards(bool b)
    {
        PlacementsScript.CanMove = b;
        MonsterScripts.canStart = true;

    }

    [PunRPC]
    public void RPC_DeActivate(int r, bool b)
    {
        try
        {
            if (MonstersChosen.Count == 0) return;
            if (MonstersChosen[r] == null) return;
            MonstersChosen[r].SetActive(b);
            PlacementsScript.CanMove = b;
            MonsterScripts.canStart = true;
        }
        catch
        {
            print("Error line 854. Manger.cs");
        }
    }

    [PunRPC]
    public void RPC_AddStats(int r, float a, float h)
    {
        try
        {
            MonstersChosen[r].GetComponent<MonsterScripts>().attackDamage += a;
            MonstersChosen[r].GetComponent<MonsterScripts>().health += h;
        }
        catch
        {
            print("Err Add Stats");
        }
    }

    [PunRPC]
    public void ActivatePowerUps(int i3, bool active, int i2)
    {
        PowerUps[i3].transform.position = powerUpsPosition[i2].position;
        PowerUps[i3].SetActive(active);

    }

    [PunRPC]
    public void ChangeLayer(int i3, int i2)
    {
        try
        {
            if (MonstersChosen[i3] != null)
            {

            }
               // MonstersChosen[i3].layer = LayerMask.NameToLayer("Monster2");
            //MonstersChosen[i3].layer = i2;
        }
        catch
        {
            print("Error at change layer");
        }

    }

}
