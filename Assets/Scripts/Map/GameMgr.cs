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
        SpawnObject(itemBox, boxSpawnPoints, boxCount, boxSpawnCount, 1);
        GetRandomInt(missionCount, missionCount.Length - 1);
        missionSpawnPoints = GetSpwanPoints(missionObjectSpawnPoints);
        SpawnObject(mission, missionSpawnPoints, missionCount, missionSpawnCount, 2);
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
            //Debug.Log(boxCount[i] + "위치에 아이템박스 생성");
        }
    }

    //아이템박스 패스워드 생성
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

    //미션에 사용한 셔플 알고리즘 - 9자리 까지만 생성가능, 중복체크함
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

    //missionClearCount를 이용해 생성된 미션이 전부 완료됬는지 판단하고 탈출구를 작동시킴
    public static void MissionClear()
    {
        instance.missionClearCount++;
        if (instance.missionClearCount == instance.missionSpawnCount)
        {
            instance.ActiveExitDoor();
        }
    }

    public void ActiveExitDoor()
    {
        GetRandomInt(exitCount, exitCount.Length - 1);
        exitLever[exitCount[0]].gameObject.layer = LayerMask.NameToLayer("Interact");
        Debug.Log("Active Lever : " + exitCount[0]);
    }

    //미션에 사용할 패스워드 - 원하는 길이만큼 생성가능, 중복체크안함
    //차후 변경할 가능성 있으니 지우지 말것
    /*public static string GenerateMissionCode(int length)
    {
        StringBuilder codeSB = new StringBuilder(10);
        char singleChar;
        string numbers = "123456789";

        while (codeSB.Length < length)
        {
            singleChar = numbers[UnityEngine.Random.Range(0, numbers.Length)];
            codeSB.Append(singleChar);
        }
        return codeSB.ToString();
    }*/
}
