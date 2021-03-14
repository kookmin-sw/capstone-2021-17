using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;

/*   NetManager는 NetworkRoomManager 클래스를 상속하였습니다.
 *   Start - WaitingRoom - GamePlay 씬으로 이어지는 대부분의 네트워크 과정을 처리하며
 *   씬 전환도 합니다.
 *   
 *   대부분 이 프로젝트에 작성한Network 관련 스크립트가 이 NetManager의 싱글톤인 
 *   instance를 가져오는 식으로 처리하게 됩니다.
 *   
 *   이 NetManager의 부모인 NetworkRoomManager와 NetworkManager의 메소드 사용은
 *   https://mirror-networking.gitbook.io/docs/ 읽어주시면 감사해주겠습니다.
 *   
 *   서버의 상태, 버튼을 나타내는 디버깅용 GUI가 뜰텐데 
 *   
 *   Host버튼을 누르시면 서버 작동 + 플레이어가 스폰됩니다.
 *   Client버튼을 누르시면 Host가 열어둔 서버에 접속하여 다른 플레이어가 스폰됩니다.
 *   
 *   다중 클라이언트 접속 확인할려면 유니티 위 메뉴에 ParrelSync > Clones Manager로 복사된 다른 에디터 여시면 됩니다.
 *   
 *   GUI를 끄시려면 NetworkManager의 오브젝트에서 
          NetworkManagerHUD의 ShowGUI
          NetManager의 SHOW ROOM GUI 체크 풀으시면 됩니다.
 *   
 */

public class NetManager : NetworkRoomManager
{
    public static int PLAYER_MAXNUM = 4;

    public static NetManager instance;


    //singleton
    public override void Awake() 
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
    }
    /*
     * ---------------------- Start ---------------------
     * StartScene에서의 입력 처리들은 Start_MultiGameManager가 
     * Netmanager의 메소드를 호출하는 방식으로 이루어집니다.
     * netManager.StartClient() , netManager.StartHost() 이런 방식으로 합니다.
     * 
     * Start_MultiGameManger.cs 파일 한번 참고해주시면 좋을것같습니다.
     * 참고로 Start_MultiGameManger.cs 스크립트는 MainManager 오브젝트에 달아놨습니다.
     * 
     * 서버의 상태, 버튼을 나타내는 디버깅용 GUI가 뜰텐데 
     * 이거 끄시려면 NetworkManager의 오브젝트에서 
     *  . NetworkManagerHUD의 ShowGUI
     *  . NetManager의 SHOW ROOM GUI 체크 풀으시면 됩니다.
     *  
     *  



    /*
     *  -----------------------------ROOM -------------------------
     *  WatingRoom 씬에서 플레이어를 받습니다.
     *  만약에 서버를 키게 되면 서버는 방을 만듭니다.
     *  그리고 클라이언트가 접속하면 클라이언트가 방에 처음들어온 대상일 경우  Leader를 부여합니다.
     *  Leader가 방에서 나가면 다른 플레이어로 Leader가 교체됩니다.
     *  
     *  UI 표시에 있어서 
     *  WatingRoomScene에 PlayerSpace 오브젝트가 있는데, 유저(클라이언트)가 들어오면
     *  PlayerSpace 오브젝트의 UI를 RoomPlayerPrefab의 UI가 대체하는 방식입니다.
     *  
     *  방장에게만 StartButton이 표시가 되고 
     *  모두가 Ready인 상태여야만 StartButton이 표시됩니다.
     *  
     *  게임을 Start하게 될 경우 게임 씬으로 넘어가게 됩니다.
     *  
     *  WaitingRoom_MultiGameManager.cs 가 NetManager를 이용하여
     *  WatingRoom Scene내의 오브젝트들을 처리하게 됩니다.
     *  
     *  또한 Lobby 폴더에 있는 다른 스크립트들도 네트워크 과정이 필요할경우
     *  처리합니다.
     *  
     */

    //플레이어가 생성될때 플레이어 객체에 닉네임등 스타트씬에서 입력된 정보를 입력
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        NetRoomPlayer newPlayer = (NetRoomPlayer)Instantiate(roomPlayerPrefab);
        newPlayer.nickname = PlayerPrefs.GetString("nickname");
        if (roomSlots.Count == 0) newPlayer.isLeader = true;

        return newPlayer.gameObject;
    }
    // Client가 Room에서 나갈때
    //그 Client의 RoomPlayerPrefab의 UI를 PlayerSpace의 UI가 대체함
    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        
        if (conn.identity != null && IsSceneActive(RoomScene))
        {
            NetRoomPlayer disconnectedPlayer = conn.identity.GetComponent<NetRoomPlayer>();
            //disconnectedPlayer.playerSpace.SetActive(true); // UI 대체

        }
        
    }
    


    // 모든 유저가 준비됐으면 StartButton준비
    public override void OnRoomServerPlayersReady()
    {
        foreach (NetRoomPlayer player in roomSlots)
        {
            if (player.isLeader)
            {
                player.AcivateStartButton();
                break;
            }
        }
    }

    //준비 안됐으면 StartButton 미준비
    public override void OnRoomServerPlayersNotReady()
    {
        foreach (NetRoomPlayer player in roomSlots)
        {
            if (player.isLeader)
            {
                player.DeActivateStartButton();
                break;
            }
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
    }
    
    

}
