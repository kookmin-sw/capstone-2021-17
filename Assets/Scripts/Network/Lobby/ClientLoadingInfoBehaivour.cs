using UnityEditor;
using UnityEngine;
using MasterServerToolkit.Logging;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.UI;
using MasterServerToolkit.Games;


public class ClientLoadingInfoBehaivour : BaseClientBehaviour
{

    public ClientToMasterConnector clientToMasterConnector;

    public ClientToMasterConnector Connector
    {
        get
        {
            if (!clientToMasterConnector)
                clientToMasterConnector = FindObjectOfType<ClientToMasterConnector>();

            return clientToMasterConnector;
        }
    }

    public void Initialize()
    {
        OnInitialize();
    }
    protected override void OnInitialize()
    {
        Mst.Events.Invoke(MstEventKeys.showLoadingInfo, "Connecting to server... Please wait!");

        MstTimer.WaitForSeconds(0.5f, () =>
        {
            // Listen to connection events
            Connection.AddConnectionListener(OnClientConnectedToServer);
            Connection.AddDisconnectionListener(OnClientDisconnectedFromServer, false);

            if (!Connection.IsConnected && !Connection.IsConnecting)
            {
                Connector.StartConnection();
            }
        });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // unregister from connection events
        Connection?.RemoveConnectionListener(OnClientConnectedToServer);
        Connection?.RemoveDisconnectionListener(OnClientDisconnectedFromServer);
    }

    void OnClientConnectedToServer()
    {
        Mst.Events.Invoke(MstEventKeys.hideLoadingInfo);
    }

    void OnClientDisconnectedFromServer()
    {
        Connection.RemoveConnectionListener(OnClientConnectedToServer);
        Connection.RemoveDisconnectionListener(OnClientDisconnectedFromServer);

    }
}
