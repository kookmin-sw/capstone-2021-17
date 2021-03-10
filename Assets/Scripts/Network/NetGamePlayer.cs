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

        identity = GetComponent<NetworkIdentity>();
    }
}
