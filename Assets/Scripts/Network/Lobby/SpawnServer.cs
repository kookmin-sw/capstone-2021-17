using UnityEditor;
using UnityEngine;

using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;


public class SpawnServer : MonoBehaviour
{
    public static SpawnServer Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        // Spawn options for spawner controller and spawn task controller
        var spawnOptions = new MstProperties();
        spawnOptions.Add(MstDictKeys.ROOM_MAX_PLAYERS, 10);
        spawnOptions.Add(MstDictKeys.ROOM_NAME, "Deathmatch [West 7]");
        spawnOptions.Add(MstDictKeys.ROOM_PASSWORD, "my-password-here-123");
        spawnOptions.Add(MstDictKeys.ROOM_IS_PUBLIC, true);

        // Custom options that will be given to room as command-line arguments
        var customSpawnOptions = new MstProperties();
        customSpawnOptions.Add(Mst.Args.Names.StartClientConnection);

        // Start new game server/room instance
        StartNewServerInstance(spawnOptions, customSpawnOptions);
    }

    public void AddMasterServerConnectionListners()
    {
        ClientToMasterConnector.Instance.Connection.AddConnectionListener(OnConnectedToMasterServer, true);
    }

    private void OnConnectedToMasterServer()
    {
        Init();
    }

    /// <summary>
    /// Starts new game server/room instance with given options
    /// </summary>
    /// <param name="spawnOptions"></param>
    /// <param name="customSpawnOptions"></param>
    public void StartNewServerInstance(MstProperties spawnOptions, MstProperties customSpawnOptions)
    {
        Mst.Client.Spawners.RequestSpawn(spawnOptions, customSpawnOptions, "", (controller, error) =>
        {
            // If controller is null it means an error occurred
            if (controller == null)
            {
                Debug.LogError(error);

                // Invoke your error event here

                return;
            }

            Mst.Events.Invoke(MstEventKeys.showLoadingInfo, "Room started. Finalizing... Please wait!");

            // Listen to spawn status
            controller.OnStatusChangedEvent += Controller_OnStatusChangedEvent;

            // Wait for spawning status until it is finished
            MstTimer.WaitWhile(() =>
            {
                return controller.Status != SpawnStatus.Finalized;
            }, (isSuccess) =>
            {
                // Unregister listener
                controller.OnStatusChangedEvent -= Controller_OnStatusChangedEvent;

                Mst.Events.Invoke(MstEventKeys.hideLoadingInfo);

                if (!isSuccess)
                {
                    Mst.Client.Spawners.AbortSpawn(controller.SpawnTaskId);

                    Debug.LogError("Failed spawn new room. Time is up!");

                    // Invoke your error event here

                    return;
                }

                Debug.Log("You have successfully spawned new room");

                // Invoke your success event here

            }, 30f);
        });
    }

    private void Controller_OnStatusChangedEvent(SpawnStatus status)
    {
        // Invoke your status event here to show in status window
    }
}
