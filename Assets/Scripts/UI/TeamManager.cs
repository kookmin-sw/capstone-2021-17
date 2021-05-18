using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TeamManager : MonoBehaviour
{
    public Text[] NameUI;
    public Text[] EndUI;

    public List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);
    private List<string> names = new List<string>();



    void Update()
    {
        if (NetworkClient.active || NetworkServer.active)
        {
            Players = InGame_MultiGameManager.Players;
            names = InGame_MultiGameManager.GetPlayersNickname();
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
            NameUI[otherIdx].text = names[id];

            if(Players[id].EndState == PlayerEndingState.Live)
            {
                EndUI[otherIdx].text = "ESCAPED!";
            }
            else if(Players[id].EndState == PlayerEndingState.Dead)
            {
                EndUI[otherIdx].text = "DEAD!";
            }
            else if(Players[id].EndState == PlayerEndingState.Disconnected)
            {
                EndUI[otherIdx].text = "DISCONNECTED";
            }
            otherIdx++;
        }

        for(int id = otherIdx ; id < 3; id++)
        {
            NameUI[id].text = "";
            EndUI[id].text = "";
        }
    }
/*
    //서버 상태 간략화
    private void SimpleServer(){

    }*/

    //사망 또는 게임 클리어 여부

        

}
