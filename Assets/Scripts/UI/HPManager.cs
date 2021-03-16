using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    //1 : 플레이어, 2 : 팀원 1, 3 : 팀원2, 3 : 팀원3
    public Image[] bar = new Image[4];
    public Button button_plus;
    public Button button_minus;
    public GameObject gameOver; 

    public static float[] hp = new float[4];
    private float hp_max = 3;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<hp.Length; i++)
        {
            hp[i] = hp_max;
        }
    }

    //버튼 함수(임시)
    public void Function_Button_Plus(int num)
    {
        Change_HP(+1, num);
    }
    public void Function_Button_Minus(int num)
    {
        Change_HP(-1, num);
    }

    //체력 변경
    private void Change_HP(float _value, int num)
    {
        hp[num] += _value;
        Set_HP(hp[num], num);
    }

    public static bool IsDead(int num)
    {
        if(hp[num]==0)
            return true;
        else
            return false;
    }
    //HP 바 표시
    private void Set_HP(float value, int num)
    {
        hp[num] = value;
        if(hp[num]<=0)
        {
            hp[num] = 0;
        }
        else
        {
            if(hp[num]>hp_max){
                hp[num] = hp_max;
            }
        }

        bar[num].fillAmount = hp[num] / hp_max;

    //체력 0일 시 게임오버창
        if(IsDead(num)==true && num==0)
        {
            gameOver.SetActive(true);
        }

        Debug.Log("체력 : " + hp[num]);
    }
}
