using UnityEditor;
using UnityEngine;
using MasterServerToolkit.Games;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;

public class NetMatchmakingBehaviour : MatchmakingBehaviour
{

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

    public void StartMatchByName(string name)
    {
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
                    Debug.Log(game.Properties);
                    Debug.Log(game.ToString());
                    if (game.Name == name)
                    {
                        StartMatch(game);
                    }

                }

            });
        });

        
    }

    public void StartMyMatch()
    {
        StartMatchByName(roomName);
    }

}

