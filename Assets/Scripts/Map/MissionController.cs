using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class MissionController : MonoBehaviour
{
    //캔버스
    [SerializeField] private GameObject missionCanvas;
    //연동할 오브젝트
    [SerializeField] private GameObject missionObject;
    //입력받은값
    public string codeText;
    //저장된 퀴즈의 답
    public string[] correctCode;
    //스테이지 클리어 확인
    public bool[] clearStage;
    //입력받은 값의 길이
    public int correctString;
    //스테이지 단계
    private int stage;
    //설정할 마지막 단계
    public int maxStage;
    //입력받은값 변동확인용
    public bool firstClick;
    //버튼을 저장하는 배열
    public Button[] button;
    //Button[num]으로 어느버튼인지 확인용
    private int num;
    //현재 상태를 표기할 텍스트
    public Text state;


    public void CheckStageCode()
    {
        //버튼과 저장된 퀴즈를 한자리씩 비교함
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
        //틀리면 그 단계 처음부터 다시
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
            UnableMission();
            //ValidStageClear();

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
        KPDisableManager.instance.DisablePlayer(true);
        missionCanvas.SetActive(true);
        StartCoroutine("ShowQuiz");
    }

    //미션창 비활성화
    public void CloseMission()
    {
        stage = 0;
        KPDisableManager.instance.DisablePlayer(false);
        missionCanvas.SetActive(false);
    }

    void ValidStageClear()
    {
        //탈출구랑 연동할 부분
    }

    //GameMgr 스크립트의 GenerateMissionCode를 실행해 미션에 사용할 코드를 배열에 저장함
    public void SetStageCode()
    {
        clearStage = new bool[maxStage];
        correctCode = new string[maxStage];
        int length = 0;
        for (int i = 0; i < maxStage; i++)
        {
            length = i * 2 + 3;
            correctCode[i] = GameMgr.GenerateMissionCode(length);
            Debug.Log(correctCode[i]);
        }
    }

    //ShowQuiz에서 사용 버튼을 비활성화하고 비활성화시 색상을 변경하는 방식
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

    //버튼 비활성화
    public void ButtonFalse()
    {
        for(int i=0; i<9; i++)
        {
            button[i].interactable = false;
        }
    }

    //버튼 활성화
    public void ButtonActive()
    {
        for (int i = 0; i < 9; i++)
        {
            button[i].interactable = true;
        }
    }


    //버튼의 색상을 바꾸는 방식으로 입력할 코드 순서를 보여줌
    //실행되는 동안 버튼 비활성화됨 실행 완료후 버튼 활성화
    IEnumerator ShowQuiz()
    {
        Debug.Log("ShowQuiz Start");
        int length = stage * 2 + 3;
        ButtonFalse();
        for (int i = 0; i < length; i++)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            state.gameObject.SetActive(false);
            num = int.Parse(correctCode[stage].Substring(i, 1)) - 1;
            Invoke("ButtonTurnYellow", 0f);
            Invoke("ButtonTurnGray", 0.15f);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        ButtonActive();
    }
}
