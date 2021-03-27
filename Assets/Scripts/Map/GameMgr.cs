using System.Text;
using UnityEngine;
using Mirror;


public class GameMgr : MonoBehaviour
{

    public static GameMgr instance;

    public Transform[] boxspawnPoints;
    public GameObject itemBox;
    public GameObject itemBoxSpawnPoints;
    public int spawnPointCount;
    int[] boxCount = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

    void Start()
    {
        instance = this;
        //아이템박스 생성
    }

    
    public void Init()
    {
        GetRandomInt(boxCount, boxCount.Length - 1);
        boxspawnPoints = GetSpwanPoints(itemBoxSpawnPoints);
        SpawnObject(itemBox, boxspawnPoints, spawnPointCount);
    }
    static void GetRandomInt(int []arr, int max)
    {
        System.Random r = new System.Random();
        for (int i = max; i > 0; i--)
        {
            int j = r.Next(0, i + 1);
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }

    public Transform[] GetSpwanPoints(GameObject spawnPoinst)
    {
        return spawnPoinst.GetComponentsInChildren<Transform>();
    }
    
    private void SpawnObject(GameObject gameObject, Transform[] spawnPoints, int objCount)
    {
        for (int i = 0; i < objCount; i++)
        {
            GameObject createdObject = Instantiate(gameObject, spawnPoints[boxCount[i]].position, Quaternion.identity);
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(createdObject);
            }
            //Debug.Log(boxCount[i] + "위치에 아이템박스 생성");
        }
    }

    public static string GeneratePassword(int length)
    {
        StringBuilder codeSB = new StringBuilder(10);
        char singleChar;
        string numbers = "0123456789";

        while (codeSB.Length < length)
        {
            singleChar = numbers[UnityEngine.Random.Range(0, numbers.Length)];
            codeSB.Append(singleChar);
        }
        return codeSB.ToString();
    }
}
