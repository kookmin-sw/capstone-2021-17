using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class MissionController : MonoBehaviour
{
    [SerializeField] private GameObject missionObject;
    [SerializeField] private Mission1Controller mission1Controller;
    [SerializeField] private Mission2Controller mission2Controller;
    void Awake()
    {
        missionObject = transform.parent.gameObject;
        if(missionObject.name == "Mission1(Clone)")
        {
            mission1Controller = gameObject.GetComponent<Mission1Controller>();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller = gameObject.GetComponent<Mission2Controller>();
        }
    }

    //미션오브젝트 상호작용 제거
    public void UnableMission()
    {
        if (missionObject.name == "Mission1(Clone)")
        {
            mission1Controller.UnableMission();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller.UnableMission();
        }
    }

    //미션창 활성화
    public void ShowMission()
    {
        if (missionObject.name == "Mission1(Clone)")
        {
            mission1Controller.ShowMission();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller.ShowMission();
        }
    }

    //미션창 비활성화
    public void CloseMission()
    {
        if (missionObject.name == "Mission1(Clone)")
        {
            mission1Controller.CloseMission();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller.CloseMission();
        }
    }
}
