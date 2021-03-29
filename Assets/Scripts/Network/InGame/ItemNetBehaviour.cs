using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ItemNetBehaviour : NetworkBehaviour
{
    [SerializeField] 
    private GameObject Item;

    public override void OnStartClient()
    {
        if(Item == null)
        {
            Item = gameObject;
        }
    }

    public void SetActive(bool isActive, GameObject target = null)
    {
        CmdSetActive(isActive, target);
    }


    [Command(requiresAuthority = false)]
    private void CmdSetActive(bool isActive, GameObject target)
    {
        RpcSetActive(isActive, target);
    }

    [ClientRpc]
    private void RpcSetActive( bool isActive, GameObject target)
    {
        if (target == null) 
        { 
            Item.SetActive(isActive); 
        }

        else
        {
            target.SetActive(isActive);
        }

        
    }
}
