using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using keypadSystem;
public class PlayerHealth : MonoBehaviour
{
    private MovePlayerTestForEnemy playerHealthState; // A reference to the MovePlayerTestForEnemy on the object
    private PlayerItem playerHealItem;                // A reference to the  PlayerItem on the object
    private Animator playerAnimator;                  //Animation

    public const int MAXHP = 2;  //Maximum health setting
    public int health = 2;       //Current health setting

    [SerializeField]
    private NetGamePlayer NetPlayer;

    void Start()
    {
        playerHealthState=GetComponent<MovePlayerTestForEnemy>();
        playerHealItem = GetComponent<PlayerItem>();
        playerAnimator = GetComponent<Animator>();
    }

    public void Hit() //Player hit
    {
        if (NetPlayer.isLocalPlayer)
        {
            KPDisableManager.instance.DisablePlayer(false);
        }
        

        health -= 1; // Health minus
        if (NetPlayer != null) //Net Player
        {
            NetPlayer.ChangeHealth(health);
        }
        if(health == 1) //If Health is 1, player hit
        {
            playerHealthState.isHit=true;
        }
        else if (health == 0) //If Health is 0, player die
        {
            Die();           
        }
    }
    public void Heal() //Player heal
    {
        //You can heal only when health is 1
        if (health == 1) 
        {
            StartCoroutine("Healing");
            playerHealthState.isHit = false;
        }
    }
    
    public void Die() //Player is dead
    {
        playerHealthState.isDie=true;
        playerAnimator.SetBool("Die", true);
        ChangeLayersRecursively(transform, "Interact");
        //gameObject.layer = 8;           //layer �����ؼ� ��ֹ��� �Ǵ��ϰ� ��
    }

    
    bool isHitRoutine = false;

    void OnTriggerEnter(Collider other)//When entering the trigger
    {
        //When the tag is attack
        if (other.CompareTag("Attack"))
        {
            if(isHitRoutine)
            {
                return; // 
            }
            else
            {
                isHitRoutine = true;
                StartCoroutine("Hiting");
            }
            //It is attacked continuously. So use coroutines.

        }
    }
    IEnumerator Hiting() //Set Hiting Coroutine
    {
        playerHealthState.isHeal = false;
        playerAnimator.SetBool("Heal", false);
        Hit();
        yield return new WaitForSeconds(1); //
        isHitRoutine = false;
    }
    IEnumerator Healing() //Set Healing Coroutine
    {
        playerHealthState.isHeal = true;
        health += 1; // Health plus
        if (NetPlayer != null)
        {
            NetPlayer.ChangeHealth(health);
            NetPlayer.PlayerInventory.RemoveHealPack();
        }
        yield return new WaitForSeconds(2f); //Until the animation ends
        if (playerHealItem != null)
        {
            playerHealItem.hasItems[1] = false; //Do not have an item
            playerHealItem.Items[1].SetActive(false); //Deactivate items in the user's hand
        }
        playerHealthState.isHeal = false;      
    }

    public static void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }

}
