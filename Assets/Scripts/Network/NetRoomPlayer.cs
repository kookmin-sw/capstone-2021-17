using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;

public class NetRoomPlayer : NetworkRoomPlayer
{
    /*public TMP_Text nickname;
    public Image profile_image;
    public TMP_Text readyTxt;*/

    [SyncVar]
    public string nickname;

    [SyncVar]
    public bool isLeader;

    public TMP_Text Nickname_txt; // 에디터 내에서 지정
    public Image Profile_image; // 에디터 내에서 지정
    public TMP_Text Readystatus_txt; // 에디터 내에서 지정

    public RectTransform Rect_Trans;

    public string playerSpaceObjectName = "PlayerSpace";

    public string NotReady_msg = "Not Ready";
    public string Ready_msg = "Ready";

    private NetManager netManager;
    WaitingRoom_GameManager gameManager;

    private void Awake()
    {
        netManager = NetManager.instance;
        gameManager = WaitingRoom_GameManager.instance;
    }

    public override void OnStartClient() //CallBack 함수
    {
        setNicknameText();
        setReadyText();
        gameManager.AddPlayerToPlayerSpace(this);
    }

    public override void ReadyStateChanged(bool _, bool newReadyState) // [Syncvar] readyToBegin hook
    {
        base.ReadyStateChanged(_, newReadyState);

        setReadyText();

    }
    public override void OnStopClient()
    {
        gameManager.removePlayerFromPlayerSpace(this);
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


    /*  Player의 UI를 변경함.
     *  PlayerSpace 라는 게임오브젝트를 Find 한뒤에
     *  ||  GameObject tryFind = GameObject.Find(playerSpaceObjectName + index);
     *  
     *  PlayerSpace의 UI를 RoomPlayerPrefab의 UI가 대체하는 방식임.
     *  ||  playerSpace.SetActive(true or false);
     */
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
    public void UpdateUI()

    {
        /*if (playerSpace == null)
        {
            GameObject tryFind = GameObject.Find(playerSpaceObjectName + index);
            if (tryFind == null)
            {
                return;
            }

            playerSpace = tryFind;
        }
        */
        
        /*
         * RectTransform playerSpaceRectTrans = playerSpace.GetComponentInChildren<RectTransform>();
        if (playerSpaceRectTrans == null)
        {
            Debug.LogError("PlayerSpace must get Rect Transform! - heeunAn");
            return;
        }

        Rect_Trans.localPosition = playerSpaceRectTrans.localPosition;
        */
        Nickname_txt.text = nickname;
        
        
    }
}
