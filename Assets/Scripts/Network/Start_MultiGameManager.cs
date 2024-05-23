﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    [SerializeField]
    private SceneManager sceneManager;
    private void Start() // Awake할때는 instance 못부름
    {
        netManager = NetManager.instance;

        if (Mst.Server.IsConnected)
        {
            OnServerAlreadyConnected.Invoke();
        }

        nameField.onValueChanged.AddListener(delegate { SetPlayerNameValid(); });
        
        roomNameField.onValueChanged.AddListener(delegate { SetRoomNameValid(); });

        ClientToMasterConnector.Instance.OnConnectedEvent.AddListener(sceneManager.ShowMainMenu);
        ClientToMasterConnector.Instance.OnDisconnectedEvent.AddListener(sceneManager.ShowServerFail);

        if(NetClientToMasterConnector.Instance is NetClientToMasterConnector connector)
        {
            if (connector.isDisconnected) sceneManager.ShowServerFail(); 
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
    public UnityEvent OnPlayerNameError;

    public void  JoinDedicatedRoom()
    {
        if(nameField.text.Length ==0 || nameField.text.Length > 10)
        {
            OnPlayerNameError.Invoke();
            return;
        }

        if (ClientToMasterConnector.Instance.Connection.IsConnected)
        {
            SetPlayerName();
            OnRoomJoin?.Invoke();
            sceneManager.ShowLoadingInfo();
        }
        else
        {
            OnServerCantConnect?.Invoke();
        }
        
        //ClientToMasterConnector.Instance.Connection.AddConnectionListener(OnConnectedToMasterServerHandler, true);
    }

    public void CreateDedicatedRoom()
    {
        if (nameField.text.Length == 0 || nameField.text.Length > 10)
        {
            OnPlayerNameError.Invoke();
            return;
        }
        if (ClientToMasterConnector.Instance.Connection.IsConnected)
        {
            SetPlayerName();
            OnRoomCreate?.Invoke();
            sceneManager.ShowLoadingInfo();
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
        //Instantiate(Resources.Load("PlayerNameSave"));

        //PlayerNameSave.instance.PlayerName = nameField.text;

        PlayerPrefs.SetString("PlayerName", nameField.text);
    }


    public void OnAlreadyServerConnected()
    {
        OnServerAlreadyConnected.Invoke();
    }

    public void ReconnectServer()
    {
        var loadingInfo =  FindObjectOfType<ClientLoadingInfoBehaivour>();
        loadingInfo.Initialize();
    }

    void SetRoomNameValid()
    {
        string roomNameText = roomNameField.text;
        string upperText = roomNameText.ToUpper();
        
        
        if(upperText.Length > 4)
        {
            roomNameField.text = upperText.Substring(0, 4);
        }
        else
        {
            roomNameField.text = upperText;
        }
    }

    void SetPlayerNameValid()
    {
        string playerNameText = nameField.text;
        string OnlyAsciiText = "";

        foreach(char c in playerNameText)
        {
            if(33<=c && c <= 126)
            {
                OnlyAsciiText += c;
            }
        }

        if (playerNameText.Length > 10)
        {
            nameField.text = OnlyAsciiText.Substring(0, 10);
        }
        else
        {
            nameField.text = OnlyAsciiText;
        }

    }

    
}
