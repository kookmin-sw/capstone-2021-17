using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumChangeController : MonoBehaviour
{
    private Mission2Controller missionController;
    void Awake()
    {
        missionController = this.GetComponent<Mission2Controller>();
        missionController.SetStageCode();
    }

    public void ButtonPress(string keyString)
    {
        if (!missionController.firstClick)
        {
            missionController.firstClick = true;
        }
        missionController.CodeChange(keyString);
        missionController.CheckCode();
        missionController.firstClick = false;
    }
}
