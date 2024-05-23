using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransformChild))] // transform doorTransform
public class DoorNetBehaviour : NetworkBehaviour
{
    [SerializeField]
    private DoorController controller;

    [SerializeField]
    private LeverController leverController;

    [SyncVar]
    public bool IsOpen;

    [Command(requiresAuthority = false)] // send to Server
    public void CmdOpenDoor()
    {
        IsOpen = true;
        controller.OpenDoor();
    }

    [Command(requiresAuthority = false)] // send to Server
    public void CmdCloseDoor()
    {
        IsOpen = false;
        controller.CloseDoor();
    }

    [Command(requiresAuthority = false)]
    public void CmdPullDownLever(GameObject lever_obj)
    {
        RpcPullDownLever(lever_obj);
    }

    [ClientRpc]
    public void RpcPullDownLever(GameObject lever_obj)
    {
        leverController.PullDownLever(lever_obj);
    }
}
