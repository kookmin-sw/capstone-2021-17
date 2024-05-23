using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class Mission1Controller : MonoBehaviour
{
    //ĵ����
    [SerializeField] private GameObject missionCanvas;
    //������ ������Ʈ
    [SerializeField] private GameObject missionObject;
    //��Ʈ��ũ
    [SerializeField] private MissionNetBehaviour missionNet;
    //�Է¹�����
    public string codeText;
    //����� ������ ��
    public string[] correctCode;
    //�������� Ŭ���� Ȯ��
    public bool[] clearStage;
    //�Է¹��� ���� ����
    public int correctString;
    //�������� �ܰ�
    private int stage;
    //������ ������ �ܰ�
    public int maxStage;
    //�Է¹����� ����Ȯ�ο�
    public bool firstClick;
    //��ư�� �����ϴ� �迭
    public Button[] button;
    //Button[num]���� �����ư���� Ȯ�ο�
    private int num;
    //���� ���¸� ǥ���� �ؽ�Ʈ
    public Text state;


    public void CheckStageCode()
    {
        //��ư�� ����� ��� ���ڸ��� ����
        if (codeText == correctCode[stage].Substring(correctString,1))
        {
            correctString++;
            if(correctString == correctCode[stage].Length)
            {
                state.text = "Correct";
                state.gameObject.SetActive(true);
                clearStage[stage] = true;
                stage++;
                correctString = 0;
                StartCoroutine("ShowQuiz");
            }
        }
        //Ʋ���� �� �ܰ� ó������ �ٽ�
        else if (codeText != correctCode[stage].Substring(correctString, 1))
        {
            state.text = "Error";
            state.gameObject.SetActive(true);
            correctString = 0;
            StartCoroutine("ShowQuiz");
        }

        if (clearStage[maxStage-1]==true)
        {
            StopCoroutine("ShowQuiz");
            state.text = "Clear";
            if (missionNet != null)
            {
                missionNet.UnableMission();
            }
            else
            {
                UnableMission();
            }
            ValidStageClear();

            //1�ʵ� â ����
            Invoke("CloseMission", 1.0f);
        }
    }

    //�̼ǿ�����Ʈ ��ȣ�ۿ� ����
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
        StartCoroutine("ShowQuiz");
    }

    //�̼�â ��Ȱ��ȭ
    public void CloseMission()
    {
        if (missionNet != null)
        {
            missionNet.SetUsing(false);
        }
        StopCoroutine("ShowQuiz");
        //stage = 0;
        KPDisableManager.instance.DisablePlayer(false);
        missionCanvas.SetActive(false);
    }

    void ValidStageClear()
    {
        GameMgr.instance.MissionClear();
    }

    //GameMgr ��ũ��Ʈ�� GenerateMissionCode�� ������ �̼ǿ� ����� �ڵ带 �迭�� ������
    public void SetStageCode()
    {
        clearStage = new bool[maxStage];
        correctCode = new string[maxStage];
        int length = 0;
        for (int i = 0; i < maxStage; i++)
        {
            length = i + 4;
            correctCode[i] = GameMgr.GenerateMissionCode(length);
            //Debug.Log(correctCode[i]);
        }
    }

    //ShowQuiz���� ��� ��ư�� ��Ȱ��ȭ�ϰ� ��Ȱ��ȭ�� ������ �����ϴ� ���
    public void ButtonTurnYellow()
    {
        //Debug.Log(num);
        ColorBlock colors = button[num].colors;
        colors.disabledColor = new Color32(255, 255, 0, 255);
        button[num].colors = colors;
        //Debug.Log("color change Y");
    }
    public void ButtonTurnGray()
    {
        //Debug.Log(num);
        ColorBlock colors = button[num].colors;
        colors.disabledColor = new Color32(200, 200, 200, 128);
        button[num].colors = colors;
        //Debug.Log("color change G");
    }

    //��ư ��Ȱ��ȭ
    public void ButtonFalse()
    {
        for(int i=0; i<9; i++)
        {
            button[i].interactable = false;
        }
    }

    //��ư Ȱ��ȭ
    public void ButtonActive()
    {
        for (int i = 0; i < 9; i++)
        {
            button[i].interactable = true;
        }
    }


    //��ư�� ������ �ٲٴ� ������� �Է��� �ڵ� ������ ������
    //����Ǵ� ���� ��ư ��Ȱ��ȭ�� ���� �Ϸ��� ��ư Ȱ��ȭ
    IEnumerator ShowQuiz()
    {
        //Debug.Log("ShowQuiz Start");
        int length = stage + 4;
        ButtonFalse();
        for (int i = 0; i < length; i++)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            state.gameObject.SetActive(false);
            num = int.Parse(correctCode[stage].Substring(i, 1)) - 1;
            Invoke("ButtonTurnYellow", 0f);
            Invoke("ButtonTurnGray", 0.15f);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        ButtonActive();
    }
}
