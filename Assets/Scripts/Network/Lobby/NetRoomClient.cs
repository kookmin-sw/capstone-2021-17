using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MasterServerToolkit.Bridges.MirrorNetworking;

using MS = MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;

public class NetRoomClient : RoomClient
{
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        NetworkClient.UnregisterHandler<ValidateRoomAccessResultMessage>();

        // Stop listening to OnServerStartedEvent of our MirrorNetworkManager
        if (NetworkManager.singleton is NetManager manager)
        {
            manager.OnClientStartedEvent -= OnMirrorClientStartedEventHandler;
        }

        // Remove master server connection and disconnection listener
        Connection?.RemoveDisconnectionListener(OnDisconnectedFromMasterServerEventHandler);
    }

    protected override void OnInitialize()
    {
        // If we hav offline scene in global options
        if (MS.Mst.Options.Has(MS.MstDictKeys.ROOM_OFFLINE_SCENE_NAME))
        {
            logger.Debug("Assigning offline scene to mirror network manager");
            RoomNetworkManager.offlineScene = MS.Mst.Options.AsString(MS.MstDictKeys.ROOM_OFFLINE_SCENE_NAME);
        }

        // Start listening to OnServerStartedEvent of our MirrorNetworkManager
        if (NetworkManager.singleton is NetManager manager)
        {
            manager.OnClientStartedEvent += OnMirrorClientStartedEventHandler;
            manager.OnClientStoppedEvent += OnMirrorClientStoppedEventHandler;
        }
        else
        {
            logger.Error("Before using MirrorNetworkManager add it to scene");
        }

        // Add master server connection and disconnection listeners
        Connection.AddDisconnectionListener(OnDisconnectedFromMasterServerEventHandler, false);

        MstTimer.WaitForSeconds(0.5f, () =>
        {
            // If connection to master server is not established
            if (!Connection.IsConnected && !Connection.IsConnecting)
            {
                Connection.UseSsl = MS.MstApplicationConfig.Instance.UseSecure;
                Connection.Connect(masterIp, masterPort);
            }
        });
    }

    public override void CreatePlayer()
    {
        //NetworkClient.Send(new CreateRoomPlayerMessage());
    }

    protected override void JoinTheRoom()
    {
        if(roomAccess.SceneName != RoomNetworkManager.onlineScene)
        {
            Debug.Log("Room Game is Already Started!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(MS.Mst.Options.AsString(MS.MstDictKeys.ROOM_OFFLINE_SCENE_NAME));
            return;
        }
        base.JoinTheRoom();
    }
}
