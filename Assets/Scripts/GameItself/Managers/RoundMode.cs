using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoundMode : MonoBehaviour
{
    public static RoundMode roundMode;

    [SerializeField]
    private Transform TeamBlue;
    [SerializeField]
    private Transform TeamRed;
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


    public bool isEndGame = false;
    [SerializeField]
    private string winText;
    private Color winColor;
    [SerializeField]
    private float SpawnTimeAllPlayer;

    private string characterName;

    private GameObject[] Ghosts;
    private List<GameObject> blueGhosts = new List<GameObject>();
    private List<GameObject> redGhosts = new List<GameObject>();
    private GameObject[] players;
    private string[] bluePlayersName = { "", "", "" };
    private string[] redPlayersName = { "", "", "" };

    private byte redCount = 0;
    private byte blueCount = 0;

    public byte blueKillCount = 0;
    public byte redKillCount = 0;

    private void Awake()
    {
        RoundMode.roundMode = this;
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

            if (Ghost.GetComponent<PlayerScript>().Team == "red")
            {
                
                redGhosts.Add(Ghost);
            }
            else if (Ghost.GetComponent<PlayerScript>().Team == "blue")
            {
                
                blueGhosts.Add(Ghost);
            }
        }

        Spawner();
    }

    private void Spawner()
    {
        if (redGhosts.Count>0)
        {
            foreach (GameObject ghost in redGhosts)
            {
                if (ghost == null)
                    return;

                if (PhotonNetwork.NickName != ghost.name)
                    return;

                ghost.GetComponent<PlayerScript>().SpawnTime -= Time.deltaTime;
                //Debug.Log(ghost.name + ": " + ghost.GetComponent<PlayerScript>().SpawnTime);
                if (ghost.GetComponent<PlayerScript>().SpawnTime<=0)
                {
                    Debug.Log("Red Spawning");
                    PhotonNetwork.Destroy(ghost);
                    Destroy(GameObject.Find("Camera"));
                    Vector2 offset = Random.insideUnitCircle * 8f;
                    Vector3 position = new Vector3(TeamRed.position.x + offset.x, Vector3.zero.y + 1f, TeamRed.position.z + offset.y);
                    characterName = PlayerPrefs.GetString("CharacterName");

                    GameObject myPlayer = PhotonNetwork.Instantiate(characterName, position, Quaternion.Euler(0, 180, 0));
                    myPlayer.name = PhotonNetwork.NickName;
                    myPlayer.GetComponent<PlayerScript>().Team = "red";
                    blueKillCount++;
                }
            }
            redGhosts.Clear();
            
        }
        if (blueGhosts.Count>0)
        {
            foreach (GameObject ghost in blueGhosts)
            {
                if (ghost == null)
                    return;

                if (PhotonNetwork.NickName != ghost.name)
                    return;

                ghost.GetComponent<PlayerScript>().SpawnTime -= Time.deltaTime;
                Debug.Log(ghost.name + ": " + ghost.GetComponent<PlayerScript>().SpawnTime);
                if (ghost.GetComponent<PlayerScript>().SpawnTime <= 0)
                {
                    Debug.Log("Blue Spawning");
                    PhotonNetwork.Destroy(ghost);
                    Destroy(GameObject.Find("Camera"));
                    Vector2 offset = Random.insideUnitCircle * 8f;
                    Vector3 position = new Vector3(TeamBlue.position.x + offset.x, Vector3.zero.y + 1f, TeamBlue.position.z + offset.y);
                    characterName = PlayerPrefs.GetString("CharacterName");

                    GameObject myPlayer = PhotonNetwork.Instantiate(characterName, position, Quaternion.identity);
                    myPlayer.name = PhotonNetwork.NickName;
                    myPlayer.GetComponent<PlayerScript>().Team = "blue";
                    redKillCount++;

                }                    
            }
            blueGhosts.Clear();
        }
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

    public void GameEnd()
    {
        Debug.Log("End Game");
        if (redKillCount>blueKillCount)
        {
            Debug.Log("Red Win");
            GetEndGameCanvas();
        }
        else if(redKillCount > blueKillCount)
        {
            Debug.Log("Blue Win");
            GetEndGameCanvas();
        }
    }


    private void GetPlayerWithTeam()
    {
        redCount = 0;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerScript>().Team == "red")
            {
                redPlayersName[redCount] = player.name;
                redCount++;
            }
            else if (player.GetComponent<PlayerScript>().Team == "blue")
            {
                bluePlayersName[blueCount] = player.name;
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
