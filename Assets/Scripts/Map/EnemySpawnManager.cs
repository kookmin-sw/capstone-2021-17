using UnityEditor;
using UnityEngine;
using Mirror;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public Transform[] spawnPoints;
    public Transform[] wayPoints;

    [SerializeField]
    private GameObject enemyPrefab; 
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

        SpawnEnemy(spawnPoints);
    }

    public void SpawnEnemy(Transform[] spawnPoints)
    {
        for(int i=0; i<spawnPoints.Length; i++)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            // 동적으로 Waypoint 할당
            enemy.SetWayPoints(wayPoints);

            if (NetworkServer.active)
            {
                NetworkServer.Spawn(enemyObject);
            }
        }        

    }
}
