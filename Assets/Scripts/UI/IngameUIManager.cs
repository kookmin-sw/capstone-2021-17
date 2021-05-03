using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    public GameObject ingame_quit; // Qxit 창 열기
    public GameObject ingame_sound; //사운드 조절 창 열기
    public GameObject[] ingame_howto; //how to 창 열기
    public GameObject menu_UI;
    public GameObject game_clear;
    public GameObject menuinfo;

    public Slider missionCount; // 미션 게이지바
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

        OpenMenu();
        MissionCount();
    }

        //게임 클리어시 활성화
    private void GameClear() // 4명이 전부 클리어 할 시 게임 클리어됨
    {
        int clearPlayer_count = 0;
        int serverPlayer = 4;
        if(clearPlayer_count==serverPlayer)
        {
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

        //메뉴창 스크립트
    private void OpenMenu()
    {   
        //메뉴창 열고닫기
        if(Input.GetKeyDown(KeyCode.Escape) && !GameMgr.lockKey) //esc키로 메뉴창 열고닫기
        {
            if(menu_UI.activeSelf == false)
            {
                menu_UI.SetActive(true);
            }
            else
            {
                menu_UI.SetActive(false);
            }
        }

        //메뉴탕이 열려있을 때 액션
        if(menu_UI.activeSelf == true)
        {   //볼륨조절창
            if(Input.GetKeyDown(KeyCode.S))
            {
                if(ingame_sound.activeSelf==false)
                {
                    ingame_sound.SetActive(true);
                    ingame_quit.SetActive(false);
                    ingame_howto[0].SetActive(false);
                    ingame_howto[1].SetActive(false);
                    menuinfo.SetActive(false);
                }
                else
                {
                    ingame_sound.SetActive(false);
                }
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {//게임종료 선택창
                if(ingame_quit.activeSelf==false)
                {
                    ingame_quit.SetActive(true);
                    ingame_sound.SetActive(false);
                    ingame_howto[0].SetActive(false);
                    ingame_howto[1].SetActive(false);
                    menuinfo.SetActive(false);
                }
                else
                {
                    ingame_quit.SetActive(false);
                }
            }

            else if(Input.GetKeyDown(KeyCode.H))
            {//How to 창 열기
                if(ingame_howto[0].activeSelf==false)
                {
                    ingame_howto[0].SetActive(true);
                    ingame_quit.SetActive(false);
                    ingame_sound.SetActive(false);
                    ingame_howto[1].SetActive(false);
                    menuinfo.SetActive(false);
                }
                else
                {
                    ingame_howto[0].SetActive(false);
                    ingame_howto[1].SetActive(false);
                }
            }

            if(ingame_quit.activeSelf==true)
            {
                if(Input.GetKeyDown(KeyCode.Z)) //시작창으로 돌아가기
                {
                    SceneManager.ChangeStartScene();
                }
                else if(Input.GetKeyDown(KeyCode.X))
                {
                    SceneManager.GameExit(); // 게임 종료하기
                }
            }

            if(ingame_howto[0].activeSelf==true)
            {
                if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    ingame_howto[0].SetActive(false);
                    ingame_howto[1].SetActive(true);
                }
            }
            else if(ingame_howto[1].activeSelf==true)
            {
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ingame_howto[0].SetActive(true);
                    ingame_howto[1].SetActive(false);
                }
            }
        }
    }

    //미션 수행도 진행바
    private void MissionCount()
    {
        GameMgr mg = new GameMgr();
        missionCount.value = mg.GetMissionClearCount();
        //총 미션 갯수 구해야함
    }
}
