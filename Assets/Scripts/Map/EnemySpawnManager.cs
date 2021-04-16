using UnityEditor;
using UnityEngine;
using Mirror;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public Transform[] spawnPoints;
    public Transform[] wayPoints;

    [SerializeField]
    private GameObject EnemyPrefab; 
    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        if(spawnPoints == null)
        {
            Debug.LogWarning("Need to Set SpawnPoint at EnemySpawnManager! - Heeun An");
        }
        SpawnEnemy(spawnPoints[0]);
    }

    public void SpawnEnemy(Transform pos)
    {
        GameObject enemyObject = Instantiate(EnemyPrefab, pos.position, Quaternion.identity);
        EnemyControl enemyControl = enemyObject.GetComponent<EnemyControl>();

        // 동적으로 Waypoint 할당
        enemyControl.wayPoint = wayPoints;

        if (NetworkServer.active)
        {
            NetworkServer.Spawn(enemyObject);
        }

    }
}
