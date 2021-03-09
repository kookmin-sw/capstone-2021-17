using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public GameObject[] pl = new GameObject[3];
    public GameObject pl2;
    public GameObject pl3;

    public Text[] Pl_box = new Text[3];
    public Image[] Pl_ServerBar = new Image[3];

    private bool isDead;
    private bool connectingServer;
    public int count_player;
    /*
    void Update()
    {
        CountPlayer(count_player);
    }
    private void CountPlayer(int i)){ //플레이어수 세기
        if(i==3)
            pl[i].SetActive(false);
    }
    private void setText(int num, Text name, float server){ //플레이어 박스, 서버상태와 플레이어 이름을 받아옴
        Pl_box[num].text = "Name : " + name + "\nHP : \n";
    }
    private string CheckHP(int num){
        HPManager.IsDead(num);
    }
    // Update is called once per frame*/
    
}
