using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkAnimator))] // lever animation
public class DoorNetBehaviour : NetworkBehaviour
{
    [SerializeField]
    private DoorController controller;

    [SyncVar]
    public bool IsOpen;

    [Command(ignoreAuthority = true)] // send to Server
    public void OpenDoor()
    {
        IsOpen = true;
        controller.OpenDoor();
    }

    [Command(ignoreAuthority = true)] // send to Server
    public void CloseDoor()
    {
        IsOpen = false;
        controller.CloseDoor();
    }
}
