using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.RegisterStartPosition(this.transform);
    }
}
