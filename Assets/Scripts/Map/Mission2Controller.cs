using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class Mission2Controller : MonoBehaviour
{
    //캔버스
    [SerializeField] private GameObject missionCanvas;
    //연동할 오브젝트
    [SerializeField] private GameObject missionObject;
    //네트워크
    [SerializeField] private MissionNetBehaviour missionNet;
    //저장된 퀴즈의 답
    public string correctCode;
    //랜덤하게 섞인값
    public string randomCode;
    //입력받은값 변동확인용
    public bool firstClick;
    //현재 상태를 표기할 텍스트
    public Text[] state;
    //미션 클리어 여부
    private bool clearMission = false;


    public void CheckCode()
    { 
        if(state[1].text == correctCode[3].ToString() && state[2].text == correctCode[2].ToString() && state[3].text == correctCode[1].ToString() && state[4].text == correctCode[0].ToString())
        {
            clearMission = true;
        }
        if (clearMission == true)
        {
            state[0].text = "Clear";
            if(missionNet != null)
            {
                missionNet.UnableMission();
            }
            else
            {
                UnableMission();
            }
            ValidStageClear();

            //1초뒤 창 종료
            Invoke("CloseMission", 1.0f);
        }
    }

    //미션오브젝트 상호작용 제거
    public void UnableMission()
    {
        missionObject.tag = "Untagged";
        missionObject.layer = 0;
    }

    //미션창 활성화
    public void ShowMission()
    {
        if (missionNet != null)
        {
            if (missionNet.IsUsing == false)
            {
                missionNet.SetUsing(true); // Set IsUsing true [Command] 
            }
            else
            {
                Debug.Log("Other Player Using this mission");
                return;
            }
        }

        KPDisableManager.instance.DisablePlayer(true);
        missionCanvas.SetActive(true);
    }

    //미션창 비활성화
    public void CloseMission()
    {
        if (missionNet != null)
        {
            missionNet.SetUsing(false);
        }

        KPDisableManager.instance.DisablePlayer(false);
        missionCanvas.SetActive(false);
    }

    void ValidStageClear()
    {
        //탈출구랑 연동할 부분
        GameMgr.MissionClear();
    }
    public void CodeChange(string num)
    {
        switch(num)
        {
            case "1":
                ChangeNum(1, 1);
                break;
            case "2":
                ChangeNum(1, 0);
                break;
            case "3":
                ChangeNum(2, 1);
                break;
            case "4":
                ChangeNum(2, 0);
                break;
            case "5":
                ChangeNum(3, 1);
                break;
            case "6":
                ChangeNum(3, 0);
                break;
            case "7":
                ChangeNum(4, 1);
                break;
            case "8":
                ChangeNum(4, 0);
                break;
        }
    }

    private void ChangeNum(int n, int calc)
    {
        int num = 0;
        num = int.Parse(state[n].text);
        if(calc == 0)
        {
            num--;
        }
        else 
        {
            num++;
        }
        if(0 < num && num < 10)
        {
            state[n].text = num.ToString();
        }
        else if(num == -1)
        {
            num = 9;
            state[n].text = num.ToString();
        }
        else
        {
            num = 0;
            state[n].text = num.ToString();
        }
    }

    //GameMgr 스크립트의 GenerateMissionCode를 실행해 미션에 사용할 코드를 배열에 저장함
    public void SetStageCode()
    {
        int length = 4;
        correctCode = GameMgr.GeneratePassword(length);
        randomCode = GameMgr.GeneratePassword(length);
        state[0].text = correctCode;
        state[1].text = randomCode[0].ToString();
        state[2].text = randomCode[1].ToString();
        state[3].text = randomCode[2].ToString();
        state[4].text = randomCode[3].ToString();
        Debug.Log(correctCode);
        Debug.Log(randomCode);
    }
}
