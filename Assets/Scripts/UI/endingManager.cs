using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class EndingManager : MonoBehaviour
{
    public Text[] nickname;
    public Text gameClear;

    public List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);
    private List<string> names = new List<string>();
    private List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();
    private List<string> networkTimes = new List<string>();

    
    private bool isclear; //게임 클리어 여부

    void Start()
    {
        
        isclear = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkClient.active || NetworkServer.active)
        {
            Players = InGame_MultiGameManager.Players;
            names = InGame_MultiGameManager.GetPlayersNickname();
            networkTimes = InGame_MultiGameManager.GetPlayersNetworkTime(); 
        }
    }

    //캐릭터 모델 로드 후 캐릭터 상태에 따라 EndingPlayerManager의 islive or lsdead 호출

    private void ShowClearText() //게임 클리어, 게임 오버 여부 출력
    {
        //게임 클리어시
        if(isclear)
        {
            gameClear.text = "Game Clear!";
        }
        else //게임 오버(전원 사망) 시
        {
            gameClear.text = "Game Over...";
        }
        
    }

    private void ShowPlayerText() //플레이어캐들 닉네임 출력
    {
        for(int id = 0; id<Players.Count; id++)
        {
            nickname[id].text = names[id];
        } //왼쪽에서부터 id 순으로 닉네임 출력
    }
}
