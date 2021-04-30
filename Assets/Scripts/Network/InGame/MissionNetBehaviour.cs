using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MissionNetBehaviour : NetworkBehaviour
{
    [SyncVar]
    public bool IsUsing = false;

    [SerializeField]
    private MissionController missionController;

    public void SetUsing(bool isUsing)
    {
        CmdSetUsing(isUsing);
    }

    [Command(requiresAuthority = false)]
    private void CmdSetUsing(bool isUsing)
    {
        IsUsing = isUsing;
    }

    public void UnableMission()
    {
        CmdUnableMission();
    }

    [Command(requiresAuthority = false)]
    private void CmdUnableMission()
    {
        RpcUnableMission();
    }

    [ClientRpc]
    private void RpcUnableMission()
    {
        missionController.UnableMission();
    }

    public void MissionClear()
    {

    }
}
