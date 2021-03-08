using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetSpawnPlayer : NetworkBehaviour
{
    public List<Transform> spawnpoints;
    

    private NetworkManager networkManager;
    

    public override void OnStartClient()
    {
        base.OnStartClient();
        networkManager = NetworkManager.singleton;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        networkManager = NetworkManager.singleton;
        SpawnPlayers();
    }


    private int getSpawnCount = 0; // Used Only GetSpawnPoint

    private Transform GetSpawnPoint()
    {
        // 스폰 어떻게할지 알고리즘
        if(spawnpoints.Count < NetManager.PLAYER_MAXNUM)
        {
            Debug.LogError("Spawnpoint count is less than Player Maximum : " + NetManager.PLAYER_MAXNUM + " - Heeun AN");
            return null;
        }

        Transform point = spawnpoints[getSpawnCount];
        getSpawnCount++;

        return point;
    }

    [Server]
    public void SpawnPlayers()
    {
        Transform spawnPoint = GetSpawnPoint();

        GameObject newPlayer = Instantiate(networkManager.playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
