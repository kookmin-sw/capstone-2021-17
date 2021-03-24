using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public Image[] bar = new Image[4];
    public GameObject gameOver; 

    public static List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);
    private static List<int> healths = new List<int>();
    private static List<string> names = new List<string>();
    private static List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();

    private int hp_max = 2;
    private bool isDead;

    // 전원 체력 2로 시작
    void Start()
    {
        for(int i=0; i<healths.Count; i++)
        {
            healths[i] = hp_max;
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
        InGame_MultiGameManager.GetPlayersHealth();

        for(int i=0; i<Players.Count; i++)
        {
            if(healths[i]<=0)
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

            bar[i].fillAmount = healths[i] / hp_max;
        }

    //체력 0일 시 게임오버창 출력
        if(IsDead(0)==true)
        {
            gameOver.SetActive(true);
        }

        Debug.Log("체력 : " + healths[0]);
    }
}
