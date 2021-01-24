using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class LookAtCamera : MonoBehaviourPun
{
    private GameObject player;
    private PlayerScript playerScript;

    private void Start()
    {
        player = GameObject.Find(PhotonNetwork.NickName);
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles);
        

        if (playerScript.Team == "red")
        {
            transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles) * Quaternion.Euler(0,-180,0);
        }
        else if (playerScript.Team == "blue")
        {
            transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles);
        }
        
    }
}
