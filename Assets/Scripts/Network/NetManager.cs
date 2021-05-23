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
/*
*   https://mirror-networking.gitbook.io/docs/ 읽어주시면 감사합니다
*/   


public class NetManager : NetworkRoomManager
{
    public static int PLAYER_MAXNUM = 4;

    public static NetManager instance;

    public string RoomName;

    public string PlayerName;
    private string clientPlayerName;

    [Scene]
    public string endingScene;

    [SerializeField]
    [FormerlySerializedAs("LoadingManagerPrefab")]
    private GameObject loadingManagerPrefab;

    public LoadingManager loadingManager;


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

        base.Awake();
    }
    #region RoomServer Callbacks
    public override void OnRoomStartServer()
    {
        base.OnRoomStartServer();
        NetworkServer.RegisterHandler<CreateRoomPlayerMessage>(CreateRoomPlayerRequestHandler);
    }

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

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {

        NetRoomPlayer newPlayer = (NetRoomPlayer)Instantiate(roomPlayerPrefab);
        newPlayer.Nickname = clientPlayerName;
        if (roomSlots.Count == 0) newPlayer.IsLeader = true;

        return newPlayer.gameObject;
    }

    public override void OnRoomStartClient()
    {
        OnClientStartedEvent?.Invoke();
        NetworkClient.RegisterHandler<CreateGamePlayerMessage>(CreateGamePlayerMessageClientHandler);
    }
    


    /// <summary>
    /// Invokes when client requested to create player on mirror server
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    protected virtual void CreateRoomPlayerRequestHandler(NetworkConnection connection, CreateRoomPlayerMessage message)
    {

        clientPlayerName = message.name;
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
    #endregion

    #region RoomClient CallBacks
    public override void OnRoomClientConnect(NetworkConnection conn)
    {
        base.OnRoomClientConnect(conn);
        //PlayerName = PlayerNameSave.instance.PlayerName;
        PlayerName = PlayerPrefs.GetString("PlayerName");
        conn.Send(new CreateRoomPlayerMessage { name = PlayerName });
    }

    #endregion

    #region GameServer Callbacks
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

    public struct WaitingGamePlayer
    {
        public NetworkConnection conn;
        public GameObject GamePlayer;
    }

    private List<WaitingGamePlayer> WaitingGamePlayers = new List<WaitingGamePlayer>();

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        WaitingGamePlayers.Add(new WaitingGamePlayer { conn = conn, GamePlayer = gamePlayer });
        if (CheckOtherPlayers())
        {
            SpawnGameObjects();
        }

        
        return false;
    }

    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        OnClientDisconnectedEvent?.Invoke(conn);
        if (IsSceneActive(GameplayScene) && CheckOtherPlayers())
        {
            SpawnGameObjects();
        }
    }

    bool CheckOtherPlayers()
    {
        if (WaitingGamePlayers.Count == roomSlots.Count)
        {
            return true;
        }
        return false;
    }

    void SpawnGameObjects()
    {

        inGameMgr = GameMgr.instance;
        inGameMgr.Init();

        enemySpawnManager = EnemySpawnManager.instance;
        enemySpawnManager.Init();


        foreach (WaitingGamePlayer player in WaitingGamePlayers)
        {
            NetworkServer.ReplacePlayerForConnection(player.conn, player.GamePlayer, true);
            player.conn.Send(new CreateGamePlayerMessage());
        }
    }

    #endregion

    #region GameClient Callbacks
    

    public override void OnRoomClientSceneChanged(NetworkConnection conn)
    {
        if (IsSceneActive(GameplayScene))
        {
            loadingManager = Instantiate(loadingManagerPrefab).GetComponent<LoadingManager>();
        }
    }

    public void CreateGamePlayerMessageClientHandler(CreateGamePlayerMessage msg)
    {
        loadingManager.gameObject.SetActive(false);
    }
    #endregion

    #region Change RoomScene To GamePlayScene
    public override void OnServerChangeScene(string newSceneName)
    {
        if (newSceneName == GameplayScene)
        {
            OnChangeGamePlayScene();
        }
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

    private void OnChangeGamePlayScene()
    {

        Mst.Events.Invoke(MstEventKeys.showLoadingInfo, "Loading Client Scene...");

        foreach (NetRoomPlayer roomPlayer in roomSlots)
        {
            if (roomPlayer.gameObject != null)
            {
                roomPlayer.Rect_Trans.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region CALLBACK WITH MASTER ACTIONS


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
        WaitingGamePlayers.Clear();
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void OnRoomClientDisconnect(NetworkConnection conn)
    {
        OnDisconnectedEvent?.Invoke(conn);
    }

    #endregion

    
}



