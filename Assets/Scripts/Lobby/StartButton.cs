using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartButton : MonoBehaviour
{
    private NetManager netManager;

    private void Start()
    {
        netManager = NetManager.instance;
        netManager.OnRoomPlayersReady += StartBtnActive;
        netManager.OnRoomPlayersNotReady += StartBtnDeactive;
        //netManager.startBtn = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public void PressStart()
    {
        //game start!
        netManager.ServerChangeScene(netManager.GameplayScene);
    }
    public void StartBtnActive()
    {
        this.gameObject.SetActive(true);
    }
    public void StartBtnDeactive()
    {
        this.gameObject.SetActive(false);
    }




}
