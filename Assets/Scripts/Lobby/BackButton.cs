using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* WaitingRoom Scene에서의 BackButton에 달린 스크립트 입니다.
 * NetManager를 이용하여 서버와 클라이언트와의 접속을 끊습니다.
 */

public class BackButton : MonoBehaviour
{
    private NetManager netManager;

    private void Start()
    {
        netManager = NetManager.instance;
    }

    public void PressBack()
    {
        //game stop
        if (netManager.roomSlots[0].isClientOnly)
        {
            netManager.StopClient();
        }
        else
        {
            netManager.StopHost();
        }
        
    }
}
