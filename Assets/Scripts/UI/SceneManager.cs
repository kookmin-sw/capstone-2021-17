using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameObject HowTo;
    public GameObject Name;
    bool onoff;
    bool onoff2;
    //Start, WaitingRoom,Opening, GamePlay
    public void ChangeStartScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
    public void ChangeWaitingRoomScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("WaitingRoom");
    }
    public void ChangeOpeningScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Opening");
    }
    public void ChangeGamePlayScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }

    //버튼 스크립트
    //게임 종료
    public void GameExit(){
        Debug.Log("Game Exit");
        Application.Quit();
    }
    //버튼 onoff
    public void OnOff(){
        
        onoff = !onoff;
        onoff2 = !onoff2;
        HowTo.SetActive(onoff);
        Name.SetActive(onoff2);

    }

}
