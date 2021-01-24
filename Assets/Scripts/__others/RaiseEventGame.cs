using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RaiseEventGame : MonoBehaviourPunCallbacks
{
    private Renderer _render;

    private const byte COLOR_CHANGE_EVENT = 0;

    private void Awake()
    {
        _render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (base.photonView.IsMine&&Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColor();
        }
        if (!base.photonView.IsMine)
        {
            base.GetComponent<RaiseEventGame>().enabled = false;
        }
    }


    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData Obj)
    {
        if (Obj.Code==COLOR_CHANGE_EVENT)
        {
            object[] datas = (object[])Obj.CustomData;
            float r = (float)datas[0];
            float g = (float)datas[1];
            float b = (float)datas[2];
            _render.material.color = new Color(r, g, b, 1);
        }
    }

    private void ChangeColor()
    {
        float r = Random.Range(0, 1f);
        float g = Random.Range(0, 1f);
        float b = Random.Range(0, 1f);

        _render.material.color = new Color(r, g, b, 1);


        //object[] datas = new object[] { base.photonView.ViewID, r, g, b };
        object[] datas = new object[] { r, g, b };

        PhotonNetwork.RaiseEvent(COLOR_CHANGE_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendReliable);


    }

}
