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

    public TMP_Text Nickname_txt; // 에디터 내에서 지정
    public Image Profile_image; // 에디터 내에서 지정
    public TMP_Text Readystatus_txt; // 에디터 내에서 지정

    public RectTransform Rect_Trans;

    public string playerSpaceObjectName = "PlayerSpace";

    public string NotReady_msg = "Not Ready";
    public string Ready_msg = "Ready";

    private NetManager netManager;

    [HideInInspector]
    public GameObject playerSpace;

    private void Awake()
    {
        netManager = NetManager.instance;
    }

    public override void OnStartClient()
    {
        Nickname_txt.text = nickname;
    }
    public override void IndexChanged(int oldIndex, int newIndex)
    {
        base.IndexChanged(oldIndex, newIndex);

        UpdateUI();
    }
    public override void ReadyStateChanged(bool _, bool newReadyState)
    {
        base.ReadyStateChanged(_, newReadyState);
        UpdateUI();
    }
    // 바뀌었을 때 ui가 바뀌는거
    // 
    public override void OnClientEnterRoom()
    {
        base.OnClientEnterRoom();
        UpdateUI();
    }

    public override void OnClientExitRoom()
    {
        if (SceneManager.GetActiveScene().path == netManager.RoomScene)
        {
            base.OnClientExitRoom();
            playerSpace.SetActive(true);
            UpdateUI();
        }

        if (SceneManager.GetActiveScene().path == netManager.GameplayScene)
        {
            Rect_Trans.gameObject.SetActive(false);
            Debug.Log(gameObject);
        }
    }



    public void UpdateUI()

    {
        /*if (!hasAuthority)
        {
            foreach (var player in netManager.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.ChangeUI();
                    break;
                }
            }

            return;
        }*/
        if (playerSpace == null)
        {
            GameObject tryFind = GameObject.Find(playerSpaceObjectName + index); // 효율저하 Refactoring  필요
            if (tryFind == null)
            {
                return;
            }

            playerSpace = tryFind;
        }
        
        RectTransform playerSpaceRectTrans = playerSpace.GetComponentInChildren<RectTransform>();
        if (playerSpaceRectTrans == null)
        {
            Debug.LogError("PlayerSpace must get Rect Transform! - heeunAn");
            return;
        }

        Rect_Trans.localPosition = playerSpaceRectTrans.localPosition;
        playerSpace.SetActive(false);
        Nickname_txt.text = nickname;
        
        if (readyToBegin)
        {
            Readystatus_txt.text = Ready_msg;
        }
        else
        {
            Readystatus_txt.text = NotReady_msg;
        }

    }



    
    

}
