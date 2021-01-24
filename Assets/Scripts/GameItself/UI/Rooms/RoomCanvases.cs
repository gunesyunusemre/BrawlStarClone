using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCanvases : MonoBehaviour
{
    public static RoomCanvases RC;

    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas;
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas { get { return _createOrJoinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    [SerializeField]
    private Text ExpText;

    private void OnEnable()
    {
        RoomCanvases.RC = this;
    }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        CreateOrJoinRoomCanvas.FirstInitialized(this);
        CurrentRoomCanvas.FirstInitialized(this);
    }

    public void GetPlayerStats()
    {
        ExpText.text = "Exp: " + PlayfabPlayerStatsSystem.PFSS.exp.ToString();
    }

}
