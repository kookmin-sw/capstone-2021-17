using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartButton : NetworkBehaviour
{
    private NetManager netManager;
    private bool isSpawned = false;

    public override void OnStartClient()
    {
        netManager = NetManager.instance;
        this.gameObject.SetActive(false);
    }
    public override void OnStartServer()
    {
        netManager = NetManager.instance;
        this.gameObject.SetActive(false);
    }

    public void AssignAuthority(NetRoomPlayer player)
    {
        if (!isSpawned)
        {
            NetworkServer.Spawn(gameObject, player.connectionToClient);
        }
        else
        {
            NetworkIdentity identity = GetComponent<NetworkIdentity>();
            identity.AssignClientAuthority(player.connectionToClient);

        }
    }

    public void PressStart()
    {
        //game start!
        cmdPressStart();
        
    }



    [Command] // called after AssignAuthority
    private void cmdPressStart()
    {
        netManager.ServerChangeScene(netManager.GameplayScene);
    }




}
