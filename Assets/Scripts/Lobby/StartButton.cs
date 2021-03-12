using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/* WaitingRoomScene에 적용된 StartButton 오브젝트에 적용된 스크립트 입니다.
 * NetworkBehaviour를 적용하여 서버에 단 한명에 플레이어만이 작동할 수 있게 했습니다.
 * 
 * 버튼을 누를경우 Game이 시작됩니다
 */

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
