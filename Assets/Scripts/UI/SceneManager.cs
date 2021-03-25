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
    public static void ChangeWaitingRoomScene()
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
    }

}
