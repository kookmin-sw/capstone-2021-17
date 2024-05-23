using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChangeController : MonoBehaviour
{
    private Mission3Controller missionController;
    void Awake()
    {
        missionController = this.GetComponent<Mission3Controller>();
        missionController.SetMissionTime();
    }
    public void ButtonPressString(string keyString)
    {
        if(keyString == "1")
        {
            missionController.TimeDownCoFast();
        }
        else if(keyString == "2")
        {
            missionController.StopCo();
        }
        else if (keyString == "3")
        {
            missionController.TimeUpCoSlow();
        }
        else if (keyString == "4")
        {
            missionController.TimeUpCoFast();
        }
    }
}
