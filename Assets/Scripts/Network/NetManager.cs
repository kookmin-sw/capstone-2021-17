using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Serialization;
using Mirror;

using SceneMg = UnityEngine.SceneManagement;
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

    public string PlayerName;
    private string clientPlayerName;

    [SerializeField]
    [FormerlySerializedAs("LoadingManagerPrefab")]
    private GameObject loadingManagerPrefab;

    private GameMgr inGameMgr;
    private EnemySpawnManager enemySpawnManager;

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
     * netManager.StartClient() , netManager.StartHost()
     * 
     * Start_MultiGameManger.cs 파일 한번 참고해주시면 좋을것같습니다.
     * 참고로 Start_MultiGameManger.cs 스크립트는 MainManager 오브젝트에 달아놨습니다.
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

    public struct CreateRoomPlayerMessage : NetworkMessage
    {
        public string name;
    }
    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
        NetworkServer.RegisterHandler<CreateRoomPlayerMessage>(OnCreatePlayer);
    }

    void OnCreatePlayer(NetworkConnection conn, CreateRoomPlayerMessage msg)
    {
        clientPlayerName = msg.name;
    }

    public override void OnRoomClientConnect(NetworkConnection conn)
    {
        base.OnRoomClientConnect(conn);
        conn.Send(new CreateRoomPlayerMessage { name = PlayerName });
    }



    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        
        NetRoomPlayer newPlayer = (NetRoomPlayer)Instantiate(roomPlayerPrefab);
        newPlayer.Nickname = clientPlayerName;
        if (roomSlots.Count == 0) newPlayer.IsLeader = true;

        return newPlayer.gameObject;
    }
    


    // 모든 유저가 준비됐으면 StartButton준비
    public override void OnRoomServerPlayersReady()
    {
        foreach (NetRoomPlayer player in roomSlots)
        {
            if (player.IsLeader)
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
            if (player.IsLeader)
            {
                player.DeActivateStartButton();
                break;
            }
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        Transform startPos = GetStartPosition();
        GameObject gamePlayer = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        NetGamePlayer netGamePlayer = gamePlayer.GetComponent<NetGamePlayer>();
        NetRoomPlayer netRoomPlayer = roomPlayer.GetComponent<NetRoomPlayer>();
        netGamePlayer.Nickname = netRoomPlayer.Nickname;
        netGamePlayer.isLeader = netRoomPlayer.IsLeader;

        return netGamePlayer.gameObject;
    }


    public override void ServerChangeScene(string newSceneName)
    {
        if (newSceneName == RoomScene)
        {
            OnReturnToRoom();
        }

        if (newSceneName == GameplayScene)
        {
            OnChangeGamePlayScene();
        }

        NetworkServer.SetAllClientsNotReady();

        networkSceneName = newSceneName;

        OnServerChangeScene(newSceneName);

        Transport.activeTransport.enabled = false;

        

        LoadScene(newSceneName);

        // ServerChangeScene can be called when stopping the server
        // when this happens the server is not active so does not need to tell clients about the change
        if (NetworkServer.active)
        {
            // notify all clients about the new scene
            NetworkServer.SendToAll(new SceneMessage
            {
                sceneName = newSceneName,
                customHandling = true
            });
        }

        startPositionIndex = 0;
        startPositions.Clear();
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        if (newSceneName == GameplayScene)
        {
            OnChangeGamePlayScene();
        }
        startPositionIndex = 0;
        startPositions.Clear();
        //base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        LoadScene(newSceneName);

    }
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if(sceneName == GameplayScene)
        {
            inGameMgr = GameMgr.instance;
            inGameMgr.Init();

            enemySpawnManager = EnemySpawnManager.instance;
            enemySpawnManager.Init();
        }
    }
    void LoadScene(string newSceneName)
    {
        LoadingManager loadingManager = Instantiate(loadingManagerPrefab).GetComponent<LoadingManager>();

        loadingSceneAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName);

        loadingManager.SetAsyncOperation(loadingSceneAsync);

        loadingManager.StartLoading();
    }
    void OnReturnToRoom()
    {
        foreach (NetworkRoomPlayer roomPlayer in roomSlots)
        {
            if (roomPlayer == null)
                continue;

            // find the game-player object for this connection, and destroy it
            NetworkIdentity identity = roomPlayer.GetComponent<NetworkIdentity>();

            if (NetworkServer.active)
            {
                // re-add the room object
                roomPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin = false;
                NetworkServer.ReplacePlayerForConnection(identity.connectionToClient, roomPlayer.gameObject);
                
            }
        }

        allPlayersReady = false;
    }

    private void OnChangeGamePlayScene()
    {
        foreach (NetRoomPlayer roomPlayer in roomSlots)
        {
            if (roomPlayer.gameObject != null)
            {
                roomPlayer.Rect_Trans.gameObject.SetActive(false);
            }
        }
    }


}
