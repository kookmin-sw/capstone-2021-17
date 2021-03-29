using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_MultiGameManager : MonoBehaviour
{
    public static List<NetGamePlayer> Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);

    private static List<int> healths = new List<int>();

    private static List<string> names = new List<string>();

    private static List<ThirdPersonCharacter.State> states = new List<ThirdPersonCharacter.State>();

    public static int playerCount = 0;


    /***************** 데이터 전달 *****************/

    public static bool IsLocalPlayer(int index)
    {
        if (Players[index].isLocalPlayer)
        {
            return true;
        }
        return false;
    }

    public static void AddPlayer(NetGamePlayer newPlayer)
    {
        Players.Add(newPlayer);
        healths.Add(newPlayer.Health);
        names.Add(newPlayer.Nickname);
        states.Add(newPlayer.State);
    }
    public static void DisablePlayer(NetGamePlayer targetPlayer)
    {
        foreach(NetGamePlayer player in Players)
        {
            if(player == targetPlayer)
            {

            }
        }
    }
    // 체력 전달
    public static List<int> GetPlayersHealth()
    {
        for(int index = 0; index < Players.Count; index++)
        {
            healths[index] = Players[index].Health;
        }
        return healths;
    }

    //이름 전달
    public static List<string> GetPlayersNickname()
    {

        for (int index = 0; index < Players.Count; index++)
        {
            names[index] = Players[index].Nickname;
        }
        return names;
    }


    //플레이어 상태 전달
    public static List<ThirdPersonCharacter.State> GetPlayersState()
    {
        for (int index = 0; index < Players.Count; index++)
        {
            states[index] = Players[index].State;
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
