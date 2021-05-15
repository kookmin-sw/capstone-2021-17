using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InGame_MultiGameManager : MonoBehaviour
{
    public static List<NetGamePlayer> Players;

    private static List<int> healths;

    private static List<string> names;

    private static List<ThirdPersonCharacter.State> states;

    public static InGame_MultiGameManager instance;

    public Camera DebugIntroCam;

    /***************** 데이터 전달 *****************/

    private void Awake()
    {
        instance = this;
        Players = new List<NetGamePlayer>(NetManager.PLAYER_MAXNUM);
        healths = new List<int>();
        names =  new List<string>();
        states = new List<ThirdPersonCharacter.State>();

    }
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
                player.Health = 0;
                player.Nickname = "Disconnected";
                player.State = ThirdPersonCharacter.State.Die;
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
