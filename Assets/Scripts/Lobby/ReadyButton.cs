using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/* WaitingRoomScene에 적용된 ReadyButton 오브젝트에 적용된 스크립트 입니다.
 * NetManager를 이용하여 roomPlayer의 Ready 상태를 변화시킵니다.
 */
public class ReadyButton : MonoBehaviour
{
    private NetManager netManager;

    private void Start()
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
