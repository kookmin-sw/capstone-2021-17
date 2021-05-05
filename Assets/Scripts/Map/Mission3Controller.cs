using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class Mission3Controller : MonoBehaviour
{
    //캔버스
    [SerializeField] private GameObject missionCanvas;
    //연동할 오브젝트
    [SerializeField] private GameObject missionObject;
    //네트워크
    [SerializeField] private MissionNetBehaviour missionNet;
    //미션에 사용될 답
    public string correctTime;
    //미션 캔버스에 띄울 텍스트
    public Text stateTime;
    public Text hintTime;
    //미션 클리어 여부
    private bool clearMission = false;
    //미션 답의 시간
    public int correctHour;
    public int correctMinute;
    public int correctSecond;
    //미션에서 변경되는 시간
    public int hour;
    public int minute;
    public int second;
    //코루틴 제어에 사용
    public bool[] corutineRunning = { false };

    //미션 오브젝트 비활성화
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

    public void ChangeTimeUp()
    {
        if (second == 60)
        {
            second = 0;
            if (minute == 60)
            {
                minute = 0;
                hour += 1;
            }
            else
            {
                minute += 1;
            }
        }
        else
        {
            second += 1;
        }
        stateTime.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
    }
    public void ChangeTimeDown()
    {
        if (second == 0)
        {
            second = 59;
            if (minute == 0)
            {
                minute = 59;
                hour -= 1;
            }
            else
            {
                minute -= 1;
            }
        }
        else
        {
            second -= 1;
        }
        stateTime.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
    }

    IEnumerator ChangeTimeDownFast()
    {
        corutineRunning[0] = true;
        yield return new WaitForSecondsRealtime(0.05f);
        ChangeTimeDown();
        StartCoroutine("ChangeTimeDownFast");
    }
    IEnumerator ChangeTimeUpSlow()
    {
        corutineRunning[1] = true;
        yield return new WaitForSecondsRealtime(0.5f);
        ChangeTimeUp();
        StartCoroutine("ChangeTimeUpSlow");
    }

    IEnumerator ChangeTimeUpFast()
    {
        corutineRunning[2] = true;
        yield return new WaitForSecondsRealtime(0.05f);
        ChangeTimeUp();
        StartCoroutine("ChangeTimeUpFast");
    }

    public void TimeUpCoSlow()
    {
        if(corutineRunning[0])
        {
            StopCoroutine("ChangeTimeDownFast");
            corutineRunning[0] = false;
        }
        if (corutineRunning[2])
        {
            StopCoroutine("ChangeTimeUpFast");
            corutineRunning[2] = false;
        }
        StartCoroutine("ChangeTimeUpSlow");
    }
    public void TimeUpCoFast()
    {
        if (corutineRunning[0])
        {
            StopCoroutine("ChangeTimeDownFast");
            corutineRunning[0] = false;
        }
        if (corutineRunning[1])
        {
            StopCoroutine("ChangeTimeUpSlow");
            corutineRunning[1] = false;
        }
        StartCoroutine("ChangeTimeUpFast");
    }
    public void TimeDownCoFast()
    {
        if (corutineRunning[1])
        {
            StopCoroutine("ChangeTimeUpSlow");
            corutineRunning[1] = false;
        }
        if (corutineRunning[2])
        {
            StopCoroutine("ChangeTimeUpFast");
            corutineRunning[2] = false;
        }
        StartCoroutine("ChangeTimeDownFast");
    }
    public void StopCo()
    {
        if (corutineRunning[1])
        {
            StopCoroutine("ChangeTimeUpSlow");
            corutineRunning[1] = false;
        }
        if (corutineRunning[2])
        {
            StopCoroutine("ChangeTimeUpFast");
            corutineRunning[2] = false;
        }
        if (corutineRunning[0])
        {
            StopCoroutine("ChangeTimeDownFast");
            corutineRunning[0] = false;
        }
        if(correctHour == hour && correctMinute == minute && correctSecond == second)
        {
            ValidStageClear();
            UnableMission();
        }
    }
    void ValidStageClear()
    {
        GameMgr.instance.MissionClear();
    }
    public void SetMissionTime()
    {
        Debug.Log("setTime");
        int randomTime = Random.Range(200, 300);
        int correctTimeNum = 0;
        int randomTimeNum = 0;
        int hours = 0;
        int mins = 0;
        int secs = 0;
        //정답 시간 생성
        correctTime = GameMgr.GenerateMissionTime();
        //텍스트에 넣어줄 시간 설정
        correctTimeNum = int.Parse(correctTime);
        randomTimeNum = correctTimeNum - randomTime;
        hours = Mathf.FloorToInt(correctTimeNum / 3600);
        correctTimeNum = correctTimeNum % 3600;
        mins = Mathf.FloorToInt(correctTimeNum / 60);
        correctTimeNum = correctTimeNum % 60;
        secs = correctTimeNum;
        hintTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, mins, secs);
        correctHour = hours;
        correctMinute = mins;
        correctSecond = secs;

        hours = Mathf.FloorToInt(randomTimeNum / 3600);
        randomTimeNum = randomTimeNum % 3600;
        mins = Mathf.FloorToInt(randomTimeNum / 60);
        randomTimeNum = randomTimeNum % 60;
        secs = randomTimeNum;
        stateTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, mins, secs);
        hour = hours;
        minute = mins;
        second = secs;
    }
}
