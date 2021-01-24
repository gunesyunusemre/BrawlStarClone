using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun//, IPunObservable
{
    public PlayerScript playerScript;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool IsGroundedPlayer;
    private int jumpCount;

    [Header("Player Properties")]
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public int maxJumpCount = 2;


    private float gravityValue = -9.81f;


    private float axisHorizontal;
    private float axisVertical;

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }*/

    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        if (!base.photonView.IsMine)
        {
            base.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    private void Update()
    {
        if (base.photonView.IsMine)
        {
            MovementStatement();
        }

    }

    private void MovementStatement()
    {
        IsGroundController();
        MoveController();
        JumpController();       

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void IsGroundController()
    {
        if (photonView.IsMine)
        {
            IsGroundedPlayer = controller.isGrounded;
            if (IsGroundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                jumpCount = 0;
            }
        }
    }

    private void MoveController()
    {
        if (photonView.IsMine)
        {
            axisHorizontal = Input.GetAxis("Horizontal");
            axisVertical = Input.GetAxis("Vertical");

            if (playerScript.Team == "red")
            {
                axisHorizontal = -axisHorizontal;
                axisVertical = -axisVertical;
            }

            Vector3 move = new Vector3(axisHorizontal, 0, axisVertical);
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
    }

    private void JumpController()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                jumpCount++;
            }
        }
    }




}
