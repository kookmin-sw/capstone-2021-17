using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Awake�� �� NetManager���� ��������ġ�� �����ݴϴ�

public class NetSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.RegisterStartPosition(this.transform);
    }
}
