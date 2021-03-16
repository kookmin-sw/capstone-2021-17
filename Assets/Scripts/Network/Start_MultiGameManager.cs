using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
 * NetManager를 이용합니다
 * StartScene에서의 GameObject를 다루는 Manager 객체입니다 
 */
public class Start_MultiGameManager: MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField addressField;

    private NetManager netManager;

    private void Start() // Awake할때는 instance 못부름
    {
        netManager = NetManager.instance;
    }

    public void SaveAddress()
    {
        string address = addressField.text;
        if (address == "") address = "localhost";
        netManager.networkAddress = address;
    }
    public void SaveNickName()
    {
        PlayerPrefs.SetString("nickname", nameField.text);
    }


    //클라이언트로 시작
    public void JoinRoom()
    {
        SaveAddress();
        SaveNickName();


        netManager.StartClient();
    }

    //호스트로 시작 (추후에 방 만드는 기능이 생기면 변화가 필요함)
    public void CreateRoom()
    {
        SaveNickName();
        netManager.StartHost();
    }


}
