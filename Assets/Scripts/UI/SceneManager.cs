using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameObject[] obj;

    //Start, WaitingRoom,Opening, GamePlay
    public static void ChangeStartScene()
    {
        LoadingManager.LoadScene("Start");
    }
    public static void ChangeLoadingScene()
    {
        LoadingManager.LoadScene("Loading");
    }
    public static void ChangeOpeningScene()
    {
        LoadingManager.LoadScene("Opening");
    }
    public static void ChangeGamePlayScene()
    {
        LoadingManager.LoadScene("GamePlay");
    }
    public static void ChangeLobbyScene()
    {
        LoadingManager.LoadScene("WaitingRoom");//로비씬으로 이동(네트워크 연동 필요할 수 있음)
    }

    //게임 종료
    public static void GameExit()
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

        GameNameShow();
    }

    public void ShowOnly(int index)
    {
        for(int i=0;i< obj.Length; i++)
        {
            if(i == index)
            {
                obj[i].SetActive(true);
            }
            else
            {
                obj[i].SetActive(false);
            }
        }
    }

    public void ShowLoadingInfo()
    {
        ShowOnly(5);
    }

    public void ShowServerFail()
    {
        ShowOnly(6);
    }

    public void ShowMainMenu()
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (i == 1 || i== 2)
            {
                obj[i].SetActive(true);
            }
            else
            {
                obj[i].SetActive(false);
            }
        }
    }

    public void GameNameShow()
    {
        bool isGameNameShow = true;
        for(int i=0; i < obj.Length; i++)
        {
            if (i == 1 || i==2) continue;

            if(obj[i].activeSelf)
            {
                isGameNameShow = false;
            }
        }

        if (isGameNameShow)
        {
            obj[1].SetActive(true);
        }
        else
        {
            obj[1].SetActive(false);
        }
    }


}
