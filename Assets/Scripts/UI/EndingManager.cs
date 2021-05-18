using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Events;

public class EndingManager : MonoBehaviour
{

    public static EndingManager instance;
    public Text[] nickname;
    public Text gameClear;

    public List<EndingPlayerManager> endingPlayerManagers;

    public List<NetGamePlayer> players;

    [SerializeField]
    private GameObject EndingSceneObject;

    private List<SkinnedMeshRenderer> heads;
    private List<SkinnedMeshRenderer> bodys;

    public UnityEvent OnChangeEndingSceneObject;


    private bool isclear; //게임 클리어 여부

    void Awake()
    {
        instance = this;

        isclear = true;

        heads = new List<SkinnedMeshRenderer>();
        bodys = new List<SkinnedMeshRenderer>();

        foreach (var player in endingPlayerManagers)
        { // 플레이어간 동작을 맞추기 위해 플레이어들의 Mesh를 이용함
            heads.Add(player.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>());
            bodys.Add(player.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>());
        }
    }

    public void DisconnectRoom()
    {
        if (NetworkManager.singleton)
        {
            NetworkManager.singleton.StopClient(); // 시작으로 돌아감
        }
    }

    public void StartEnding()
    {
        EndingSceneObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update() // EndingMessage에는 플레이어 이름, 깼는지 죽었는지 상태가 포함됨.
    {
        if (!EndingSceneObject.activeSelf)
        {
            return;
        }
                //다른 플레이어들이 게임을 클리어할경우 EndingMessage가 NetManager.endingmessage로 데이터가 전달됨
        ShowPlayers();
        ShowPlayerText();
    }

    private void ShowPlayers() // 접속된 플레이어들 까지만 Mesh가 보이도록 함
    {
        for (int id = 0; id < players.Count; id++)
        {
            heads[id].gameObject.SetActive(true);
            bodys[id].gameObject.SetActive(true);


            if (players[id].EndState == PlayerEndingState.Escape)
            {
                endingPlayerManagers[id].isLive();
            }
            else
            {
                endingPlayerManagers[id].isDead();
            }


        }
        for (int id = players.Count; id < 4; id++)
        {
            heads[id].gameObject.SetActive(false);
            bodys[id].gameObject.SetActive(false);
        }
    }

    //캐릭터 모델 로드 후 캐릭터 상태에 따라 EndingPlayerManager의 islive or lsdead 호출

    private void ShowClearText() //게임 클리어, 게임 오버 여부 출력
    {
        //게임 클리어시
        if (isclear)
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
        for (int id = 0; id < players.Count; id++)
        {
            nickname[id].text = players[id].Nickname;
        } //왼쪽에서부터 id 순으로 닉네임 출력

        for (int id = players.Count; id < 4; id++)
        {
            nickname[id].text = "";
        } // 없는 플레이어들은 닉네임 표시 안함
    }
}

