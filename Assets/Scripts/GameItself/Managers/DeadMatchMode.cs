using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class DeadMatchMode : MonoBehaviourPun
{

    [SerializeField]
    private GameObject UICanvas;
    [SerializeField]
    private GameObject EndGameCanvas;
    [SerializeField]
    private Text WinTeamText;
    [SerializeField]
    private Button LobbyButton;
    [SerializeField]
    private Text[] WinnerPlayers;


    [SerializeField]
    private bool isEndGame = false;
    [SerializeField]
    private string winText;
    private Color winColor;

    private GameObject[] Ghosts;
    private GameObject[] players;
    private string[] bluePlayers = { "", "", "" };
    private string[] redPlayers = { "", "", "" };

    private byte redGhostCount=0;
    private byte redCount = 0;
    private byte blueGhostCount = 0;
    private byte blueCount = 0;

    private void Awake()
    {
        isEndGame = false;
        Time.timeScale = 1f;
        Invoke("GetPlayerWithTeam", 0.5f);
        LobbyButton.onClick.AddListener(OnClick_BackToLobby);
    }

    void Update()
    {
        if (isEndGame)
            return;

        Ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject Ghost in Ghosts)
        {
            if (Ghost.GetComponent<PlayerScript>().Team=="red")
            {
                redGhostCount++;
            }
            else if(Ghost.GetComponent<PlayerScript>().Team == "blue")
            {
                blueGhostCount++;
            }
        }
        if (redGhostCount>=(PhotonNetwork.CurrentRoom.PlayerCount / 2))
        {
            winText = "BLUE WIN!";
            winColor = Color.blue;
            for (int i = 0; i < WinnerPlayers.Length; i++)
            {
                WinnerPlayers[i].text = bluePlayers[i];

                if (PhotonNetwork.NickName==bluePlayers[i])
                {
                    Debug.Log("Exp Gain");
                    //PlayfabPlayerStatsSystem.PFSS.OnClick_SetStats();
                }

            }
            GetEndGameCanvas();
        }
        else if (blueGhostCount >= (PhotonNetwork.CurrentRoom.PlayerCount / 2))
        {
            winText = "RED WIN!";
            winColor = Color.red;
            for (int i = 0; i < WinnerPlayers.Length; i++)
            {
                WinnerPlayers[i].text = redPlayers[i];

                if (PhotonNetwork.NickName == redPlayers[i])
                {
                    Debug.Log("Exp Gain");
                    //PlayfabPlayerStatsSystem.PFSS.OnClick_SetStats();
                }

            }
            GetEndGameCanvas();
        }

        redGhostCount = 0;
        blueGhostCount = 0;
    }

    private void GetEndGameCanvas()
    {
        isEndGame = true;
        UICanvas.SetActive(false);
        EndGameCanvas.SetActive(true);
        Time.timeScale = 0.1f;
        WinTeamText.text = winText;
        WinTeamText.color = winColor;
    }

    private void GetPlayerWithTeam()
    {
        redCount = 0;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerScript>().Team == "red")
            {
                redPlayers[redCount] = player.name;
                redCount++;
            }
            else if (player.GetComponent<PlayerScript>().Team == "blue")
            {
                bluePlayers[blueCount] = player.name;
                blueCount++;
            }
        }
    }

    private void OnClick_BackToLobby()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.IsMasterClient)
                {
                    PhotonNetwork.SetMasterClient(player);
                }                
            }
        }
        SceneManager.LoadScene("Main");
    }

}
