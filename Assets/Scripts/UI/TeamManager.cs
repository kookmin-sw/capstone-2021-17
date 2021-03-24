using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public Text[] UI;

    public static List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);
    private static List<int> healths = new List<int>();
    private static List<string> names = new List<string>();
    private static List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();

    private static int[] server;


    void Start()
    {
        InGame_MultiGameManager.GetPlayersNickname();
        InGame_MultiGameManager.GetPlayersHealth();
        InGame_MultiGameManager.GetPlayersState();
    }

    void Update()
    {
        ShowText();
    }

//텍스트 정보 출력 (이름, 체력, 서버상태)
    private void ShowText()
    {
        for(int id = 0; id<Players.Count-1; id++)
        {
            if(InGame_MultiGameManager.isPlayerLeader(id+1))
            {
                UI[id].text = "Name : " + names[id+1] + "\n" + healths[id+1] + "\n state : " 
            + states[id] + "\n < Leader";
            }
            else
            { 
                UI[id].text = "Name : " + names[id+1] + "\n" + healths[id+1] + "\n state : " 
                + states[id];
            }
        }
    }
/*
    //서버 상태 간략화
    private void SimpleServer(){

    }*/

    //사망 또는 게임 클리어 여부

        

}
