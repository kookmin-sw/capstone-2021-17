using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Events;

public class EndingManager : MonoBehaviour
{

    public static EndingManager instance;
    public Text[] nickname;
    public Text gameClear;

    public List<EndingPlayerManager> endingPlayerManagers;
    public List<Teleporter> teleporters;

    public List<string> PlayersName;
    public List<bool> PlayersIsDead;

    public GameObject EndingCanvas;

    public UnityEvent OnChangeEndingSceneObject;


    private bool isclear; //게임 클리어 여부

    void Awake()
    {
        instance = this;

        isclear = true;
    }

    public void DisconnectRoom()
    {
        if (NetworkManager.singleton)
        {
            NetworkManager.singleton.StopClient(); // 시작으로 돌아감
        }
    }

    public void StartEnding()
    {
        foreach (var audio in GameObject.FindObjectsOfType<AudioSource>())
        {
            audio.volume = 0;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UIFadeInOut uiFadeInOut = UIFadeInOut.instance;

        UpdateOwnEnding();
        uiFadeInOut.OnFadeOut.AddListener(ChangeToEnding);
        uiFadeInOut.FadeoutandIn(0.5f, 0.5f, 0.3f);
    }
    void ChangeToEnding()
    {
        OnChangeEndingSceneObject.Invoke();
        
    }

    private int idx = 0;
    public void UpdateEnding(string name, bool isDead)
    {
        Debug.Log("UPDATE");

        if (!isDead)
        {
            teleporters[idx].gameObject.SetActive(true);
            teleporters[idx].FadeOut(1);
            endingPlayerManagers[idx].gameObject.SetActive(true);
        }

        idx++;
    }

    void UpdateOwnEnding()
    {

        Debug.Log("UPDATE OWN");
        string PlayerName = PlayerPrefs.GetString("PlayerName");
        teleporters[idx].gameObject.SetActive(true);
        teleporters[idx].FadeOut(1);
        endingPlayerManagers[idx].gameObject.SetActive(true);
        endingPlayerManagers[idx].isLocalPlayer = true;

        idx++;
    }
    
}

