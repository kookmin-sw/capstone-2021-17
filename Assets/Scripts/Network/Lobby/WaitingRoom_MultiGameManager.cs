using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*  WaitingRoomScene의 게임오브젝트를 다루는 Manager 객체입니다.
 *  NetRoomPlayer.cs가 이 스크립트의 메소드를 호출합니다
 * 
 */

public class WaitingRoom_MultiGameManager : MonoBehaviour
{
    private NetManager netManager;
    public static WaitingRoom_MultiGameManager instance;
    public PlayerSpace[] playerSpaces = new PlayerSpace[NetManager.PLAYER_MAXNUM];

    [SerializeField]
    private StartButton startButton;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        netManager = NetManager.instance;

    }

    //룸플레이어가 들어올경우 룸플레이어를 룸플레이어 공간에 옮기고 롬플레이어 공간은 비활성화
    public void AddPlayerToPlayerSpace(NetRoomPlayer newPlayer)
    {
        Debug.Log(newPlayer.roomidx);
        if (newPlayer.roomidx != -1)
        {
            PlayerSpace space = playerSpaces[newPlayer.roomidx];
            space.player = newPlayer;
            RectTransform playerSpaceRectTrans = space.GetComponentInChildren<RectTransform>();
            space.player.Rect_Trans.localPosition = playerSpaceRectTrans.localPosition;
            space.gameObject.SetActive(false);
        }
        else
        {
            for (int index = 0; index < playerSpaces.Length; index++)
            {
                PlayerSpace space = playerSpaces[index];
                if (space.player == null)
                {
                    space.player = newPlayer;
                    newPlayer.roomidx = index;
                    Debug.Log("roomidx= "+ index);
                    RectTransform playerSpaceRectTrans = space.GetComponentInChildren<RectTransform>();
                    if (playerSpaceRectTrans == null)
                    {
                        Debug.LogError("PlayerSpace must get Rect Transform! - heeunAn");
                        return;
                    }

                    space.player.Rect_Trans.localPosition = playerSpaceRectTrans.localPosition;
                    space.gameObject.SetActive(false);
                    break;
                }
            }
        }
        foreach (PlayerSpace space in playerSpaces)
        {
            
        }
    }
    //룸 플레이어가 나갈시 플레이어 공간을 다시 활성화
    public void RemovePlayerFromPlayerSpace(NetRoomPlayer roomPlayer)
    {
        if (!NetManager.IsSceneActive(netManager.RoomScene))
        {
            return;
        }
        foreach (PlayerSpace space in playerSpaces)
        {
            if(space.player.gameObject == roomPlayer.gameObject)
            {
                space.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void ActivateStartButton()
    {
        if (startButton == null)
        {
            return;
        }
        startButton.gameObject.SetActive(true);
    }

    public void DeActivateStartButton()
    {
        if (startButton == null)
        {
            return;
        }
        startButton.gameObject.SetActive(false);
    }




}
