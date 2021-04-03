using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;

public class NetRoomPlayer : NetworkRoomPlayer
{

    [SyncVar]
    public string Nickname;

    [SyncVar]
    public bool IsLeader;

    public RectTransform Rect_Trans;

    public string NotReady_msg = "Not Ready";
    public string Ready_msg = "Ready";

    [SerializeField]
    private TMP_Text Nickname_txt; // 에디터 내에서 지정
    [SerializeField]
    private Image Profile_image; // 에디터 내에서 지정
    [SerializeField]
    private TMP_Text Readystatus_txt; // 에디터 내에서 지정

    

    private NetManager netManager;
    WaitingRoom_MultiGameManager gameManager;

    public bool isJoinRoom = false;

    private void Awake()
    {
        netManager = NetManager.instance;
        gameManager = WaitingRoom_MultiGameManager.instance;
    }


    public override void OnClientEnterRoom() // 다른 클라이언트가 들어와도 OnClientEnterRoom은 불림
    {
        if (isJoinRoom) // Join Room 한 상태라면 바꾸지 않게끔
        {
            return;
        }
        setNicknameText();
        setReadyText();

        gameManager = WaitingRoom_MultiGameManager.instance;
        
        gameManager.AddPlayerToPlayerSpace(this);

        if (this.gameObject != null)
        {
            Rect_Trans.gameObject.SetActive(true);
        }

        isJoinRoom = true;
    }


    public override void ReadyStateChanged(bool _, bool newReadyState) // [Syncvar] readyToBegin hook
    {
        base.ReadyStateChanged(_, newReadyState);

        setReadyText();       
    }
    public override void OnStopClient()
    {
        if (gameManager != null)
        {
            gameManager.RemovePlayerFromPlayerSpace(this);
        }
        if (this.IsLeader)
        {
            ReplaceLeader(this);
        }
    }
    public override void OnClientExitRoom()
    {
        if (NetManager.IsSceneActive(netManager.GameplayScene))
        {
            isJoinRoom = false;
        }
    }

    public void ReplaceLeader(NetRoomPlayer roomPlayer)
    {
        foreach (NetRoomPlayer player in netManager.roomSlots)
        {
            if (player != roomPlayer && !player.IsLeader)
            {
                player.IsLeader = true;
                player.setNicknameText();
                
                break;
            }
        }
    }

    [TargetRpc]
    public void AcivateStartButton()
    {
        if(gameManager == null)
        {
            gameManager = WaitingRoom_MultiGameManager.instance;
        }
        gameManager.ActivateStartButton();
    }

    [TargetRpc]
    public void DeActivateStartButton()
    {
        if (gameManager == null)
        {
            gameManager = WaitingRoom_MultiGameManager.instance;
        }
        gameManager.DeActivateStartButton();
    }

    [Command]
    public void StartGame()
    {
        netManager.ServerChangeScene(netManager.GameplayScene);
    }

    
    public void setReadyText()
    {
        if (readyToBegin)
        {
            Readystatus_txt.text = Ready_msg;
        }
        else
        {
            Readystatus_txt.text = NotReady_msg;
        }
    }
    public void setNicknameText()
    {  
        if (!IsLeader)
        {
            Nickname_txt.text = Nickname;
        }
        else // if(isLeader)
        {
            Nickname_txt.text = Nickname + "\n <LEADER>";
        }
    }


}
