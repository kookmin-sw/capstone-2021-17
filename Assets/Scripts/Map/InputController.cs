using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using keypadSystem;

public class InputController : MonoBehaviour
{
    private Mission1Controller missionController;

    void Awake()
    {
        missionController = this.GetComponent<Mission1Controller>();
        missionController.SetStageCode();
        //missionController.ShowMission();
        //missionController.StartCoroutine("ShowQuiz");
    }

    public void ButtonPressString(string keyString)
    {
        //��ư�� �ڵ带 ���ڸ��� ���ϱ����� �߰��Ѱ�
        if (!missionController.firstClick)
        {
            missionController.codeText = string.Empty;
            missionController.firstClick = true;
        }
        missionController.codeText += keyString;
        missionController.CheckStageCode();
        missionController.firstClick = false;
    }
}
