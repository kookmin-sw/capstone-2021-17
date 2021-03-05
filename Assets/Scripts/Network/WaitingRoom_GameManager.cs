using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitingRoom_GameManager : MonoBehaviour
{
    public List<GameObject> playerBackgroundList = new List<GameObject>();
    public Button readyBtn;
    public Button startBtn;
    public Button BackBtn;

    private NetManager netManager;

    private void Start()
    {
        netManager = NetManager.instance;        
    }

    



}
