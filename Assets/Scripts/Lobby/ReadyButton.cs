using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReadyButton : MonoBehaviour
{
    private NetManager netManager;

    private void Awake()
    {
        netManager = NetManager.instance;
    }

    public void PressReady()
    {
        foreach(NetRoomPlayer player in netManager.roomSlots)
        {
            if (player.isLocalPlayer)
            {
                if (player.readyToBegin)
                {
                    player.CmdChangeReadyState(false);
                }
                else
                {
                    player.CmdChangeReadyState(true);
                }
                return;
            }
        }
    }
   
    
}
