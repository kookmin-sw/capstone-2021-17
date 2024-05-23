using UnityEditor;
using UnityEngine;
using Mirror;


public class DroppedItemNetBehaviour : NetworkBehaviour
{
    public void SetActive(bool isActive)
    {
        CmdSetActive(isActive);
    }


    [Command(requiresAuthority = false)]
    private void CmdSetActive(bool isActive)
    {
        RpcSetActive(isActive);
    }

    [ClientRpc]
    private void RpcSetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
