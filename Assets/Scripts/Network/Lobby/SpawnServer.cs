using UnityEditor;
using UnityEngine;
using System;


using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.Games;
using MasterServerToolkit.Logging;
using System.Text;

public class SpawnServer : MonoBehaviour
{
    private Action CreateServerAction;
    public void CreateRoomServer()
    {
        // Spawn options for spawner controller and spawn task controller
        var spawnOptions = new MstProperties();
        Mst.Events.Invoke(MstEventKeys.showLoadingInfo, "Starting room... Please wait!");

        Logs.Debug("Starting room... Please wait!");

        string roomName = GetRoomCode();

        if (MatchmakingBehaviour.Instance is NetMatchmakingBehaviour netMatchmaking)
        {
            netMatchmaking.FindMatch(roomName, (game) =>
            {
                if(game != null)
                {
                    CreateServerAction += CreateRoomServer;
                    CreateServerAction?.Invoke();
                }
                else
                {
                    netMatchmaking.SetRoomName(roomName);
                    spawnOptions.Add(MstDictKeys.ROOM_NAME, roomName);
                    spawnOptions.Add(MstDictKeys.ROOM_MAX_PLAYERS, 4);
                    spawnOptions.Add(MstDictKeys.ROOM_IS_PUBLIC, true);

                    MatchmakingBehaviour.Instance.CreateNewRoom(regionName: "", spawnOptions);
                }
            }); 
        }

        
        // Start new game server/room instance
        //StartNewServerInstance(spawnOptions, customSpawnOptions);
    }

    private string GetRoomCode()
    {
        string sample = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var builder = new StringBuilder();
        for(int i = 0; i < 4; i++)
        {
            int randomIdx = UnityEngine.Random.Range(0, sample.Length);
            builder.Append(sample[randomIdx]);
        }
        return builder.ToString();
    }

}
