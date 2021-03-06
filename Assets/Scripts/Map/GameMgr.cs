using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public Transform[] points;
    public GameObject itembox;
    public int boxcount;

    void Start()
    {
        List<Transform> temp_list = new List<Transform>(points);
        int idx;
        for (int i =0; i<boxcount; ++i)
        {
            idx = Random.Range(0, temp_list.Count);
            Instantiate(itembox, temp_list[idx].position, Quaternion.identity);
            temp_list.Remove(temp_list[idx]);
            Debug.Log(idx+"위치에 생성");
        }
    }

    void Update()
    {

    }
}
