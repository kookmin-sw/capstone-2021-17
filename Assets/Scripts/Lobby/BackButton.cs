using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    private NetManager netManager;

    private void Start()
    {
        netManager = NetManager.instance;
    }

    public void PressBack()
    {
        //game stop
        if (netManager.roomSlots[0].isClientOnly)
        {
            netManager.StopClient();
        }
        else
        {
            netManager.StopHost();
        }
        
    }
}
