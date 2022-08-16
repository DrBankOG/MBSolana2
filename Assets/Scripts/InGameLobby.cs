using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chiligames.MetaAvatarsPun;

public class InGameLobby : MonoBehaviour
{

    public GameObject WorldSpawnPoint;
    public GameObject BH;
    public GameObject BC;
    public Transform PortalPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<manager>().WorldSpawnPointRef == null)
        {
            other.GetComponent<manager>().WorldSpawnPointRef = WorldSpawnPoint;
            other.GetComponent<manager>().BlackHole = BH;
            other.GetComponent<manager>().BlackImage = BC;
            other.GetComponent<manager>().PortalPos = PortalPos;
            gameObject.SetActive(false);
            //ls.ContinuousMovementEnabled = true;
        }
    }
}
