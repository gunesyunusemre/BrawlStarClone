using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListingsMenu _playerListingsMenu;
    [SerializeField]
    private LeaveRoomMenu _leaveRoomMenu;
    public LeaveRoomMenu LeaveRoomMenu { get { return _leaveRoomMenu; } }


    private RoomCanvases _roomCanvases;

    public void FirstInitialized(RoomCanvases canveses)
    {
        _roomCanvases = canveses;
        _playerListingsMenu.FirstInitialize(canveses);
        _leaveRoomMenu.FirsInitialize(canveses);
    }

    public void ShowOrHide(int _status)
    {
        switch(_status)
        {
            case 1:
                gameObject.SetActive(true);
                break;
            case 2:
                gameObject.SetActive(false);
                break;
        }
    }

}
