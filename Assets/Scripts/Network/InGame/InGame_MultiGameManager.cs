using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_MultiGameManager : MonoBehaviour
{
    public static List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);



    /***************** 데이터 전달 *****************/


    // 체력 전달
    public static List<int> GetPlayersHealth()
    {
        List<int> healths = new List<int>();
        foreach(NetGamePlayer player in Players)
        {
            healths.Add(player.Health);
        }

        return healths;
    }

    //이름 전달
    public static List<string> GetPlayersNickname()
    {
        List<string> nicknames = new List<string>();
        foreach (NetGamePlayer player in Players)
        {
            nicknames.Add(player.Nickname);
        }

        return nicknames;
    }


    //플레이어 상태 전달
    public static List<ThirdPersonCharacter.State> GetPlayersState()
    {
        List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();
        foreach (NetGamePlayer player in Players)
        {
            states.Add(player.State);
        }
        return states;
    }


    // 누가 방장인지 알려주는 기능. 필요할지?
    public static bool isPlayerLeader(int index)
    {
        if (Players[index].isLeader)
        {
            return true;
        }
        return false;
    }


}
