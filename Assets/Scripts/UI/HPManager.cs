using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HPManager : MonoBehaviour
{
    public Image[] bar = new Image[4];
    public GameObject gameOver;

    //public static List<NetGamePlayer> Players;
    private static List<int> healths;
    //private static List<string> names;
    //private static List<ThirdPersonCharacter.State> states;

    private int hp_max = 2;
    private bool isDead;

    // 전원 체력 2로 시작
    void Start()
    {
        /* 서버가 실행되기전에는 Start해도 안먹힘 SceneObject -> NetworkObject 순으로 활성화됨.
        for (int i=0; i<healths.Count; i++)
        {
            healths[i] = hp_max;
        }
        */
    }

    void Update()
    {
        if (NetworkClient.active || NetworkServer.active)
        {
            healths = InGame_MultiGameManager.GetPlayersHealth();
            
            Set_HP();
        }
    }

    //플레이어 (리스트 0번) 사망 여부 확인
    public static bool IsDead(int num)
    {
        if(healths[num]==0)
            return true;
        else
            return false;
    }

    //체력과 HP 바 연동
    private void Set_HP()
    {
        int localPlayerIdx = 0; // LocalPlayerIdx - 실제 자기자신 플레이어의 Index
        
        if(healths.Count ==0)
        {
            return; // 시작하자마자 플레이어가 추가되는게 아니여서 플레이어 추가 안됐을때 0이 됨.
        }

        int otherIdx = 1; // Other Index == 자기 자신을 제외한 플레이어들의 Index

        for (int i=0; i<healths.Count; i++)
        {
            if (healths[i]<=0)
            {
                healths[i] = 0;
            }
            else
            {
                if(healths[i] > hp_max)
                {
                    healths[i] = hp_max;
                }
            }

            if (InGame_MultiGameManager.IsLocalPlayer(i))
            {
                localPlayerIdx = i;
                bar[0].fillAmount = (float)healths[i] / hp_max;
            }
            else
            {
                bar[otherIdx].fillAmount = (float)healths[i] / hp_max;
                otherIdx++;
            }

            
        }

    //체력 0일 시 게임오버창 출력
        if(IsDead(localPlayerIdx)==true)
        {
            gameOver.SetActive(true);
            //keypadSystem.KPDisableManager.instance.DisablePlayer(true) 할시 플레이어 동작 정지 가능함.
        }
    }
}
