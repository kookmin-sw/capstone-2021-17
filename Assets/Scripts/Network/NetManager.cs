using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;

public class NetManager : NetworkRoomManager
{
    public static int PLAYER_MAXNUM = 4;

    public static NetManager instance;

    public List<NetRoomPlayer> RoomPlayers { get; } = new List<NetRoomPlayer>();

    
    public GameObject startBtn;

    // Awake()할시 기존에 instance를 지우고 새로운 instance로 받을 수 있도록함 (StartScene)

    public override void Awake() 
    {

        if (instance == null && this.dontDestroyOnLoad) 
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(instance.gameObject);
                instance = this; 
            }
        }
    }


    /*
     *  -----------------------------ROOM -------------------------
     *  NetRoomPlayer.cs를 참고하자면
     *  PlayerSpace 라는 게임오브젝트를 Find 한뒤에 
     *  GameObject tryFind = GameObject.Find(playerSpaceObjectName + index);
     *  
     *  PlayerSpace의 UI를 RoomPlayerPrefab의 UI가 대체하는 방식임.
     *  
     */

    //플레이어가 생성될때 플레이어 객체에 닉네임등 스타트씬에서 입력된 정보를 입력
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        NetRoomPlayer newPlayer = (NetRoomPlayer)Instantiate(roomPlayerPrefab);
        newPlayer.nickname = PlayerPrefs.GetString("nickname");

        return newPlayer.gameObject;
    }
    // Client가 Room에서 나갈때
    //그 Client의 RoomPlayerPrefab의 UI를 PlayerSpace의 UI가 대체함
    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        
        if (conn.identity != null)
        {
            NetRoomPlayer disconnectedPlayer = conn.identity.GetComponent<NetRoomPlayer>();
            disconnectedPlayer.playerSpace.SetActive(true); // UI 대체

        }
        
    }

    // 모든 유저가 준비됐으면 StartButton준비
    public override void OnRoomServerPlayersReady()
    {
        startBtn.SetActive(true);
        
    }
    //준비 안됐으면 StartButton 미준비
    public override void OnRoomServerPlayersNotReady()
    {
        startBtn.SetActive(false);
    }

}
