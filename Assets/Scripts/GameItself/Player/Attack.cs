using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// This class manages the attack
/// </summary>
public class Attack : MonoBehaviourPunCallbacks, IPunObservable
{
    public PlayerScript playerScript;

    [SerializeField]
    private Image fillImg;

    //Bullet values-------------
    [SerializeField]
    Transform FireBase;
    private Vector3 Pos;
    public float bulletCount;
    //--------------------------

    //Reload values-------------
    private float time;
    private bool isReload = false;
    //--------------------------


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(bulletCount);
            stream.SendNext(playerScript.Team);
        }
        else if (stream.IsReading)
        {
            bulletCount = (float)stream.ReceiveNext();
            playerScript.Team = (string)stream.ReceiveNext();
        }
        fillImg.fillAmount = bulletCount / 3f;
    }


    void Start()
    {
        if (!photonView.IsMine)
        {
            GetComponent<Attack>().enabled = false;
            return;
        }
        time = playerScript.ReloadTime;
    }

    private void Update()
    {
        //This "if" controls "is this object mine?"
        if (!photonView.IsMine)
        {
            return;
        }
        
        //This "if" controls Reload time
        if (isReload)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                isReload = false;
                time = playerScript.ReloadTime;
            }
            return;
        }

        //This "if" controls the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (isReload)
            {
                return;
            }
            if (playerScript.Bullets.Count == 0)
            {
                isReload = true;
                return;
            }
            foreach (GameObject bullet in playerScript.Bullets)
            {
                FireBullet(bullet);
                break;
            }
        }
    }

    private void LateUpdate()
    {
        bulletCount = (float)playerScript.Bullets.Count;
        fillImg.fillAmount = bulletCount / 3f;
    }

    /// <summary>
    /// It is the method that fires the bullet.
    /// </summary>
    /// <param name="bullet">Found bullets</param>
    [PunRPC]
    private void FireBullet(GameObject bullet)
    {
        bullet.transform.position = FireBase.position;
        bullet.transform.rotation = FireBase.rotation;
        bullet.GetComponent<Bullet>().photonView.RPC("SetActive", RpcTarget.All, true);
        Pos = bullet.transform.position;
        bullet.GetComponent<Bullet>().SetBulletValues(Pos);
        playerScript.Bullets.Remove(bullet);
    }
}
