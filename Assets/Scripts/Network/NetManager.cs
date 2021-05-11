using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Serialization;
using Mirror;
using MasterServerToolkit.Logging;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Utils;
using UnityEngine.SceneManagement;
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

    public string RoomName;

    public string PlayerName;
    private string clientPlayerName;

    public List<EndingPlayerMessage> EndingMessages;

    [Scene]
    public string endingScene;

    [SerializeField]
    [FormerlySerializedAs("LoadingManagerPrefab")]
    private GameObject loadingManagerPrefab;

    private GameMgr inGameMgr;
    private EnemySpawnManager enemySpawnManager;

    //singleton
    public override void Awake() 
    {
        logger = Mst.Create.Logger(GetType().Name);
        logger.LogLevel = logLevel;

        // Prevent to create player automatically
        autoCreatePlayer = false;

        // Prevent start network manager in headless mode automatically
        autoStartServerBuild = false;

        if(instance == null)
        {
            instance = this;
        }

        EndingMessages = new List<EndingPlayerMessage>();

        base.Awake();
    }

    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
        NetworkServer.RegisterHandler<CreateRoomPlayerMessage>(CreateRoomPlayerRequestHandler);
        NetworkServer.RegisterHandler<EndingPlayerMessage>(EndingPlayerMessageServerHandler);
    }

    public override void OnRoomStartClient()
    {
        OnClientStartedEvent?.Invoke();

        NetworkClient.RegisterHandler<EndingPlayerMessage>(EndingPlayerMessageClientHandler);
    }

    void EndingPlayerMessageServerHandler(NetworkConnection conn , EndingPlayerMessage message)
    {
        NetworkServer.SendToAll(message);

        SceneMessage sceneMsg = new SceneMessage
        {
            sceneName = endingScene,
        };
        conn.Send(sceneMsg);
        
    }

    void EndingPlayerMessageClientHandler(EndingPlayerMessage message)
    {

        EndingMessages.Add(message);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().path == endingScene)
        {
            EndingManager.instance.UpdatePlayers();
        }
    }


    /// <summary>
    /// Invokes when client requested to create player on mirror server
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    protected virtual void CreateRoomPlayerRequestHandler(NetworkConnection connection, CreateRoomPlayerMessage message)
    {

        clientPlayerName = message.name;
        Debug.Log(clientPlayerName);

        Debug.Log("CREATE ROOMPLAYER REQUEST HANDLER");
        // Try to get old player
        GameObject oldPlayer = null;

        if (connection.identity != null)
        {
            oldPlayer = connection.identity.gameObject;
        }
        

        // Create new player
        GameObject player = OnRoomServerCreateRoomPlayer(connection);

        if (oldPlayer)
        {
            NetworkServer.ReplacePlayerForConnection(connection, player);
        }
        else
        {
            NetworkServer.AddPlayerForConnection(connection, player);
        }

    }


    public override void OnRoomClientConnect(NetworkConnection conn)
    {
        base.OnRoomClientConnect(conn);
        //PlayerName = PlayerNameSave.instance.PlayerName;
        PlayerName = PlayerPrefs.GetString("PlayerName");
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

        loadingSceneAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName);

        // ServerChangeScene can be called when stopping the server
        // when this happens the server is not active so does not need to tell clients about the change
        if (NetworkServer.active)
        {
            // notify all clients about the new scene
            NetworkServer.SendToAll(new SceneMessage
            {
                sceneName = newSceneName,
                customHandling = false
            });
        }
        

        startPositionIndex = 0;
        startPositions.Clear();
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

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {        
        if (newSceneName == GameplayScene)
        {
            OnChangeGamePlayScene();
        }
        startPositionIndex = 0;
        startPositions.Clear();     
        //base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        //LoadScene(newSceneName);

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

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {

        return true;
    }
    public override void OnClientLoadScene()
    {
        
        LoadingManager loadingManager = Instantiate(loadingManagerPrefab).GetComponent<LoadingManager>();
        loadingManager.SetAsyncOperation(loadingSceneAsync);
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

    


    [Header("Mirror Network Manager Settings"), SerializeField]
    private HelpBox help = new HelpBox()
    {
        Text = "This is extension of NetworkManager",
        Type = HelpBoxType.Info
    };

    /// <summary>
    /// Log levelof this module
    /// </summary>
    [SerializeField]
    protected LogLevel logLevel = LogLevel.Info;

    /// <summary>
    /// Logger assigned to this module
    /// </summary>
    protected MasterServerToolkit.Logging.Logger logger;

    /// <summary>
    /// Invokes when mirror server is started
    /// </summary>
    public event Action OnServerStartedEvent;

    /// <summary>
    /// Invokes when mirror server is stopped
    /// </summary>
    public event Action OnServerStoppedEvent;

    /// <summary>
    /// Invokes when mirror host is started
    /// </summary>
    public event Action OnHostStartedEvent;

    /// <summary>
    /// Invokes when mirror host is stopped
    /// </summary>
    public event Action OnHostStopEvent;

    /// <summary>
    /// Invokes when mirror client is started
    /// </summary>
    public event Action OnClientStartedEvent;

    /// <summary>
    /// Invokes when mirror client is stopped
    /// </summary>
    public event Action OnClientStoppedEvent;

    /// <summary>
    /// This is called on the Server when a Client disconnects from the Server
    /// </summary>
    public event Action<NetworkConnection> OnClientDisconnectedEvent;

    /// <summary>
    /// Called on clients when connected to a server
    /// </summary>
    public event Action<NetworkConnection> OnConnectedEvent;

    /// <summary>
    /// Called on clients when disconnected from a server
    /// </summary>
    public event Action<NetworkConnection> OnDisconnectedEvent;


    #region MIRROR CALLBACKS

    /// <summary>
    /// When mirror server is started
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Register handler to listen to player creation message
       // NetworkServer.RegisterHandler<CreateRoomPlayerMessage>(CreateRoomPlayerRequestHandler, false);
        OnServerStartedEvent?.Invoke();
    }

    public override void OnRoomStopServer()
    {
        OnServerStoppedEvent?.Invoke();
    }

    public override void OnRoomStartHost()
    {
        OnHostStartedEvent?.Invoke();
    }

    public override void OnRoomStopHost()
    {
        OnHostStopEvent?.Invoke();
    }

    
    public override void OnRoomStopClient()
    {
        OnClientStoppedEvent?.Invoke();
    }

    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {

        OnClientDisconnectedEvent?.Invoke(conn);
    }



    public override void OnRoomClientDisconnect(NetworkConnection conn)
    {
        OnDisconnectedEvent?.Invoke(conn);
    }

    #endregion

    
}



