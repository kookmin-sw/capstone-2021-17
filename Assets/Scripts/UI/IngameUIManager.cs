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
    public GameObject reCheck_UI;

    public Slider missionCount; // 미션 게이지바
    public Text clearText;
    public Text missionText;
    public Text checktext;
    
    bool gameClear;
    float loadingTime = 5; 
    private int check_state; // 게임 종료인지, 중간 종료인지 등 상태 체크용 int
    
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
        //if(Input.GetKeyDown(KeyCode.Escape) && !GameMgr.lockKey)
        if(Input.GetKeyDown(KeyCode.F1) && !GameMgr.lockKey) //esc키로 메뉴창 열고닫기
        {//에디터 오류 방지위해 임시 f1키
            if(menu_UI.activeSelf == false)
            {
                menu_UI.SetActive(true);
                ingame_sound.SetActive(false);
                ingame_howto[0].SetActive(false);
                ingame_howto[1].SetActive(false);
                ingame_quit.SetActive(false);
                menuinfo.SetActive(true);
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
                    SetCheckText(2);
                }
                else if(Input.GetKeyDown(KeyCode.X))
                {
                    SetCheckText(3); // 게임 종료하기
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
        GameMgr mg = GameMgr.instance;
        float missionProgress;
        if (mg.GetMissionSpawnPoint() != 0) // Fixed DivideByZero Error
        {
            missionProgress = ((float)mg.GetMissionClearCount() / mg.GetMissionSpawnPoint());
        }
        else
        {
            missionProgress = 0;
        }
        
        if (missionProgress > 1)
        {
            missionProgress = 1; // 100% 안넘어가게끔
        }
        missionCount.value = missionProgress;
        missionText.text = (int)(missionProgress*100) + "%";
    }

    //게임 종료시 다시 물어보는 UI (ex : 정말로 나가시겠습니까?)
    public void SetCheckText(int check)
    {
        reCheck_UI.SetActive(true);
        if(menu_UI.activeSelf==true)
        {
            menu_UI.SetActive(false);
        }
        

        // 1 : 로비
        // 2 : 시작창
        // 3 : 종료
        switch (check)
        {
            case 1:
                checktext.text = "로비로 돌아갈 경우, 현재 방의 플레이어들과 다시 게임을 진행하게 됩니다.\n로비로 돌아가시겠습니까?\n(Y/N)";
                MoveScene(1);
                break;
            case 2:
                checktext.text = "시작창으로 돌아갈 경우, 새로운 방을 찾아 게임을 진행해야 합니다.\n시작창으로 돌아가시겠습니까?\n(Y/N)";
                MoveScene(2);
                break;
            case 3:
                checktext.text = "게임이 완전히 종료됩니다. 종료하시겠습니까?\n(Y/N)";
                MoveScene(3);
                break;
            default:
                checktext.text = "정말로 게임에서 나가시겠습니까?\n(Y/N)";
                MoveScene(2);
                break;
        }
    }
    private void MoveScene(int scene)
    {//입력된 int 값에 따라 scene 이동 (로비 : 1 / 시작 : 2, 종료 : 3)
        if(reCheck_UI.activeSelf==true)
        {
            if(Input.GetKeyDown(KeyCode.Y))
            {
                if(scene==1)
                {
                    SceneManager.ChangeLobbyScene();
                }
                else if(scene==2)
                {
                    SceneManager.ChangeStartScene();
                }
                else if(scene==3)
                {
                    SceneManager.GameExit();
                }
                
            }
            else if(Input.GetKeyDown(KeyCode.N))
            {
                reCheck_UI.SetActive(false);
            }
        }//버튼 안먹는 오류 고쳐야함
    }
}
