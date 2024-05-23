using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class Mission3Controller : MonoBehaviour
{
    //ĵ����
    [SerializeField] private GameObject missionCanvas;
    //������ ������Ʈ
    [SerializeField] private GameObject missionObject;
    //��Ʈ��ũ
    [SerializeField] private MissionNetBehaviour missionNet;
    //�̼ǿ� ���� ��
    public string correctTime;
    //�̼� ĵ������ ��� �ؽ�Ʈ
    public Text stateTime;
    public Text hintTime;
    //�̼� Ŭ���� ����
    private bool clearMission = false;
    //�̼� ���� �ð�
    public int correctHour;
    public int correctMinute;
    public int correctSecond;
    //�̼ǿ��� ����Ǵ� �ð�
    public int hour;
    public int minute;
    public int second;
    //�ڷ�ƾ ��� ���
    public bool[] corutineRunning = { false };
    //��ư�� �����ϴ� �迭
    public Button[] button;

    //�̼� ������Ʈ ��Ȱ��ȭ
    public void UnableMission()
    {
        missionObject.tag = "Untagged";
        missionObject.layer = 0;
    }
    //�̼�â Ȱ��ȭ
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

    //�̼�â ��Ȱ��ȭ
    public void CloseMission()
    {
        if (missionNet != null)
        {
            missionNet.SetUsing(false);
        }
        KPDisableManager.instance.DisablePlayer(false);
        missionCanvas.SetActive(false);
    }
    //�ð� ���� �Լ���
    public void ChangeTimeUp()
    {
        if (second == 59)
        {
            second = 0;
            if (minute == 59)
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
    //��ư�� ���� �ð����� �ڷ�ƾ
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
        yield return new WaitForSecondsRealtime(0.01f);
        ChangeTimeUp();
        StartCoroutine("ChangeTimeUpFast");
    }

    public void TimeUpCoSlow()
    {
        if(corutineRunning[0])
        {
            StopCoroutine("ChangeTimeDownFast");
            corutineRunning[0] = false;
            button[0].interactable = true;
        }
        if (corutineRunning[2])
        {
            StopCoroutine("ChangeTimeUpFast");
            corutineRunning[2] = false;
            button[3].interactable = true;
        }
        button[2].interactable = false;
        StartCoroutine("ChangeTimeUpSlow");
    }
    public void TimeUpCoFast()
    {
        if (corutineRunning[0])
        {
            StopCoroutine("ChangeTimeDownFast");
            corutineRunning[0] = false;
            button[0].interactable = true;
        }
        if (corutineRunning[1])
        {
            StopCoroutine("ChangeTimeUpSlow");
            corutineRunning[1] = false;
            button[2].interactable = true;
        }
        button[3].interactable = false;
        StartCoroutine("ChangeTimeUpFast");
    }
    public void TimeDownCoFast()
    {
        if (corutineRunning[1])
        {
            StopCoroutine("ChangeTimeUpSlow");
            corutineRunning[1] = false;
            button[2].interactable = true;
        }
        if (corutineRunning[2])
        {
            StopCoroutine("ChangeTimeUpFast");
            corutineRunning[2] = false;
            button[3].interactable = true;
        }
        button[0].interactable = false;
        StartCoroutine("ChangeTimeDownFast");
    }
    //�ð� ������ ���߰� ��� ��ġ�ϴ��� ���մϴ�
    public void StopCo()
    {
        if (corutineRunning[1])
        {
            StopCoroutine("ChangeTimeUpSlow");
            corutineRunning[1] = false;
            button[2].interactable = true;
        }
        if (corutineRunning[2])
        {
            StopCoroutine("ChangeTimeUpFast");
            corutineRunning[2] = false;
            button[3].interactable = true;
        }
        if (corutineRunning[0])
        {
            StopCoroutine("ChangeTimeDownFast");
            corutineRunning[0] = false;
            button[0].interactable = true;
        }
        if(correctHour == hour && correctMinute == minute && correctSecond == second)
        {
            ButtonFalse();
            stateTime.text = "Mission Clear!";
            ValidStageClear();
            if (missionNet != null)
            {
                missionNet.UnableMission();
            }
            else
            {
                UnableMission();
            }
            Invoke("CloseMission", 1.0f);
        }
    }

    bool stageClearValid = true; // ��ư �ι� ������� Clear�� �ѹ��� Invoke �ϰԲ�
    //�̼�Ŭ���� ī��Ʈ ����
    void ValidStageClear()
    {
        if (stageClearValid)
        {
            GameMgr.instance.MissionClear();
            stageClearValid = false; 
        }
    }

    //�̼� Ŭ������ ��ư�� �������� �Լ�
    public void ButtonFalse()
    {
        for (int i = 0; i < 4; i++)
        {
            button[i].interactable = false;
        }
    }

    public void SetMissionTime()
    {
        //Debug.Log("setTime");
        int randomTime = Random.Range(200, 300);
        int correctTimeNum = 0;
        int randomTimeNum = 0;
        int hours = 0;
        int mins = 0;
        int secs = 0;
        //���� �ð� ����
        correctTime = GameMgr.GenerateMissionTime();
        //�ؽ�Ʈ�� �־��� �ð� ����
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
