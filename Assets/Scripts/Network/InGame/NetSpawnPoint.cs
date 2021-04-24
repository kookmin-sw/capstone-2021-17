using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Awake할 시 NetManager에게 시작점위치를 보내줍니다

public class NetSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.RegisterStartPosition(this.transform);
    }
}
