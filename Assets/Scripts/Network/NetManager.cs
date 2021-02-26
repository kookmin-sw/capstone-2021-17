using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NetManager : NetworkRoomManager
{
    public static int PLAYER_MAXNUM = 4;

    public static string serverAddress;
    public static NetManager instance;

    public TMP_InputField nameField;

    public List<NetRoomPlayer> RoomPlayers { get; } = new List<NetRoomPlayer>();

    
    public GameObject startBtn;


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


    
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        NetRoomPlayer newPlayer = (NetRoomPlayer)Instantiate(roomPlayerPrefab);
        newPlayer.nickname = PlayerPrefs.GetString("nickname");
        Debug.Log(newPlayer.nickname + "," + nameField.text);


        return newPlayer.gameObject;
    }

    
    public void SaveNickName()
    {
        PlayerPrefs.SetString("nickname", nameField.text);
    }

    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        NetRoomPlayer disconnectedPlayer = conn.identity.GetComponent<NetRoomPlayer>();
        disconnectedPlayer.playerSpace.SetActive(true);
        foreach (NetRoomPlayer player in roomSlots)
        {
            player.UpdateUI();
        }
    }


    public override void OnRoomServerPlayersReady()
    {
        startBtn.SetActive(true);
        
    }
    public override void OnRoomServerPlayersNotReady()
    {
        startBtn.SetActive(false);
    }





}
