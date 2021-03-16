using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public Text[] UI;
    private int count_player;
    private string[] p_name;
    private float[] p_HP;
    private float[] p_server;

    void Start()
    {
        count_player = UI.Length;
        LoadName();
        LoadServer();
        p_HP = new float[count_player];
    }

    void Update()
    {
        ShowText();
    }

//텍스트 정보 출력 (이름, 체력, 서버상태)
    private void ShowText()
    {
        for(int id = 0; id<count_player; id++)
        {
            UI[id].text = "Name : " + p_name[id] + "\n" + CheckHP(id) + "\n Server : " 
            + p_server[id];
        }
    }

//이름 가져오기 (임시 목업)
    private void LoadName()
    {
        p_name = new string[count_player];
        p_name[0] = "dog";
        p_name[1] = "cat";
        p_name[2] = "bird";
    }

    private string CheckHP(int _id)
    {
        //hp정보 연동
        p_HP[_id] = HPManager.hp[_id+1];

        //사망시 Dead 표시
        if(p_HP[_id]==0)
        {
            return "Dead";
        }
        else
        {
            return "HP : " + p_HP[_id];
        }
    }

    //서버 정보 연동(임시 목업)
    private void LoadServer()
    {
        p_server = new float[count_player];
        p_server[0] = 33;
        p_server[1] = 66;
        p_server[2] = 99;
    }
        

}
