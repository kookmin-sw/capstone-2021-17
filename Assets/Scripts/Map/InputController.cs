using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using keypadSystem;

public class InputController : MonoBehaviour
{
    private MissionController missionController;

    void Awake()
    {
        missionController = this.GetComponent<MissionController>();
        missionController.SetStageCode();
        missionController.ShowMission();
        missionController.StartCoroutine("ShowQuiz");
    }

    public void ButtonPressString(string keyString)
    {
        if (!missionController.firstClick)
        {
            missionController.codeText = string.Empty;
            missionController.firstClick = true;
        }
        missionController.codeText += keyString;
        missionController.CheckStageCode();
        missionController.firstClick = false;
    }

    public void ShutDownMission()
    {
        //KPDisableManager.instance.DisablePlayer(false);
        //missionController.CloseMission();
    }
}
