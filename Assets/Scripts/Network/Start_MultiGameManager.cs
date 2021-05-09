using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterServerToolkit.MasterServer;
using TMPro;
using MasterServerToolkit.Networking;
using MasterServerToolkit.Games;
using UnityEngine.Events;
/*
* NetManager를 이용합니다
* StartScene에서의 GameObject를 다루는 Manager 객체입니다 
*/
public class Start_MultiGameManager: MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField roomNameField;

    private NetManager netManager;
    private void Start() // Awake할때는 instance 못부름
    {
        netManager = NetManager.instance;
        if (Application.isBatchMode)
        {
            Debug.Log("Server Starting! -- Heeun An");
        }
        {
            Debug.Log("Client Start");
        }

        if (Mst.Server.IsConnected)
        {
            OnServerAlreadyConnected.Invoke();
        }
    }

    public void SaveAddress()
    {
        string address = roomNameField.text;
        if (address == "") address = "localhost";
        netManager.networkAddress = address;
    }
    public void SaveNickName()
    {
        netManager.PlayerName = nameField.text;
    }


    //클라이언트로 시작
    public void JoinRoom()
    {
        SaveAddress();
        SaveNickName();


        netManager.StartClient();
    }

    //호스트로 시작 (추후에 방 만드는 기능이 생기면 변화가 필요함)
    public void CreateRoom()
    {
        SaveNickName();
        netManager.StartHost();
    }
    
    public UnityEvent OnRoomCreate;
    public UnityEvent OnRoomJoin;
    public UnityEvent OnServerCantConnect;
    public UnityEvent OnServerAlreadyConnected;

    public void  JoinDedicatedRoom()
    {
        if (ClientToMasterConnector.Instance.Connection.IsConnected)
        {
            SetPlayerName();
            OnRoomJoin?.Invoke();
        }
        else
        {
            OnServerCantConnect?.Invoke();
        }
        
        //ClientToMasterConnector.Instance.Connection.AddConnectionListener(OnConnectedToMasterServerHandler, true);
    }

    public void CreateDedicatedRoom()
    {
        if (ClientToMasterConnector.Instance.Connection.IsConnected)
        {
            SetPlayerName();
            OnRoomCreate?.Invoke();
        }
        else
        {
            OnServerCantConnect?.Invoke();
        }
    }

    public void StartMatchMaking()
    {
        if(MatchmakingBehaviour.Instance is NetMatchmakingBehaviour netMatchmaking)
        {
            netMatchmaking.StartMatchByName(roomNameField.text);
        }
        else
        {
            OnServerCantConnect?.Invoke();
        }
    }

    public void OnConnectedToMasterServer()
    {
        Mst.Events.Invoke(MstEventKeys.showLoadingInfo, "Connecting To Server...");
    }

    public void SetPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", nameField.text);
    }


    public void OnAlreadyServerConnected()
    {
        OnServerAlreadyConnected.Invoke();
    }

    
}
