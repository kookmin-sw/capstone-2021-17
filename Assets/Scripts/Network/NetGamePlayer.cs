using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/* 이 스크립트는 GamePlayer에 네트워크 기능을 다룹니다.
 * 
 */ 


public class NetGamePlayer : NetworkBehaviour
{

    public ThirdCamera thirdCamera;
    public ThirdPersonCharacter character;
    public ThirdPersonUserControl control;
    public NetworkIdentity identity;

    
    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            if (thirdCamera.gameObject != null)
            {
                thirdCamera.gameObject.SetActive(true);
            }
           
        }
    }
    

    [ClientCallback]
    public void MoveCharacter(Vector3 move, bool crouch, bool jump)
    {
        if (hasAuthority && isLocalPlayer)
        {
            character.Move(move, crouch, jump);
        }
    }
}
