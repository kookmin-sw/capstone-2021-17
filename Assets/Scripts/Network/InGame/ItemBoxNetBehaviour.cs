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

    [Command(requiresAuthority = false)]
    private void CmdSetUsing(bool isUsing)
    {
        IsUsing= isUsing;
    }

    public void OpenBox()
    {
        CmdOpenBox();
    }

    [Command(requiresAuthority = false)]
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
    
    [Command(requiresAuthority = false)]
    private void CmdUnableKeypad()
    {
        RpcUnableKeypad();
    }

    [ClientRpc]
    private void RpcUnableKeypad()
    {
        keypadController.UnableKeypad();
    }

    [SerializeField]
    GameObject item;

    public void SetActive(bool isActive)
    {
        CmdSetActive(isActive);
    }


    [Command(requiresAuthority = false)]
    private void CmdSetActive(bool isActive)
    {
        RpcSetActive(isActive);
    }

    [ClientRpc]
    private void RpcSetActive(bool isActive)
    {
        item.SetActive(isActive);
    }



}
