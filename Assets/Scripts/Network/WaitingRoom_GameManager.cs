using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitingRoom_GameManager : NetworkBehaviour
{
    private NetManager netManager;
    public static WaitingRoom_GameManager instance;
    public PlayerSpace[] playerSpaces = new PlayerSpace[NetManager.PLAYER_MAXNUM];

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        netManager = NetManager.instance;

    }

    public void AddPlayerToPlayerSpace(NetRoomPlayer newPlayer)
    {
        
        foreach (PlayerSpace space in playerSpaces)
        {
            if (space.player == null)
            {
                space.player = newPlayer;
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
    public void removePlayerFromPlayerSpace(NetRoomPlayer roomPlayer)
    {
        foreach (PlayerSpace space in playerSpaces)
        {
            if(space.player.gameObject == roomPlayer.gameObject)
            {
                space.gameObject.SetActive(true);
                break;
            }
        }
    }






}
