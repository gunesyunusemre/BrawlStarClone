using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// This class is the master class that manages the characteristics of the player.
/// </summary>
public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    public static PlayerScript PlayerS;

    public static PlayerHealth healthScript;

    private GameObject Camera;

    private GameObject Player;

    //Score working playfab save data;
    private int _score;
    public int Score { get { return _score; } set { _score = value; } }

    [SerializeField]
    private string _team;
    public string Team { get { return _team; } set { _team = value; } }

    [SerializeField]
    private float spawnTime;
    public float SpawnTime { get { return spawnTime; } set { spawnTime = value; } }

    #region Bullet Field-------------------------------------------------------------
    [Header("Bullet Values")]
    [Tooltip("Bullet Type")]
    [SerializeField]
    private GameObject _bullet;
    [Tooltip("Bullet Range Value")]
    [SerializeField]
    private int _bulletRange;
    public int BulletRange { get { return _bulletRange; } set { _bulletRange = value; } }
    [Tooltip("Bullet Speed Value")]
    [SerializeField]
    private int _bulletSpeed;
    public int BulletSpeed { get { return _bulletSpeed; } set { _bulletSpeed = value; } }
    [Tooltip("Bullet Damage Value")]
    [SerializeField]
    private int _bulletDamage;
    public int BulletDamage { get { return _bulletDamage; } set { _bulletDamage = value; } }
    [Tooltip("Reload Time Value")]
    [SerializeField]
    private int _reloadTime;
    public int ReloadTime { get { return _reloadTime; } set { _reloadTime = value; } }

    private List<GameObject> _bullets = new List<GameObject>();
    public List<GameObject> Bullets { get { return _bullets; } set { _bullets = value; } }

    private GameObject BulletsGameObj;
    #endregion --------------------------------------------------------------------

    #region Health Field-------------------------------------------------------------

    [Header("Initial Health Values")]
    [Tooltip("Start Health Value")]
    [SerializeField]
    private int _health;
    public int Health { get { return _health; } set { _health = value; } }

    [Tooltip("Start Maximum Health Value")]
    [SerializeField]
    private int _maxHealth;
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    [Header("Health Regeneration Values")]
    [Tooltip("Health Regen per second")]
    [SerializeField]
    private int _healthRegenerate;
    public int HealthRegenerate { get { return _healthRegenerate; } set { _healthRegenerate = value; } }

    [Tooltip("Cooldown for health regen")]
    [SerializeField]
    private int _regenCoolDown;
    public int RegenCoolDown { get { return _regenCoolDown; } set { _regenCoolDown = value; } }
    #endregion ----------------------------------------------------------------------


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Team);
        }
        else if (stream.IsReading)
        {
            Team = (string)stream.ReceiveNext();
        }
    }


    private void OnEnable()
    {
        PlayerScript.PlayerS = this;
        transform.name = photonView.Owner.NickName;
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Player = GameObject.Find(PhotonNetwork.NickName);

        PrepareCamera();

        if (this.gameObject.tag == "Ghost")
            return;


        PrepareBullets();

        healthScript = Player.GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Prepares the camera at the beginning
    /// </summary>
    private void PrepareCamera()
    {
        Camera = Player.transform.Find("Camera").gameObject;
        Camera.GetComponent<Follow>().enabled = true;
        Camera.GetComponent<Follow>().SetTarget(Player.transform);
        Camera.SetActive(true);
    }

    /// <summary>
    /// Creates and prepares bullets
    /// </summary>
    private void PrepareBullets()
    {
        BulletsGameObj = GameObject.Find("Bullets");
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = PhotonNetwork.Instantiate(_bullet.name, Vector3.zero, Quaternion.identity);
            bullet.GetComponent<Bullet>().photonView.RPC("SetActive", RpcTarget.All, false);
            bullet.name = photonView.Owner.NickName + "Bullet" + (i+1);
            bullet.transform.parent = BulletsGameObj.transform;
            Bullets.Add(bullet);
        }
    }



}