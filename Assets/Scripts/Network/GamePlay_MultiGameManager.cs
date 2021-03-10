using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GamePlay_MultiGameManager : MonoBehaviour
{
    public List<Transform> spawnpoints;

    public static GamePlay_MultiGameManager instance;

    private NetworkManager networkManager;
    
    private int getSpawnCount = 0; // Used Only GetSpawnPoint

    private void Awake()
    {
        instance = this;
    }

    public Transform GetSpawnPoint()
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

}
