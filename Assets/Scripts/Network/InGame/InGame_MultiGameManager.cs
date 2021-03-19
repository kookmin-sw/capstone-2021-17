using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_MultiGameManager : MonoBehaviour
{
    public List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);

    public static InGame_MultiGameManager instance;

    private void Awake()
    {
        instance = null;
    }

    /***************** 데이터 전달 *****************/

    public List<int> GetPlayersHealth()
    {
        List<int> healths = new List<int>();
        foreach(NetGamePlayer player in Players)
        {
            healths.Add(player.Health);
        }

        return healths;
    }

    public List<string> GetPlayersNickname()
    {
        List<string> nicknames = new List<string>();
        foreach (NetGamePlayer player in Players)
        {
            nicknames.Add(player.Nickname);
        }

        return nicknames;
    }

    public List<ThirdPersonCharacter.State> GetPlayersState()
    {
        List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();
        foreach (NetGamePlayer player in Players)
        {
            states.Add(player.State);
        }
        return states;
    }


    // 누가 방장인지 알려주는 기능. 필요할지?
    public bool isPlayerLeader(int index)
    {
        if (Players[index].isLeader)
        {
            return true;
        }
        return false;
    }


}
