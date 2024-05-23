using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Mirror;


public class GameMgr : MonoBehaviour
{

    public static GameMgr instance;

    public Transform[] boxSpawnPoints;
    public GameObject[] itemBox;
    public GameObject itemBoxSpawnPoints;
    public int boxSpawnCount;
    int[] boxCount = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

    public Transform[] missionSpawnPoints;
    public GameObject[] mission;
    public GameObject missionObjectSpawnPoints;
    public int missionSpawnCount;
    int[] missionCount = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14};

    [SerializeField] private GameMgrNetBehaviour gameMgrNet;

    public GameObject[] exitLever;
    public int missionClearCount;
    int[] exitCount = { 0, 1, 2, 3 };

    public static bool lockKey = false;

    void Start()
    {
        instance = this;
    }

    
    public void Init()
    {
        GetRandomInt(boxCount, boxCount.Length - 1);
        boxSpawnPoints = GetSpwanPoints(itemBoxSpawnPoints);
        SpawnObject(itemBox, boxSpawnPoints, boxCount, boxSpawnCount, 2);
        GetRandomInt(missionCount, missionCount.Length - 1);
        missionSpawnPoints = GetSpwanPoints(missionObjectSpawnPoints);
        SpawnObject(mission, missionSpawnPoints, missionCount, missionSpawnCount, 3);
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
    //스폰포인트 목록을 불러옴
    public Transform[] GetSpwanPoints(GameObject spawnPoinst)
    {
        return spawnPoinst.GetComponentsInChildren<Transform>();
    }
    //스폰포인트를 받아 지정한 오브젝트 소환
    private void SpawnObject(GameObject[] gameObject, Transform[] spawnPoints, int[] spawnCount, int objCount, int randomRange)
    {
        for (int i = 0; i < objCount; i++)
        {
            int rNum = Random.Range(0, randomRange);
            GameObject createdObject = Instantiate(gameObject[rNum], spawnPoints[spawnCount[i]].position, spawnPoints[spawnCount[i]].rotation);
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(createdObject);
            }
        }
    }

    // 아이템박스에 사용되는 코드 생성
    public static string GeneratePassword(int length)
    {
        StringBuilder codeSB = new StringBuilder(10);
        char singleChar;
        string numbers = "0123456789";
        bool firstGen = true;
        while (codeSB.Length < length)
        {
            singleChar = numbers[UnityEngine.Random.Range(0, numbers.Length)];
            codeSB.Append(singleChar);
            if(firstGen)
            {
                codeSB.Replace('0', numbers[UnityEngine.Random.Range(1, numbers.Length)]);
                firstGen = false;
            }
        }
        return codeSB.ToString();
    }
    //미션2에 사용되는 코드 생성
    public static string GenerateFourNumbers(int length)
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

    //미션1에 사용되는 코드 생성
    public static string GenerateMissionCode(int length)
    {
        string str = "123456789";
        char[] arr = str.ToCharArray();
        int n = arr.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, 9);
            var value = arr[k];
            arr[k] = arr[n];
            arr[n] = value;
        }
        string code = new string(arr);
        return code.Substring(0, length);
    }

    //missionClearCount를 증가시킴
    public void MissionClear()
    {
        gameMgrNet.AddMissionClearCount(); //GameMgr.missionClearCount를 증가
    }

    //missionClearCount 전달
    public int GetMissionClearCount()
    {
        return missionClearCount;
    }

    public int GetMissionSpawnPoint()
    {
        return missionSpawnCount;
    }

    [Server]
    //GameMgrNetBehaviour�κ��� �۵��˴ϴ�
    public void ActiveExitDoor()
    {
        GetRandomInt(exitCount, exitCount.Length - 1);
        gameMgrNet.RpcChangeLeverLayer(exitCount[0]);
        Debug.Log("Active Lever : " + exitCount[0]);
    }

    [Client]
    public void ChangeLeverLayer(int i)
    {
        exitLever[i].gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    //미션3에 사용
    public static string GenerateMissionTime()
    {
        StringBuilder codeSB = new StringBuilder(10);
        char singleChar;
        string numbers = "0123456789";
        int length = 4;
        for(int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                singleChar = numbers[UnityEngine.Random.Range(1, numbers.Length - 2)];
                codeSB.Append(singleChar);
            }
            singleChar = numbers[UnityEngine.Random.Range(0, numbers.Length)];
            codeSB.Append(singleChar);
        }
        return codeSB.ToString();
    }
}
