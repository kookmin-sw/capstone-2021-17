using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject itemBox;
    public int boxCount;
    public int spawnPointCount;

    void Start()
    {
        int[] randomIndex = GetRandomInt(boxCount, 1, spawnPointCount+1);
        spawnPoints = GetSpwanPoints("SpawnPoint");
        SpawnObject(boxCount, randomIndex);
    }

    void Update()
    {

    }

    public int[] GetRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i=0; i<length; i++)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j=0; j<i; j++)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }

    public Transform[] GetSpwanPoints(string pointsName)
    {
        return GameObject.Find(pointsName).GetComponentsInChildren<Transform>();
    }

    public void SpawnObject(int objCount, int [] randomPosition)
    {
        for (int i = 0; i < objCount; i++)
        {
            Instantiate(itembox, spawnPoints[randomPosition[i]].position, Quaternion.identity);
            Debug.Log(randomPosition[i] + "위치에 아이템박스 생성");
        }
    }
}
