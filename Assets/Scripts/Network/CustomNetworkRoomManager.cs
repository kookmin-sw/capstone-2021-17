using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    private string serverName;
    private string userName;
    private static CustomNetworkRoomManager instance;

    public override void Awake() 
    {
        base.Awake();

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

    }


    public override void OnRoomStartHost()
    {
        SetNicknameAtRoomPlayer();
    }
    public override void OnRoomClientEnter()
    {
        SetNicknameAtRoomPlayer();
    }

}
