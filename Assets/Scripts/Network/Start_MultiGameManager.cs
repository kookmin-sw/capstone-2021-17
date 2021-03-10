using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void JoinRoom()
    {
        SaveAddress();
        SaveNickName();


        netManager.StartClient();
    }
    public void CreateRoom()
    {
        SaveNickName();
        netManager.StartHost();
    }


}
