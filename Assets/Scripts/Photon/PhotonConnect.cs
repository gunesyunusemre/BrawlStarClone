using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PhotonConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text UserNameCreateRoomText;
    [SerializeField]
    private Text UserNameRoomText;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            GameBeginSetting();
        }
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    
    private void GameBeginSetting() //network run settings
    {
        
        Debug.Log("Connecting to server");
        //AuthenticationValues authValues = new AuthenticationValues("0");
        //PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.SendRate = 40; //20 default
        PhotonNetwork.SerializationRate = 20; //10 default
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.NickName == "")
        {
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        }//Else Go PlayfabSignUp Script and look GetAccountInfoSuccesss func  
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() // main host_user connection
    {
        Debug.Log("Connected to Photon.", this);
        Debug.Log("My nickname is "+PhotonNetwork.LocalPlayer.NickName,this);
        AfterLogin();
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

    }

    private void AfterLogin() // after login update text data
    {
        UserNameCreateRoomText.text = PhotonNetwork.NickName;
        UserNameRoomText.text = PhotonNetwork.NickName;
        RoomCanvases.RC.GetPlayerStats();
    }

    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.LogErrorFormat("Error authenticating to Photon using Facebook: {0}", debugMessage);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason" + cause.ToString());
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        if (PhotonNetwork.CurrentRoom.PlayerCount%2==0)
            {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "red" } });
        }else
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", "blue" } });
        }
    }

    


}
