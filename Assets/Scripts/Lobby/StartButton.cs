using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartButton : MonoBehaviour
{
    private NetManager netManager;

    private void Awake()
    {
        netManager = NetManager.instance;
        netManager.startBtn = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public void PressStart()
    {
        //game start!
        netManager.ServerChangeScene(netManager.GameplayScene);
    }
    
    


}
