using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/* 이 스크립트는 GamePlayer에 네트워크 기능을 다룹니다.
 * 
 */ 


public class NetGamePlayer : NetworkBehaviour
{
    [SyncVar]
    public int Health;

    [SyncVar]
    public string Nickname;

    [SyncVar]
    public bool isLeader;

    [SyncVar]
    public ThirdPersonCharacter.State State;

    [SerializeField]
    private ThirdCamera thirdCamera;
    [SerializeField]
    private ThirdPersonCharacter character;
    [SerializeField]
    private ThirdPersonUserControl control;
    [SerializeField]
    private PlayerHealth playerHealth;

    private void Awake()
    {
        Health = playerHealth.Health;
        State = character.state;
    }

    public override void OnStartClient()
    {
        InGame_MultiGameManager.Players.Add(this);

        if (isLocalPlayer)
        {
            if (thirdCamera.gameObject != null)
            {
                thirdCamera.gameObject.SetActive(true);
            }

            keypadSystem.KPDisableManager.instance.player = this.gameObject;
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

    [Client]
    public void ClientChangeHealth(int h)
    {
        Health = h;
    }

    [Client]
    public void ClientChangeState(ThirdPersonCharacter.State state)
    {
        State = state;
    }



    

    
}
