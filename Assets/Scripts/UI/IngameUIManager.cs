using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    public GameObject ingame_quit;
    public Text clearText;
    bool gameClear;
    float loadingTime = 5; 
    
    void Start()
    {
        loadingTime = 5f;
        gameClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameClear)
        {
            ClearTimeCount();    
        }

        GameQuit();
    }

        //게임 클리어시 활성화
    private void GameClear() // 4명이 전부 클리어 할 시 게임 클리어됨
    {
        int clearPlayer_count = 0;
        int serverPlayer = 4;
        if(clearPlayer_count==serverPlayer){
            gameClear = true;
        }
        
    }
    //게임 클리어시 자동으로 로비 이동
    private void ClearTimeCount()
    {   
        loadingTime -= Time.deltaTime;
        clearText.text = "잠시 후 메인 화면으로 이동합니다.\n" + Mathf.Round(loadingTime) + "초...";
        
        if(loadingTime<=0)
        {
            SceneManager.ChangeStartScene();
        }
    }

        //게임 중간 종료 스크립트
    private void GameQuit()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(ingame_quit.activeSelf == false)
            {
                ingame_quit.SetActive(true);
            }
            else
            {
                ingame_quit.SetActive(false);
            }
        }

        if(ingame_quit.activeSelf == true)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.ChangeStartScene();
            }
            else if(Input.GetKeyDown(KeyCode.G))
            {
                SceneManager.GameExit();
            }
        }
    }
}
