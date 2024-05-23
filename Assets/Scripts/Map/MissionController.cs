using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using keypadSystem;

public class MissionController : MonoBehaviour
{
    [SerializeField] private GameObject missionObject;
    [SerializeField] private GameObject screen;
    [SerializeField] private Renderer screenRenderer;
    [SerializeField] public Mission1Controller mission1Controller { get; private set; }
    [SerializeField] public Mission2Controller mission2Controller { get; private set; }
    [SerializeField] public Mission3Controller mission3Controller { get; private set; }

    void Awake()
    {
        missionObject = transform.parent.gameObject;
        screenRenderer = screen.GetComponent<Renderer>();
        if(missionObject.name == "Mission1(Clone)")
        {
            mission1Controller = gameObject.GetComponent<Mission1Controller>();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller = gameObject.GetComponent<Mission2Controller>();
        }
        if (missionObject.name == "Mission3(Clone)")
        {
            mission3Controller = gameObject.GetComponent<Mission3Controller>();
        }
    }

    //미션창 활성화
    public void ShowMission()
    {
        GameMgr.lockKey = true;
        if (missionObject.name == "Mission1(Clone)")
        {
            mission1Controller.ShowMission();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller.ShowMission();
        }
        if (missionObject.name == "Mission3(Clone)")
        {
            mission3Controller.ShowMission();
        }
    }

    //미션창 비활성화
    public void CloseMission()
    {
        GameMgr.lockKey = false;
        if (missionObject.name == "Mission1(Clone)")
        {
            mission1Controller.CloseMission();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller.CloseMission();
        }
        if (missionObject.name == "Mission3(Clone)")
        {
            mission3Controller.CloseMission();
        }
    }

    public void UnableMission()
    {
        GameMgr.lockKey = false;
        if (missionObject.name == "Mission1(Clone)")
        {
            mission1Controller.UnableMission();
            ScreenColorChange();
        }
        if (missionObject.name == "Mission2(Clone)")
        {
            mission2Controller.UnableMission();
            ScreenColorChange();
        }
        if (missionObject.name == "Mission3(Clone)")
        {
            mission3Controller.UnableMission();
            ScreenColorChange();
        }
    }

    private void ScreenColorChange()
    {
        if (missionObject.layer == 0)
        {
            //Debug.Log("color change");
            screenRenderer.material.color = Color.black;
            screenRenderer.material.SetColor("_EmissionColor", Color.black);
        }
    }
}
