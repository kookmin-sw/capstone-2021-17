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
        LoadingManager.LoadScene("Start");
    }
    public void ChangeWaitingRoomScene(){
        LoadingManager.LoadScene("Loading");
    }
    public void ChangeOpeningScene(){
        LoadingManager.LoadScene("Opening");
    }
    public void ChangeGamePlayScene(){
        LoadingManager.LoadScene("GamePlay");
    }

    //버튼 스크립트
    //게임 종료
    public void GameExit(){
        Debug.Log("Game Exit");
        Application.Quit();
    }
    //버튼 onoff
    public void OnOff(){
        
        if(HowTo.activeSelf==true){
            HowTo.SetActive(false);
            Name.SetActive(true);
        }
        else{
            HowTo.SetActive(true);
            Name.SetActive(false);
        }

    }

}
