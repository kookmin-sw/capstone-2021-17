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

    private GamePlay_MultiGameManager gameManager;

    private void Awake()
    {
        gameManager = GamePlay_MultiGameManager.instance;
    }

    private void Start()
    {
        gameObject.GetComponent<Transform>().position = gameManager.GetSpawnPoint().position;
    }
    private void FixedUpdate()
    {
        Debug.Log(gameObject.GetComponent<Transform>().position);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        identity = GetComponent<NetworkIdentity>();
        
        
    }

    public void MoveCharacter(Vector3 move, bool crouch, bool jump)
    {
        if (isLocalPlayer)
        {
            CmdMoveCharacter(move, crouch, jump);
        }
            
    }

    [Command] // Server에서 실행하게끔 하는 attribute
    public void CmdMoveCharacter(Vector3 move, bool crouch, bool jump)
    {

        character.Move(move, crouch, jump);
    }
}
