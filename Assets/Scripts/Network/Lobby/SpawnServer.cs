using UnityEditor;
using UnityEngine;
using System;

using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.Games;
using MasterServerToolkit.Logging;

public class SpawnServer : MonoBehaviour
{

    public void CreateRoomServer()
    {
        // Spawn options for spawner controller and spawn task controller
        var spawnOptions = new MstProperties();
        Mst.Events.Invoke(MstEventKeys.showLoadingInfo, "Starting room... Please wait!");

        Logs.Debug("Starting room... Please wait!");

        string roomName = GetRoomCode();

        spawnOptions.Add(MstDictKeys.ROOM_NAME, roomName);
        spawnOptions.Add(MstDictKeys.ROOM_MAX_PLAYERS, 4);
        spawnOptions.Add(MstDictKeys.ROOM_IS_PUBLIC, true);

        Debug.Log(MatchmakingBehaviour.Instance);

        
        if(MatchmakingBehaviour.Instance is NetMatchmakingBehaviour netMatchmaking)
        {
            netMatchmaking.SetRoomName(roomName);
        }

        MatchmakingBehaviour.Instance.CreateNewRoom(regionName:"", spawnOptions);
        // Start new game server/room instance
        //StartNewServerInstance(spawnOptions, customSpawnOptions);
    }

    private string GetRoomCode()
    {
        return "012";
    }

}
