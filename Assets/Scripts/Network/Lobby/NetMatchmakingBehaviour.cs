using UnityEditor;
using UnityEngine;
using MasterServerToolkit.Games;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;

public class NetMatchmakingBehaviour : MatchmakingBehaviour
{
    public delegate void FindMatchCallback(GameInfoPacket game);


    private string roomName;

    public void SetRoomName(string roomName)
    {
        this.roomName = roomName;
    }

    public override void StartMatch(GameInfoPacket gameInfo)
    {
        // Save room Id in buffer, may be very helpful
        Mst.Options.Set(MstDictKeys.ROOM_ID, gameInfo.Id);
        // Save max players to buffer, may be very helpful
        Mst.Options.Set(MstDictKeys.ROOM_MAX_PLAYERS, gameInfo.MaxPlayers);

        if (gameInfo.IsPasswordProtected)
        {
            Mst.Events.Invoke(MstEventKeys.showPasswordDialogBox,
                new PasswordInputDialoxBoxEventMessage("Room is required the password. Please enter room password below",
                () =>
                {
                    StartLoadingGameScene();
                }));
        }
        else
        {
            StartLoadingGameScene();
        }
    }

    public void FindMatch(string name, FindMatchCallback callback)
    {

        GameInfoPacket foundGame = null;

        MstTimer.WaitForSeconds(0.2f, () =>
        {
            Mst.Client.Matchmaker.FindGames((games) =>
            {
                if (games.Count == 0)
                {
                    Debug.Log("No Game Found");
                }

                foreach (GameInfoPacket game in games)
                {
                    Debug.Log(game.ToString());
                    if (game.Name == name)
                    {
                        foundGame = game;
                        break;
                    }
                }

                callback.Invoke(foundGame);
            });
        });

        
    }

    public void StartMatchByName(string name)
    {
        FindMatch(name, (game) =>
        {
            StartMatch(game);
        });
    }

    public void StartMyMatch()
    {
        StartMatchByName(roomName);
    }

}

