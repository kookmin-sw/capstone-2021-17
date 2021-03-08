using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetGamePlayer : NetworkBehaviour
{
    private NetworkIdentity identity;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // 시작했을 때 뭘 할까
    }
}
