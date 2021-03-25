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

    
    public ThirdCamera ThirdCamera;
    
    public ThirdPersonCharacter Character;
    
    public ThirdPersonUserControl ThirdControl;
    
    public PlayerHealth PlayerHealth;

    private void Awake()
    {
        Health = PlayerHealth.Health;
        State = Character.state;
    }

    public override void OnStartClient()
    {
        InGame_MultiGameManager.Players.Add(this);

        if (isLocalPlayer)
        {
            if (ThirdCamera.gameObject != null)
            {
                ThirdCamera.gameObject.SetActive(true);
            }

            keypadSystem.KPDisableManager.instance.player = this.gameObject;
        }
    }
    

    [ClientCallback]
    public void MoveCharacter(Vector3 move, bool crouch, bool jump)
    {
        if (isLocalPlayer)
        {
            Character.Move(move, crouch, jump);
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
