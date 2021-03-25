using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/* WaitingRoomScene에 적용된 StartButton 오브젝트에 적용된 스크립트 입니다.
 * NetworkBehaviour를 적용하여 서버에 단 한명에 플레이어만이 작동할 수 있게 했습니다.
 * 
 * 버튼을 누를경우 Game이 시작됩니다
 */

public class StartButton : MonoBehaviour
{
    private NetManager netManager;

    private void Awake()
    {
        netManager = NetManager.instance;
        this.gameObject.SetActive(false);
    }

    //플레이어에게 StartButton이 보일 수 있게 됨. (타 스크립트에서 리더에게 할당함)

    // OnPressedButtonStart
    public void PressStart()
    {
        //game start!
        netManager.ServerChangeScene(netManager.GameplayScene);
    }

}
