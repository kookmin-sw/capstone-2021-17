using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameObject[] obj;
    public Text clearText;
    bool gameClear;
    float loadingTime = 5; 


    void Start()
    {
        loadingTime = 5f;
        gameClear = false;
    }
    void Update()
    {
        if(gameClear)
        {
            ClearTimeCount();    
        }
    }
    //Start, WaitingRoom,Opening, GamePlay
    public void ChangeStartScene()
    {
        LoadingManager.LoadScene("Start");
    }
    public void ChangeWaitingRoomScene()
    {
        LoadingManager.LoadScene("Loading");
    }
    public void ChangeOpeningScene()
    {
        LoadingManager.LoadScene("Opening");
    }
    public void ChangeGamePlayScene()
    {
        LoadingManager.LoadScene("GamePlay");
    }

    //버튼 스크립트
    //게임 종료
    public void GameExit()
    {
        Debug.Log("Game Exit");
        Application.Quit();
    }
    //버튼 : 켜져있는 경우엔 끄고, 꺼진 경우엔 켜기
    public void OnOff(int i)
    {
        if(obj[i].activeSelf==true)
        {
            obj[i].SetActive(false);
        }
        else
        {
            obj[i].SetActive(true);
        }
    }
    //게임 클리어시 활성화
    public void GameClear()
    {
        gameClear = true;
    }
    //게임 클리어시 자동으로 로비 이동
    private void ClearTimeCount()
    {   
        loadingTime -= Time.deltaTime;
        clearText.text = "잠시 후 메인 화면으로 이동합니다.\n" + Mathf.Round(loadingTime) + "초...";
        
        if(loadingTime<=0)
        {
            ChangeStartScene();
        }
    }

}
