using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class EndingManager : MonoBehaviour
{

    public static EndingManager instance;
    public Text[] nickname;
    public Text gameClear;
    
    public List<EndingPlayerMessage> messages;

    [SerializeField]
    private List<SkinnedMeshRenderer> heads;
    [SerializeField]
    private List<SkinnedMeshRenderer> bodys;



    private bool isclear; //게임 클리어 여부

    void Awake()
    {
        instance = this;
        
        isclear = true;

        messages = new List<EndingPlayerMessage>();


        UpdatePlayers(); 
    }

    public void UpdatePlayers()
    {
        if (NetworkManager.singleton is NetManager netManager)
        {
            messages = netManager.EndingMessages;
        }
        else if (NetworkManager.singleton is DebugInGameNetManager debugInGameManager)
        {
            messages = debugInGameManager.EndingMessages;
        }

        //다른 플레이어들이 게임을 클리어할경우 EndingMessage가 NetManager.ending
        ShowPlayers();
        ShowPlayerText();
    }

    private void ShowPlayers()
    {
        for (int id = 0; id < messages.Count; id++)
        {
            heads[id].gameObject.SetActive(true);
            bodys[id].gameObject.SetActive(true);
        }
        for (int id = messages.Count; id < 4; id++)
        {
            heads[id].gameObject.SetActive(false);
            bodys[id].gameObject.SetActive(false);
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
        for(int id = 0; id<messages.Count; id++)
        {
            nickname[id].text = messages[id].PlayerName;
        } //왼쪽에서부터 id 순으로 닉네임 출력
    }
}
