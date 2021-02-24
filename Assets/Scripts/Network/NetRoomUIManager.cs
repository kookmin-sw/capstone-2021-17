using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NetRoomUIManager : MonoBehaviour
{
    /*
     public NetManager netManager;

    public string Ready_MSG = "READY";
    public string Unready_MSG = "UNREADY";
    public string Waiting_MSG = "Wating...";
    private static int PLAYER_MAXNUM = NetManager.PLAYER_MAXNUM;

    public TMP_Text[] nicknames = new TMP_Text[PLAYER_MAXNUM];
    public Image[] images = new Image[PLAYER_MAXNUM];
    public TMP_Text[] readyStatus = new TMP_Text[PLAYER_MAXNUM];

    private void Awake()
    {
        netManager = NetManager.instance;
        netManager.RoomUIManager = this;
        UpdateRoomUI();
    }

    public void UpdateRoomUI()
    {
        Debug.Log("UPDATE ROOM UI!");
        GameObject[] g = GameObject.FindGameObjectsWithTag("RoomPlayer");
        Debug.Log(g.Length);

        NetworkRoomPlayer[] RoomPlayers = GetComponents<NetRoomPlayer>();
        if (RoomPlayers.Length == 0)
        {
            Debug.LogError("No Player Detected in Room Scene");
        }
        else
        {
            bool[] is_exist = new bool[PLAYER_MAXNUM];
            foreach(NetRoomPlayer player in RoomPlayers)
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

    


    */



}
