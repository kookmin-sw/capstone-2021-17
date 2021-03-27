using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
public class ItemBoxNetBehaviour : NetworkBehaviour
{
    [SyncVar]
    public bool IsUsing = false;

    [SyncVar]
    public string quizText;

    [SyncVar]
    public string validCode;

    [SerializeField]
    private BoxContoller boxContoller;
    [SerializeField]
    private keypadSystem.KeypadController keypadController;


    public override void OnStartServer()
    {
        keypadController.SetPassword();
        keypadController.GenerateQuiz();
    }
    public override void OnStartClient()
    {
        keypadController.quizInfo.text = quizText;
        keypadController.validCode = validCode;
    }
    public void SetUsing(bool isUsing)
    {
        CmdSetUsing(isUsing);
    }

    [Command(ignoreAuthority = true)]
    private void CmdSetUsing(bool isUsing)
    {
        IsUsing= isUsing;
    }

    public void OpenBox()
    {
        CmdOpenBox();
    }

    [Command(ignoreAuthority = true)]
    private void CmdOpenBox()
    {
        RpcOpenBox();
    }

    [ClientRpc]
    private void RpcOpenBox()
    {
        boxContoller.PlayAnimation();
    }

    public void UnableKeypad()
    {
        CmdUnableKeypad();
    }
    
    [Command(ignoreAuthority = true)]
    private void CmdUnableKeypad()
    {
        RpcUnableKeypad();
    }

    [ClientRpc]
    private void RpcUnableKeypad()
    {
        keypadController.UnableKeypad();
    }



}
