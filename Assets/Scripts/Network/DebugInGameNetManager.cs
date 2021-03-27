using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;

/*  인게임중에서의 NetManger입니다. 디버깅용입니다.
 *  
 *  Room기능을 사용하지 않기떄문에 NetManager를 상속하지 않았습니다. 
 *  따라서 NetManager의 인자나 메소드 사용 못합니다.
 *  
 *  GamePlay Scene에서 디버깅용으로 생성될 예정입니다. 오직 GamePlay Scene에서만 돌아가게 될겁니다.
 *  
 *  다른씬에서 NetworkManager가 넘어오면 이 오브젝트는 파괴됩니다.
 * 
 *  GamePlay Scene에서 작동하게 될경우 GUI가 뜨게되는데
 *  
 *  Host버튼을 누르시면 서버 작동 + 플레이어가 스폰됩니다.
 *  Client버튼을 누르시면 Host가 열어둔 서버에 접속하여 다른 플레이어가 스폰됩니다.
 *  다중 클라이언트 접속 확인할려면 유니티 위 메뉴에 ParrelSync > Clones Manager로 복사된 다른 에디터 여시면 됩니다.
 *  
 *  
 *  
 */ 
public class DebugInGameNetManager : NetworkManager
{
    private GameMgr gameMgr;

    public override void OnStartServer()
    {
        gameMgr = GameMgr.instance;
        gameMgr.Init();
        base.OnStartServer();
    }

}
