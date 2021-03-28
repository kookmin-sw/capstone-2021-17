using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TeamManager : MonoBehaviour
{
    public Text[] UI;

    public static List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);
    private static List<int> healths = new List<int>();
    private static List<string> names = new List<string>();
    private static List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();


    void Update()
    {
        if (NetworkClient.active || NetworkServer.active)
        {
            Players = InGame_MultiGameManager.Players;
            names = InGame_MultiGameManager.GetPlayersNickname();
            healths = InGame_MultiGameManager.GetPlayersHealth();
            states = InGame_MultiGameManager.GetPlayersState();
        }
        ShowText();
    }

//텍스트 정보 출력 (이름, 체력, 서버상태)
    private void ShowText()
    {
        int otherIdx = 0; // Other Index == 자기 자신을 제외한 플레이어들의 Index
        for(int id = 0; id<Players.Count; id++)
        {

            if (InGame_MultiGameManager.IsLocalPlayer(id))
            {
                continue; // Local Player =  바로 플레이어 자기자신
                          // 플레이어가 자기 자신일때는 otherIndex가 오르지 않음.
            }
            if(InGame_MultiGameManager.isPlayerLeader(id))
            {
                UI[otherIdx].text = "Name : " + names[id] + "\n" + healths[id] + "\n state : " 
            + states[id] + "\n < Leader";
            }
            else
            { 
                UI[otherIdx].text = "Name : " + names[id] + "\n" + healths[id] + "\n state : " 
                + states[id];
            }
            otherIdx++;
        }
    }
/*
    //서버 상태 간략화
    private void SimpleServer(){

    }*/

    //사망 또는 게임 클리어 여부

        

}
