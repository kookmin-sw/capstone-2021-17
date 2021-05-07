using UnityEditor;
using UnityEngine;
using MasterServerToolkit.Games;
using MasterServerToolkit.Networking;
using MasterServerToolkit.MasterServer;

public class NetLoadingInfoView : LoadingInfoView
{
    public IClientSocket Connection => Mst.Connection;

    protected void Start()
    {
        Connection.OnStatusChangedEvent += OnStatusChangedEventHandler;
        OnStatusChangedEventHandler(Connection.Status);
    }

    protected virtual void OnStatusChangedEventHandler(ConnectionStatus status)
    {
        switch (status)
        {
            case ConnectionStatus.Connected:
                SetLables("Connected To Server");
                Owner.Show();
                break;
            case ConnectionStatus.Disconnected:
                SetLables("Start Connecting Server");
                Owner.Show();
                break;
            case ConnectionStatus.Connecting:
                SetLables("Connecting To Server...");
                Owner.Show();
                break;
            default:
                SetLables("Unknown status");
                Owner.Show();
                break;
        }
    }
}
