using MasterServerToolkit.MasterServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetServersList : MonoBehaviour
{
    private RoomController roomController;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        ClientToMasterConnector.Instance.Connection.AddConnectionListener(OnConnectedToMasterServerHandler, true);
        ClientToMasterConnector.Instance.Connection.AddDisconnectionListener(OnDisconnectedFromMasterServerHandler, false);
    }

    private void OnConnectedToMasterServerHandler()
    {
        RoomOptions options = new RoomOptions
        {
            IsPublic = true,
            // Your Game Server Name
            Name = "My Game With Friends",
            // If you want your server to be passworded
            Password = "",
            // Machine IP the server is running on
            RoomIp = "127.0.0.1",
            // Port of the game server
            RoomPort = 7777,
            // The max number of connections
            MaxConnections = 10
        };

        Mst.Server.Rooms.RegisterRoom(options, (controller, error) =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
                return;
            }

            roomController = controller;

            Debug.Log("Our server was successfully registered");
        });
    }

    private void OnDisconnectedFromMasterServerHandler()
    {
        Mst.Server.Rooms.DestroyRoom(roomController.RoomId, (isSuccess, error) =>
        {
            // Your code here...
        });
    }
}


