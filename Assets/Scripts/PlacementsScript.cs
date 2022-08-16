using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chiligames.MetaAvatarsPun;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(PhotonView))]
public class PlacementsScript : MonoBehaviourPun, IPunObservable
{

    public Material m_Material;
    public Material m_Material2;
    // was static
    public GameObject gem = null;
    //public int touching = 0;
    //public Text t;

    public List<GameObject> gems = new List<GameObject>();
    //public HandGrabInteractable hg;
    public bool ischosen;
    public Vector3 OGPlace;

    public GameObject ActualCard;
    PhotonView pv;

    // Rare Num 1 = Common, 2 = Uncommon, 3 = Rere, 4 = Epic
    public int RareNum;

    public bool istaken;

    public Transform rotRef;
    public Transform rotRef2;

    public bool SideUp;

    public Transform rot1;
    public Transform rot2;
    public Transform rot3;
    public Transform rot4;

    public Transform placePicked;
    public Vector3 lastPos;

    public Transform lastRot;

    public int Race;
    public int price;

    public manager managerScript;
    public bool alreadyChosen;

    public int level;
    public int MonsterID;

    public TMP_Text CardLevelText;
    public TMP_Text ActualCardLevelText;
    //public GameObject[] ActualCardLevels = new GameObject[2];
    //public GameObject[] CardLevels = new GameObject[2];

    public string testlvl;
    public bool levelUpCard;
    public bool beingUsed;

    public static bool CanMove;

   // PlacementsScript ps1;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync health
        if (stream.IsWriting)
        {
            stream.SendNext(ActualCardLevelText.text);
            stream.SendNext(CardLevelText.text);
            //stream.SendNext(ps1);

        }
        else
        {
            ActualCardLevelText.text = (string)stream.ReceiveNext();
            CardLevelText.text = (string)stream.ReceiveNext();
            //ps1 = (PlacementsScript)stream.ReceiveNext();
        }
    }


    private void Awake()
    {
        CanMove = true;
        CardLevelText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        ActualCardLevelText = ActualCard.transform.GetChild(
             ActualCard.transform.childCount - 1).GetChild(0).GetComponent<TMP_Text>();
        lastRot = null;
        levelUpCard = false;
        placePicked = null;
        istaken = false;
        ischosen = false;
        OGPlace = transform.position;
        pv = GetComponent<PhotonView>();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        // hg = GetComponent<HandGrabInteractable>();
    }

    private void OnEnable()
    {
        OGPlace = transform.position;
        GetComponent<BoxCollider>().enabled = true;
        level = 1;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        levelUpCard = false;
        CardLevelText.text = "i";
        ActualCardLevelText.text = "i";
        //if (SideUp)
        //{
        transform.rotation = rotRef.rotation;
        //}
        //else
        //{
        //    transform.rotation = rotRef2.rotation;
        //}
    }

    public void Relocate(bool goingback)
    {

        //if (photonView.IsMine && placePicked != null)
        if (placePicked != null)
        {
            if (!goingback)
            {
                pv.RPC("TurnOffMonster", RpcTarget.AllBuffered, false);
                transform.rotation = rot4.rotation;
                transform.position = placePicked.position;
                OGPlace = transform.position;
                Vector3 pairpos = placePicked.GetComponent<SlotPair>().PairSlot.transform.position;
                pv.RPC("RPC_Card2", RpcTarget.AllBuffered, pairpos.x, pairpos.y + 0.1f, pairpos.z);
                pv.RPC("Rotate_Card", RpcTarget.AllBuffered, true);
            }
            else
            {
                pv.RPC("TurnOffMonster", RpcTarget.AllBuffered, false);
                transform.rotation = rot3.rotation;
                transform.position = placePicked.position;
                OGPlace = transform.position;
                Vector3 pairpos = placePicked.GetComponent<SlotPair>().PairSlot.transform.position;
                pv.RPC("RPC_Card2", RpcTarget.AllBuffered, pairpos.x, pairpos.y + 0.1f, pairpos.z);
                pv.RPC("Rotate_Card2", RpcTarget.AllBuffered, true);
            }
        }
        else
        {
            print("Not Mine: " + pv);
        }
        
        
    }

    private void Update()
    {
        if (!PunOVRGrabbable.aboll && photonView.IsMine)
        {
            
            //t.text = gems.Count.ToString();
        }
        else
        {
            if (PunOVRGrabbable.CardChosen == this.transform && photonView.IsMine)
            {
                if (ischosen && CanMove)
                {

                    //Place card
                    if (placePicked != null)
                    {
                        if (placePicked.GetComponent<BoxTaken>().firstCard == gameObject)
                        {
                            placePicked.GetComponent<BoxTaken>().taken = false;
                            placePicked.GetComponent<BoxTaken>().firstCard = null;
                            placePicked.GetComponent<BoxTaken>().lastName = "";

                        }
                    }
                    pv.RPC("RPC_PlacePicked", RpcTarget.AllBuffered);
                    //placePicked = gems[gems.Count - 1].transform;
                    Vector3 gempos = placePicked.position;
                    Vector3 pairpos = placePicked.GetComponent<SlotPair>().PairSlot.transform.position;
                    placePicked.GetComponent<Renderer>().material = m_Material2;
                    //PunOVRGrabbable.CardChosen.position = new Vector3(gempos.x, gempos.y + 0f, gempos.z);
                    if (placePicked.transform.parent.parent.name == "Bench")
                    {
                        //if (SideUp)
                        //{
                        //    transform.rotation = rotRef.rotation;
                        //    lastRot = rotRef;
                        //}
                        //else
                        //{
                        //    transform.rotation = rotRef2.rotation;
                        //    lastRot = rotRef2;
                        //}
                        //transform.position = new Vector3(gempos.x, gempos.y + 0.008f, gempos.z);

                    }
                    else
                    {
                        //if (SideUp)
                        //{
                        //    transform.rotation = rot3.rotation;
                        //    lastRot = rot3;
                        //}
                        //else
                        //{
                        //    transform.rotation = rot4.rotation;
                        //    lastRot = rot4;
                        //}
                        transform.rotation = rot3.rotation;
                        lastRot = rot3;
                        transform.position = new Vector3(gempos.x, gempos.y, gempos.z);
                    }


                    managerScript.CardsInshop.Remove(gameObject);

                    //ActualCard.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    //ActualCard.transform.position = new Vector3(pairpos.x, pairpos.y + 0.1f, pairpos.z);
                    //pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                    if (!levelUpCard)
                    {
                        placePicked.GetComponent<BoxTaken>().firstCard = gameObject;
                        pv.RPC("RPC_Card2", RpcTarget.AllBuffered, pairpos.x, pairpos.y + 0.1f, pairpos.z);
                        pv.RPC("RPC_Card", RpcTarget.AllBuffered, true, true);
                        placePicked.GetComponent<BoxTaken>().lastName = name;
                        placePicked.GetComponent<BoxTaken>().taken = true;
                        
                        placePicked.GetComponent<BoxTaken>().MonsterID = MonsterID;
                    }
                    else
                    {
                        // Add damge / health
                        //placePicked.GetComponent<BoxTaken>().firstCard.GetComponent<PlacementsScript>().ActualCard.GetComponent<MonsterScripts>().attackDamage += 10;

                        //Show Card & ActualCard Level

                        pv.RPC("RPC_Card3", RpcTarget.AllBuffered, "II", "III", false, 10f, 100f);
                        //levelUpCard = false;

                    }

                    OGPlace = transform.position;
                    
                    gems = new List<GameObject>();
                    ischosen = false;
                    //PunOVRGrabbable.aboll = false;
                    if (!alreadyChosen)
                    {
                        managerScript.gold += -price;
                        if (!levelUpCard)
                        {
                            //ActualCard.GetComponent<MonsterScripts>()
                            pv.RPC("AddMonsters", RpcTarget.AllBuffered);

                            //managerScript.MonstersChosen.Add(ActualCard);
                            managerScript.populationOut += 1;
                            //if (managerScript.gm.raceNum == MonsterID)
                            //{
                                
                            //}
                        }

                        levelUpCard = false;
                        alreadyChosen = true;
                        managerScript.InfoPanel.gameObject.SetActive(false);
                        managerScript.InfoPanel.gameObject.SetActive(true);
                        managerScript.UpdateSideText();

                    }

                    ischosen = false;
                    PunOVRGrabbable.aboll = false;
                }
                else
                {
                    if (lastRot != null)
                        transform.rotation = lastRot.rotation;
                    else
                    {
                        if (CanMove)
                        {
                            transform.rotation = rotRef.rotation;
                        }
                        else
                        {
                            transform.rotation = rotRef2.rotation;
                        }
                        //if (!CanMove)
                        //{
                        //    transform.rotation = rotRef2.rotatio
                        //}
                        //transform.rotation = rotRef.rotation;
                    }
                    transform.position = OGPlace;
                    PunOVRGrabbable.aboll = false;
                }
            }

        }


        if (photonView.IsMine)
        {
            if (gems.Count > 0)
            {
                if (gems.Count > 1)
                {
                    gems[gems.Count - 2].GetComponent<Renderer>().material = m_Material2;
                }


                if ((placePicked != gems[gems.Count - 1].transform
                    && gems[gems.Count - 1].GetComponent<BoxTaken>().taken == false
                    && (managerScript.gold >= price || alreadyChosen)
                    && (managerScript.populationOut < managerScript.MaxPopulation || alreadyChosen))
                    ||
                    ((managerScript.gold >= price || alreadyChosen)
                    && gems[gems.Count - 1].GetComponent<BoxTaken>().taken == true
                    && gems[gems.Count - 1].GetComponent<BoxTaken>().MonsterID == MonsterID
                    && gems[gems.Count - 1].GetComponent<BoxTaken>().firstCard != gameObject))
                {
                    print("Entered here");
                    if (gems[gems.Count - 1].GetComponent<BoxTaken>() != null)
                    {
                        if (gems[gems.Count - 1].GetComponent<BoxTaken>().firstCard != null)
                        {
                            PlacementsScript ps = gems[gems.Count - 1].GetComponent<BoxTaken>().firstCard.GetComponent<PlacementsScript>();
                            if (ps.level >= 3)
                            {
                                levelUpCard = false;
                                ischosen = false;
                                return;
                            }
                        }
                    }
                    gems[gems.Count - 1].GetComponent<Renderer>().material = m_Material;
                    ischosen = true;
                    levelUpCard = false;

                    if (gems[gems.Count - 1].GetComponent<BoxTaken>().taken == true
                    && gems[gems.Count - 1].GetComponent<BoxTaken>().MonsterID == MonsterID)
                    {
                        //Make a bool to know that if released to make lvl2 Card
                        levelUpCard = true;
                    }
                }
                else
                {
                    //managerScript.InfoPanel.gameObject.SetActive(true);
                    levelUpCard = false;
                    ischosen = false;
                }
            }
            else
            {
                //turn off lvl2card
                levelUpCard = false;
                ischosen = false;
            }
        }
    }


    public void Placeautomatic(Transform i)
    {
        

        if (placePicked != null)
        {
            if (placePicked.GetComponent<BoxTaken>().firstCard == gameObject)
            {
                placePicked.GetComponent<BoxTaken>().taken = false;
                placePicked.GetComponent<BoxTaken>().firstCard = null;
                placePicked.GetComponent<BoxTaken>().lastName = "";

            }
        }
        //pv.RPC("RPC_PlacePicked", RpcTarget.AllBuffered);
        placePicked = i;

        Vector3 gempos = placePicked.position;
        Vector3 pairpos = placePicked.GetComponent<SlotPair>().PairSlot.transform.position;
        placePicked.GetComponent<Renderer>().material = m_Material2;

        if (SideUp)
        {
            transform.rotation = rot3.rotation;
            lastRot = rot3;
        }
        else
        {
            transform.rotation = rot4.rotation;
            lastRot = rot4;
        }
        transform.position = new Vector3(gempos.x, gempos.y, gempos.z);
        


        managerScript.CardsInshop.Remove(gameObject);

        if (!levelUpCard)
        {
            placePicked.GetComponent<BoxTaken>().firstCard = gameObject;
            pv.RPC("RPC_Card2", RpcTarget.AllBuffered, pairpos.x, pairpos.y + 0.1f, pairpos.z);
            pv.RPC("RPC_Card", RpcTarget.AllBuffered, true, SideUp);
            placePicked.GetComponent<BoxTaken>().lastName = name;
            placePicked.GetComponent<BoxTaken>().taken = true;

            placePicked.GetComponent<BoxTaken>().MonsterID = MonsterID;
        }
        else
        {
            // Add damge / health
            //Show Card & ActualCard Level
            pv.RPC("RPC_Card3", RpcTarget.AllBuffered, "II", "III", false, 10f, 100f);
        }

        OGPlace = transform.position;

        gems = new List<GameObject>();
        ischosen = false;
        //PunOVRGrabbable.aboll = false;
        if (!alreadyChosen)
        {
            managerScript.gold += -price;
            if (!levelUpCard)
            {
                pv.RPC("AddMonsters", RpcTarget.AllBuffered);
                managerScript.populationOut += 1;
            }

            levelUpCard = false;
            alreadyChosen = true;
            managerScript.InfoPanel.gameObject.SetActive(false);
            managerScript.InfoPanel.gameObject.SetActive(true);
            managerScript.UpdateSideText();

        }

        ischosen = false;
        PunOVRGrabbable.aboll = false;
    }


    [PunRPC]
    public void Rotate_Card(bool t)
    {
        MonsterScripts.canStart = true;

        ActualCard.transform.rotation = rot2.rotation;
        //ActualCard.GetComponent<MonsterScripts>().ResetMonster();
        ActualCard.SetActive(t);
        MonsterScripts.canStart = true;

    }

    [PunRPC]
    public void Rotate_Card2(bool t)
    {
        ActualCard.transform.rotation = rot1.rotation;
        //ActualCard.GetComponent<MonsterScripts>().ResetMonster();
        ActualCard.SetActive(t);
        MonsterScripts.canStart = false;

    }

    [PunRPC]
    public void AddMonsters()
    {
        managerScript.MonstersChosen.Add(ActualCard);
    }

    [PunRPC]
    public void RPC_PlacePicked()
    {
        placePicked = gems[gems.Count - 1].transform;
    }


    [PunRPC]
    public void TurnOffMonster(bool t)
    {
        ActualCard.SetActive(t);

    }

    [PunRPC]
    public void RPC_Card(bool b, bool su)
    {
        ActualCard.GetComponent<MonsterScripts>().managerScript = managerScript;
        ActualCard.GetComponent<MonsterScripts>().CardReference = this;
        ActualCard.GetComponent<MonsterScripts>().ResetMonster();
        ActualCard.SetActive(b);
        if(su)
            ActualCard.transform.rotation = rot1.rotation;
        else
            ActualCard.transform.rotation = rot2.rotation;
    }


    [PunRPC]
    public void RPC_Card2(float x, float y, float z)
    {
        ActualCard.transform.position = new Vector3(x, y, z);
    }

    [PunRPC]
    public void RPC_Card3(string lvl2t, string lvl3t, bool i, float damage, float health)
    {
        //if (!photonView.IsMine) return;
        if(photonView.IsMine)
        {
            PlacementsScript ps1 = placePicked.GetComponent<BoxTaken>().firstCard.GetComponent<PlacementsScript>();
            ps1.level++;
            if (ps1.level == 2)
            {
                ps1.ActualCard.GetComponent<MonsterScripts>().attackDamage += damage;
                ps1.ActualCard.GetComponent<MonsterScripts>().health += health;
                ps1.ActualCardLevelText.text = lvl2t;
                ps1.CardLevelText.text = lvl2t;
            }
            else if (ps1.level == 3)
            {
                ps1.ActualCard.GetComponent<MonsterScripts>().attackDamage += damage;
                ps1.ActualCard.GetComponent<MonsterScripts>().health += health;
                ps1.ActualCardLevelText.text = lvl3t;
                ps1.CardLevelText.text = lvl3t;
            }
        }
        transform.GetChild(0).gameObject.SetActive(i);
        transform.GetChild(1).gameObject.SetActive(i);
        GetComponent<BoxCollider>().enabled = i;
        ActualCard.SetActive(i);
        this.enabled = i;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "placements" && CanMove)
        {
            //ischosen = true;
            gem = other.gameObject;
            gems.Add(gem);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "placements" && CanMove)
        {
            //ischosen = false;
            other.gameObject.GetComponent<Renderer>().material = m_Material2;
            gems.Remove(other.gameObject);

        }
    }
}

