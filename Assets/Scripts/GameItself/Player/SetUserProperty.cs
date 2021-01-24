using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Set User Color and User Nick Name in the Game
/// </summary>
public class SetUserProperty : MonoBehaviourPunCallbacks//, IPunObservable
{
    [SerializeField]
    private TextMesh NickNameText;
    [SerializeField]
    private Image HealthBar;

    private Renderer _render;
    private string NickName;
    private string team;
    float r=1, g=1, b=1, a=1;

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
            stream.SendNext(r);
            stream.SendNext(g);
            stream.SendNext(b);
            //stream.SendNext(a);
            stream.SendNext(NickName);
        }
        else if (stream.IsReading)
        {
            r = (float)stream.ReceiveNext();
            g = (float)stream.ReceiveNext();
            b = (float)stream.ReceiveNext();
            NickName = (string)stream.ReceiveNext();
            // a = (float)stream.ReceiveNext();
        }
        _render.material.color = new Color(r, g, b, a);
        NickNameText.GetComponent<TextMesh>().text = NickName;
    }*/

    private void Awake()
    {
        _render = GetComponent<Renderer>();
        if (base.photonView.IsMine)
        {
            photonView.RPC("SetTeam", RpcTarget.All);
            photonView.RPC("Set_NickName", RpcTarget.All);
            photonView.RPC("SetHealthBarColor", RpcTarget.All);
        }
    }

    void Update()
    {
        if (base.photonView.IsMine)
        {
            //Todo: In game changes
        }
        
    }

    [PunRPC]
    private void SetTeam()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == photonView.Owner.NickName)
            {
                team = player.CustomProperties["team"].ToString();
                break;
            }
        }
    }

    [PunRPC]
    private void Set_NickName()
    {
        if (team=="red")
        {
            NickNameText.color = Color.red;
        }
        else if (team == "blue")
        {
            NickNameText.color = Color.blue;
        }
        NickName = photonView.Owner.NickName;
        NickNameText.text = NickName;
    }

    [PunRPC]
    private void SetHealthBarColor()
    {
        if (team == "red")
        {
            HealthBar.color = Color.red;
        }
        else if (team == "blue")
        {
            HealthBar.color = Color.blue;
        }
    }



    [PunRPC]
    private void CatcherColor()
    {
         r = 1;
         g = 0; 
         b = 0;//Random.Range(0, 1f);

        _render.material.color = new Color(r, g, b, 1);
    }

    [PunRPC]
    private void OldCatcherColor()
    {
        r = 0;
        g = 1; 
        b = 0;

        _render.material.color = new Color(r, g, b, 0.5f);
    }

    [PunRPC]
    private void RunnerColor()
    {
        r = 0;
        g = 0;
        b = 1;
        
        _render.material.color = new Color(r, g, b, 1);
    }


}
