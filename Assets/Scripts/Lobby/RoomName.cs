using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using MasterServerToolkit.MasterServer;

public class RoomName : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))]
    public string Name;

    public TMP_Text textUI;

    private void OnNameChanged(string oldName, string newName)
    {
        textUI.text = newName;
    }

    public override void OnStartServer()
    {
        Name = NetManager.instance.RoomName;
    }

}
