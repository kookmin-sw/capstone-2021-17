using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    public static int PLAYER_MAXNUM = 4;

    public static string serverAddress;
    public static CustomNetworkRoomManager instance;
    private string player_nickname;

    public TMP_InputField nameField;

    public NetworkRoomUIManager RoomUIManager;

    public override void Awake() 
    {

        if (instance == null && this.dontDestroyOnLoad) // 이게 안먹힌경우
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(instance.gameObject);
                instance = this; // instance 시작 스크린으로 돌아올때 새걸로 교체
            }
        }
    }


    private void SetNicknameAtRoomPlayer()
    {
        roomPlayerPrefab.GetComponent<CustomNetworkRoomPlayer>().nickname = nameField.text;   
    }
    public override void OnRoomStartClient()
    {
        SetNicknameAtRoomPlayer();
    }

    public override void OnRoomStartServer()
    {
        SetNicknameAtRoomPlayer();
    }

    public override void OnRoomServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("On Room server ADD PLAYER ");
        RoomUIManager.UpdateRoomUI();
    }


    public override void OnRoomServerPlayersReady()
    {
        //start button appeared
    }
    public override void OnRoomServerPlayersNotReady()
    {
        //start button disappeared
    }





}
