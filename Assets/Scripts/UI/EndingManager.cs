using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.Events;

public class EndingManager : MonoBehaviour
{

    public static EndingManager instance;
    public TMP_Text[] nicknamesUI;
    public TMP_Text[] endstatesUI;

    public List<EndingPlayerManager> endingPlayerManagers;
    public List<Teleporter> teleporters;

    public List<string> EndingNames;
    public List<PlayerEndingState> EndingStates;

    public GameObject EndingCanvas;

    public UnityEvent OnChangeEndingSceneObject;

    void Awake()
    {
        instance = this;
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

        if (!isDead)
        {
            teleporters[idx].gameObject.SetActive(true);
            teleporters[idx].FadeOut(1);
            endingPlayerManagers[idx].gameObject.SetActive(true);
            endingPlayerManagers[idx].Nickname_text.text = name;
        }
        else
        {
            EndingNames.Add(name);
            EndingStates.Add(PlayerEndingState.Dead);
        }

        if (EndingCanvas.activeSelf)
        {
            ShowEndUI();
        }

        idx++;
    }

    public void UpdateWarpPlayer(string name)
    {
        EndingNames.Add(name);
        EndingStates.Add(PlayerEndingState.Escape);
        ShowEndUI();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowEndUI()
    {
        EndingCanvas.SetActive(true);

        for (int i =0; i< EndingNames.Count; i++)
        {
            nicknamesUI[i].text = EndingNames[i];

            PlayerEndingState endingState = EndingStates[i];

            if(endingState == PlayerEndingState.Escape)
            {
                endstatesUI[i].text = "<color=green> Success </color>";
            }else if(endingState == PlayerEndingState.Dead || endingState == PlayerEndingState.Disconnected)
            {
                endstatesUI[i].text = "<color=red> Failed </color>";
            }
            else // Status None - Not Died , Not Escaped
            {
                endstatesUI[i].text = "<color=grey> ?? </color>";
            }
        }
    }

    void UpdateOwnEnding()
    {
        string PlayerName = PlayerPrefs.GetString("PlayerName");
        teleporters[idx].gameObject.SetActive(true);
        teleporters[idx].FadeOut(1);
        endingPlayerManagers[idx].gameObject.SetActive(true);
        endingPlayerManagers[idx].isLocalPlayer = true;
        endingPlayerManagers[idx].Nickname_text.text = PlayerName;
        endingPlayerManagers[idx].PlayAudio();

        idx++;
    }
    
}

