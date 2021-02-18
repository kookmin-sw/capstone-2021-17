using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NetworkRoomUIManager : MonoBehaviour
{
    public CustomNetworkRoomManager networkManager;

    public string Ready_MSG = "READY";
    public string Unready_MSG = "UNREADY";
    public string Waiting_MSG = "Wating...";
    private static int PLAYER_MAXNUM = CustomNetworkRoomManager.PLAYER_MAXNUM;

    public TMP_Text[] nicknames = new TMP_Text[PLAYER_MAXNUM];
    public Image[] images = new Image[PLAYER_MAXNUM];
    public TMP_Text[] readyStatus = new TMP_Text[PLAYER_MAXNUM];

    private void Awake()
    {
        networkManager = CustomNetworkRoomManager.instance;
        networkManager.RoomUIManager = this;
    }

    public void UpdateRoomUI()
    {
        Debug.Log("UPDATE ROOM UI!");
        List<NetworkRoomPlayer> RoomPlayers = networkManager.roomSlots;
        Debug.Log(RoomPlayers.Count);
        if (RoomPlayers.Count == 0)
        {
            Debug.LogError("No Player Detected in Room Scene");
        }
        else
        {
            bool[] is_exist = new bool[PLAYER_MAXNUM];
            foreach(CustomNetworkRoomPlayer player in RoomPlayers)
            {
                int index = player.index;
                is_exist[index] = true;

                nicknames[index].text = player.nickname;
                // images[index] == ??
                readyStatus[index].text = player.isReady ? Ready_MSG : Unready_MSG;
            }

            for (int i = 0; i < PLAYER_MAXNUM; i++)
            {
                if (!is_exist[i])
                {
                    nicknames[i].text = "";
                    readyStatus[i].text = Waiting_MSG;
                }
            }
        }
        
    }

    






}
