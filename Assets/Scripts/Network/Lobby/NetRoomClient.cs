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
        
        string[] fileSplit = NetManager.instance.GameplayScene.Split('/');
        string[] fileNameSplit = fileSplit[fileSplit.Length -1 ].Split('.');
       
        string GameplaySceneName = fileNameSplit[0];

        if (roomAccess.SceneName == GameplaySceneName)
        {
            Debug.Log("Room Game is Already Started!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(MS.Mst.Options.AsString(MS.MstDictKeys.ROOM_OFFLINE_SCENE_NAME));
            return;
        }
        base.JoinTheRoom();
    }

    public override void SetPort(int port)
    {
        if (Transport.activeTransport is kcp2k.KcpTransport kcp)
        {
            kcp.Port = (ushort)port;
        }
        else if (Transport.activeTransport is TelepathyTransport tcp)
        {
            tcp.port = (ushort)port;
        }
        else
        {
            Debug.LogError("Transport is not found");
        }
    }

    public override int GetPort()
    {
        if (Transport.activeTransport is kcp2k.KcpTransport kcp)
        {
            return kcp.Port;
        }
        else if (Transport.activeTransport is TelepathyTransport tcp)
        {
            return tcp.port;
        }
        else
        {
            Debug.LogError("Transport is not found");
            return 0;
        }
    }
}
