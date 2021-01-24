using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class QuickInstantiate : MonoBehaviour
{
    private string characterName;

    [SerializeField]
    private Transform TeamBlue;
    [SerializeField]
    private Transform TeamRed;

    private void Awake()
    {
        SpawnYourself();
    }

    private void SpawnYourself()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == PhotonNetwork.NickName)
            {
                string team = player.CustomProperties["team"].ToString();
                if (team == "red")
                {
                    Vector2 offset = Random.insideUnitCircle * 8f;
                    Vector3 position = new Vector3(TeamRed.position.x + offset.x, Vector3.zero.y + 1f, TeamRed.position.z + offset.y);
                    characterName = PlayerPrefs.GetString("CharacterName");

                    GameObject myPlayer = PhotonNetwork.Instantiate(characterName, position, Quaternion.Euler(0,180,0));
                    myPlayer.name = PhotonNetwork.NickName;
                    myPlayer.GetComponent<PlayerScript>().Team = "red";
                }
                else if (team == "blue")
                {
                    Vector2 offset = Random.insideUnitCircle * 8f;
                    Vector3 position = new Vector3(TeamBlue.position.x + offset.x, Vector3.zero.y + 1f, TeamBlue.position.z + offset.y);
                    characterName = PlayerPrefs.GetString("CharacterName");

                    GameObject myPlayer = PhotonNetwork.Instantiate(characterName, position, Quaternion.identity);
                    myPlayer.name = PhotonNetwork.NickName;
                    myPlayer.GetComponent<PlayerScript>().Team = "blue";
                }
            }
        }
    }

}


#region Quick Instantiate
/*Vector2 offset = Random.insideUnitCircle * 8f;
        Vector3 position = new Vector3(transform.position.x + offset.x, Vector3.zero.y + 1f , transform.position.z + offset.y);
        characterName = PlayerPrefs.GetString("CharacterName");

        //MasterManager.NetworkInstantiate(_prefab, position, Quaternion.identity);
        GameObject player = PhotonNetwork.Instantiate(characterName, position, Quaternion.identity);
        player.name = PhotonNetwork.NickName;*/

#endregion
