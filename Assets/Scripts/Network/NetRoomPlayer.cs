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
    public string nickname;

    [SyncVar]
    public bool isLeader;

    public TMP_Text Nickname_txt; // 에디터 내에서 지정
    public Image Profile_image; // 에디터 내에서 지정
    public TMP_Text Readystatus_txt; // 에디터 내에서 지정

    public RectTransform Rect_Trans;

    public string NotReady_msg = "Not Ready";
    public string Ready_msg = "Ready";

    private NetManager netManager;
    WaitingRoom_MultiGameManager gameManager;

    private void Awake()
    {
        netManager = NetManager.instance;
        gameManager = WaitingRoom_MultiGameManager.instance;
    }

    public override void OnStartClient() //CallBack 함수
    {
        setNicknameText();
        setReadyText();
        gameManager.AddPlayerToPlayerSpace(this);

        
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        if (isLeader)
        {
            gameManager.AssignLeaderAuthority(this);
        }
               
    }

    public override void ReadyStateChanged(bool _, bool newReadyState) // [Syncvar] readyToBegin hook
    {
        base.ReadyStateChanged(_, newReadyState);

        setReadyText();       
    }
    public override void OnStopClient()
    {
        gameManager.RemovePlayerFromPlayerSpace(this);
        if (this.isLeader)
        {
            gameManager.ReplaceLeader(this);
        }
    }

    public override void OnClientExitRoom() // CallBack 함수
    {
        //GamePlay Scene으로 넘어갔을시 Roomplayer가 남아있던 문제를 해결
        if (this.gameObject != null && NetManager.IsSceneActive(netManager.GameplayScene))
        {
            Rect_Trans.gameObject.SetActive(false);
        }
    }

    [TargetRpc]
    public void AcivateStartButton()
    {
        gameManager.startButton.gameObject.SetActive(true);
    }

    [TargetRpc]
    public void DeActivateStartButton()
    {
        gameManager.startButton.gameObject.SetActive(false);
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
        if (!isLeader)
        {
            Nickname_txt.text = nickname;
        }
        else // if(isLeader)
        {
            Nickname_txt.text = nickname + "\n <LEADER>";
        }
    }
    
}
