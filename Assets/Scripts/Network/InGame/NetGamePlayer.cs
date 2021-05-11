using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/* 이 스크립트는 GamePlayer에 네트워크 기능을 다룹니다.
 * 
 */ 


public class NetGamePlayer : NetworkBehaviour
{
    [SyncVar(hook =nameof(OnHealthChanged))]
    public int Health;

    [SyncVar]
    public string Nickname;

    [SyncVar]
    public bool isLeader;

    [SyncVar]
    public ThirdPersonCharacter.State State;

    [SyncVar]
    public bool IsEscape = false;

    public ThirdCamera ThirdCamera;

    public Canvas Canvas;
    
    public ThirdPersonCharacter Character;
    
    public ThirdPersonUserControl ThirdControl;
    
    public PlayerHealth PlayerHealth;

    public PlayerInventory PlayerInventory;

    private InGame_MultiGameManager MultigameManager;

    private void Awake()
    {
        Health = PlayerHealth.Health;
        State = Character.state;
        MultigameManager = InGame_MultiGameManager.instance;
    }

    public override void OnStartClient()
    {
        InGame_MultiGameManager.AddPlayer(this);

        if (isLocalPlayer)
        {
            if(MultigameManager.DebugIntroCam != null)
            {
                MultigameManager.DebugIntroCam.gameObject.SetActive(false);
            }
            else // == null
            {
                Debug.Log("Intro Cam is not detected - InGame_MultiGameManager  , heeun an");
            }

            if (ThirdCamera.gameObject != null)
            {
                ThirdCamera.gameObject.SetActive(true);               
            }

            if (Canvas.gameObject != null)
            {
                Canvas.gameObject.SetActive(true);
            }
            keypadSystem.KPDisableManager disableManager = keypadSystem.KPDisableManager.instance;
            disableManager.player = this.gameObject;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    public override void OnStopClient()
    {
        InGame_MultiGameManager.DisablePlayer(this);
    }
    public string GetConnState()
    {
        return ""+ Math.Round(NetworkTime.rtt * 1000)+"ms";
    }
    
    public void ChangeHealth(int h)
    {
        if (isLocalPlayer) // Authority Check
        {
            CmdChangeHealth(h);
        } 
    }
    
    public void ChangeState(ThirdPersonCharacter.State state)
    {
        if (isLocalPlayer) // Authority Check
        {
            CmdChangeState(state);
        }
    }

    public void PlaySound()
    {
        CmdPlaySound();
    }

    public void StopSound()
    {
        CmdStopSound();
    }

    public void PlaySoundOneShot(int soundId)
    {
        CmdPlaySoundOneShot(soundId);
    }
    [ClientCallback]
    public void MoveCharacter(Vector3 move, bool crouch, bool jump)
    {
        if (isLocalPlayer) // Authority Check
        {
            Character.Move(move, crouch, jump);
        }
    }

    [Command]
    private void CmdChangeHealth(int h)
    {
        Health = h;
    }


    [Command]
    private void CmdChangeState(ThirdPersonCharacter.State state)
    {
        State = state;
    }

    [Command]
    private void CmdPlaySound()
    {
        RpcPlaySound();
    }

    [ClientRpc]
    private void RpcPlaySound()
    {

    }

    [Command]
    private void CmdPlaySoundOneShot(int soundId)
    {
        RpcPlaySoundOneShot(soundId);
    }

    [ClientRpc]
    private void RpcPlaySoundOneShot(int soundId)
    {
        //사용 안해서 삭제 
    }

    [Command]
    private void CmdStopSound()
    {
        RpcStopSound();
    }

    [ClientRpc]
    private void RpcStopSound()
    {
        // Character.soundSources-> Character.soundSource
    }

    GameObject spawnPrefab;
    public void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        spawnPrefab = prefab;
        CmdSpawnObject(position, rotation);
    }

    [Command]
    private void CmdSpawnObject(Vector3 position, Quaternion rotation)
    {
        GameObject createdObject = Instantiate(spawnPrefab , position , rotation);
        NetworkServer.Spawn(createdObject);
    }

    void OnHealthChanged(int oldVal, int newVal)
    {
        if(newVal == 0)
        {
            Die();
        }
    }

    
    public void Escape()
    {
        if (isLocalPlayer)
        {
            EndingPlayerMessage msg = new EndingPlayerMessage()
            {
                PlayerName = Nickname,
                endingState = PlayerEndingState.Live
            };

           
            NetworkClient.Send(msg);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");


        }
        else
        {
            IsEscape = true;
            gameObject.SetActive(false);
        }
    }

    public void Die()
    {
        if (isLocalPlayer)
        {
            EndingPlayerMessage msg = new EndingPlayerMessage()
            {
                PlayerName = Nickname,
                endingState = PlayerEndingState.Dead
            };


            NetworkClient.Send(msg);

            //UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }
}
