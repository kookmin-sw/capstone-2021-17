using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public Transform[] points;
    public GameObject itembox;
    public int boxcount;
    public int spawnpointcount;

    void Start()
    {
        List<Transform> temp_list = new List<Transform>(points);
        int[] idx = getRandomInt(boxcount, 0, spawnpointcount);
        for (int i=0; i<boxcount; i++)
        {
            Instantiate(itembox, temp_list[idx[i]].position, Quaternion.identity);
            Debug.Log(idx[i]+"위치에 아이템박스 생성");
            //temp_list.Remove(temp_list[idx[i]]);
        }
    }

    void Update()
    {

    }

    public int[] getRandomInt(int length, int min, int max)
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
}
