using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
/* GamePlay Scene에서의 게임오브젝트를 다루는 Manager 객체입니다.
 * 약간 다른 게임매니저와는 역할이 겹칠것같아 스크립트 내용을 바꿀지 고민입니다.
 */
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
