using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MapItemBehaviour : MonoBehaviour
{
    [SerializeField]
    ItemBoxNetBehaviour itemBoxNet;

    public void SetActive(bool isActive)
    {
        itemBoxNet.SetActive(isActive);
    }


    
}
