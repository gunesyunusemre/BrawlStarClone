using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Timer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text timeCounter;
    [SerializeField]
    private double startTime;
    [SerializeField]
    private double RoundTime;

    private void Start()
    {
        startTime = PhotonNetwork.Time;
    }

    private void Update()
    {
        if (RoundMode.roundMode.isEndGame)
            return;

        timeCounter.text = ((int)(RoundTime-(PhotonNetwork.Time - startTime))).ToString();
        if ((int)(RoundTime - (PhotonNetwork.Time - startTime))<=0)
        {
            RoundMode.roundMode.GameEnd();
        }
    }
}
