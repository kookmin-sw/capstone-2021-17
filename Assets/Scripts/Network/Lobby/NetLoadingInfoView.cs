using UnityEditor;
using UnityEngine;
using MasterServerToolkit.Games;
using MasterServerToolkit.Networking;
using MasterServerToolkit.MasterServer;

public class NetLoadingInfoView : LoadingInfoView
{
    public IClientSocket Connection => Mst.Connection;

    protected virtual void Start()
    {
        Connection.OnStatusChangedEvent += OnStatusChangedEventHandler;
        OnStatusChangedEventHandler(Connection.Status);
    }

    protected virtual void OnStatusChangedEventHandler(ConnectionStatus status)
    {
        switch (status)
        {
            case ConnectionStatus.Connected:
                SetLables("Client is connected");
                break;
            case ConnectionStatus.Disconnected:
                SetLables("Client is offline");
                break;
            case ConnectionStatus.Connecting:
                SetLables("Client is connecting");
                break;
            default:
                SetLables("Unknown status");
                break;
        }
    }
}
