using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkAnimator))]
public class ItemBoxNetBehaviour : NetworkBehaviour
{
    [SyncVar]
    public bool IsOpen = false;

    public void CheckOpen()
    {
        CmdCheckOpen();
    }

    [Command]
    private void CmdCheckOpen()
    {
        IsOpen = true;
    }

    
}
