using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// It is the master class of the bullet
/// </summary>
public class Bullet : MonoBehaviourPun
{
    private GameObject Player;
    public PlayerScript playerScript; //This is working in the PlayerHealth script(OnTriggerEnter Method). 
    public string Team;

    //These values is Bullet move values------------
    private bool isMove=false;
    private Vector3 targetPos;
    private Vector3 pos;
    //---------------------------------------------

    //These values is Characteristic values--------
    private float range; 
    private int speed;
    public int damage; //This is working in the PlayerHealth script(OnTriggerEnter Method). 
    //---------------------------------------------


    private void Awake()
    {
        Player = GameObject.Find(photonView.Owner.NickName);
        playerScript = Player.GetComponent<PlayerScript>();
        range = playerScript.BulletRange;
        speed = playerScript.BulletSpeed;
        damage = playerScript.BulletDamage;
    }

    private void OnEnable()
    {
        Team = playerScript.Team;
    }

    void Update()
    {
        if (!isMove)
        {
            return;
        }
        MoveBullet();
    }

    /// <summary>
    /// Triggers the action of the bullet
    /// </summary>
    /// <param name="_pos">It is the position when it started</param>
    public void SetBulletValues(Vector3 _pos)
    {
        targetPos = _pos;
        isMove = true;
    }

    /// <summary>
    /// Moves the bullet
    /// </summary>
    private void MoveBullet()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        pos = transform.position;
        float distance = Vector3.Distance(targetPos, pos);
        if (distance>=range)
        {
            isMove = false;
            playerScript.Bullets.Add(this.gameObject);
            targetPos = Vector3.zero;
            photonView.RPC("SetActive", RpcTarget.All, false);
        }
        else
        {
            isMove = true;
        }
    }

    /// <summary>
    /// Manages your activity
    /// </summary>
    /// <param name="isActive">It is the activity value.</param>
    [PunRPC]
    private void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

}
