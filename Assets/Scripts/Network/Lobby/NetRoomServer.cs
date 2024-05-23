using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterServerToolkit.Bridges.MirrorNetworking;
using MS = MasterServerToolkit.MasterServer;
using Mirror;

public class NetRoomServer : RoomServer
{
    protected override void OnDestroy()
    {
        base.OnDestroy();

        // Remove connection listeners
        Connection?.RemoveConnectionListener(OnConnectedToMasterServerEventHandler);
        Connection?.RemoveDisconnectionListener(OnDisconnectedFromMasterServerEventHandler);

        // Start listenin to OnServerStartedEvent of our MirrorNetworkManager
        if (RoomNetworkManager is NetManager manager)
        {
            manager.OnServerStartedEvent -= OnMirrorServerStartedEventHandler;
            manager.OnClientDisconnectedEvent -= OnMirrorClientDisconnectedEvent;
            manager.OnServerStoppedEvent -= OnMirrorServerStoppedEventHandler;
        }

        // Unregister handlers
        NetworkServer.UnregisterHandler<ValidateRoomAccessRequestMessage>();
    }
    protected override void OnInitialize()
    {
        if (MS.Mst.Client.Rooms.ForceClientMode) return;

        OnRoomServerRegisteredEvent += OnSetRoomName;

        // Start listening to OnServerStartedEvent of our MirrorNetworkManager
        if (RoomNetworkManager is NetManager manager)
        {
            manager.OnServerStartedEvent += OnMirrorServerStartedEventHandler;
            manager.OnClientDisconnectedEvent += OnMirrorClientDisconnectedEvent;
            manager.OnServerStoppedEvent += OnMirrorServerStoppedEventHandler;
        }
        else
        {
            logger.Error("We cannot register listeners of MirrorNetworkManager events because we cannot find it onscene");
        }

        // Set room oprions
        roomOptions = SetRoomOptions();

        // Set port of the Mirror server
        SetPort(roomOptions.RoomPort);

        // Add master server connection and disconnection listeners
        Connection.AddConnectionListener(OnConnectedToMasterServerEventHandler, true);
        Connection.AddDisconnectionListener(OnDisconnectedFromMasterServerEventHandler, false);

        // If connection to master server is not established
        if (!Connection.IsConnected && !Connection.IsConnecting)
        {
            Connection.UseSsl = MS.MstApplicationConfig.Instance.UseSecure;
            Connection.Connect(masterIp, masterPort);
        }
    }

    void OnSetRoomName()
    {
        NetManager.instance.RoomName = roomOptions.Name;
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
