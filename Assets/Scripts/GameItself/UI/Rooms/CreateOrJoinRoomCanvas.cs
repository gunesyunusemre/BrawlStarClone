using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu _createRoomMenu;
    [SerializeField]
    private RoomsListingsMenu _roomsListingsMenu;

    private RoomCanvases _roomCanvases;

    public void FirstInitialized(RoomCanvases canveses)
    {
        _roomCanvases = canveses;
        _createRoomMenu.FirstInitialized(canveses);
        _roomsListingsMenu.FirstInitialize(canveses);
    }

}
