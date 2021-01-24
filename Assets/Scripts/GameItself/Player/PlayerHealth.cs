using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// It is the class that manages the player's life.
/// </summary>
public class PlayerHealth : MonoBehaviourPunCallbacks
{
    public PlayerScript playerScript;

    [SerializeField]
    private Image fillImg;


    //Health Values------------------------
    private float health=1;
    private float maxHealth=1;
    //-------------------------------------

    //Regeneration values------------------
    private float healthRegenerateValue = 1;
    private float maxTime=5f;
    private float time;
    //-------------------------------------

    private void Awake()
    {
        SetTarget();
    }

    private void Start()
    {
        time = maxTime;
    }

    private void LateUpdate()
    {
        HealthRegenerate();
        fillImg.fillAmount = health / maxHealth;

        if (!photonView.IsMine)
        {
            return;
        }

        SpectatorMode();


    }

    /// <summary>
    /// Death Cam Active
    /// </summary>
    private void SpectatorMode()
    {
        if (health <= 0)
        {
            Destroy(GameObject.Find("Camera"));
            PhotonNetwork.RemoveRPCs(photonView);
            PhotonNetwork.Destroy(this.gameObject);
            GameObject myPlayer;
            if (playerScript.Team == "red")
            {
                myPlayer = PhotonNetwork.Instantiate("Ghost", this.gameObject.transform.position, Quaternion.Euler(0, 180, 0));
                myPlayer.GetComponent<PlayerScript>().Team = "red";
            }
            else
            {
                myPlayer = PhotonNetwork.Instantiate("Ghost", this.gameObject.transform.position, Quaternion.identity);
                myPlayer.GetComponent<PlayerScript>().Team = "blue";
            }
            myPlayer.name = PhotonNetwork.NickName;
            myPlayer.GetComponent<PlayerScript>().SpawnTime = 5;
            return;
        }
    }

    /// <summary>
    /// Find Player's Health values
    /// </summary>
    private void SetTarget()
    {
        health= playerScript.Health;
        maxHealth = playerScript.MaxHealth;
        healthRegenerateValue = playerScript.HealthRegenerate;
        maxTime = playerScript.RegenCoolDown;
    }

    /// <summary>
    ///  Used for Update Health
    /// </summary>
    /// <param name="operation">Used 2 operation '+', '-'. Health Regeneration or Take Damage</param>
    /// <param name="value"></param>
    public void UpdateHealth(string operation, float value)
    {
        switch (operation)
        {
            case "+"://Health regeneration
                photonView.RPC("HealthIncrease", RpcTarget.All, value);
                break;
            case "-"://Take Damage
                photonView.RPC("TakeDamage", RpcTarget.All, value);
                break;
        }
    }

    /// <summary>
    /// Health Increase's Operation is here. Special to PhotonNetwork
    /// </summary>
    /// <param name="value">Value is Increase Value</param>
    [PunRPC]
    private void HealthIncrease(float value)
    {
        health = health + value;
    }

    /// <summary>
    /// Health Decreasing's Operation is here. Special to PhotonNetwork
    /// </summary>
    /// <param name="value">Value is Decreasing Value</param>
    [PunRPC]
    private void TakeDamage(float value)
    {
        health = health - value;
        time = maxTime;
        fillImg.fillAmount = health / maxHealth;
    }


    /// <summary>
    /// Regenerates life depending on time
    /// </summary>
    private void HealthRegenerate()
    {
        if (health < maxHealth)
        {
            if (time <= 0)
            {
                UpdateHealth("+", healthRegenerateValue * Time.deltaTime);
            }
            time -= Time.deltaTime;
        }
        else
        {
            time = maxTime;
        }
    }

    /// <summary>
    /// It is the method that detects the bullet contact.
    /// </summary>
    /// <param name="other">Other is the bullet</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            if (other.gameObject.GetComponent<Bullet>().Team==playerScript.Team)
                return;

            if (!photonView.IsMine)
            {
                other.gameObject.GetComponent<Bullet>().photonView.RPC("SetActive", RpcTarget.All, false);
                other.gameObject.GetComponent<Bullet>().playerScript.Bullets.Add(other.gameObject);
                UpdateHealth("-", other.gameObject.GetComponent<Bullet>().damage);
            }
        }
    }


}
