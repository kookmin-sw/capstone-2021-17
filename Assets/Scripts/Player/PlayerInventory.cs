using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] Items = new Item[4];



    public GameObject[] HandItems = new GameObject[2]; // 0 Heal, 1 Gun

    [HideInInspector]
    public SlotManager SlotManager; // Assigned in NetGamePlayer OnStartClient


    [SerializeField]
    private NetGamePlayer netPlayer;

    [SerializeField]
    private PlayerHealth playerHealth;

    
    private NetworkManager netManager;

    public GameObject HealPackPrefab;
    public GameObject GunPrefab;

    private Animator playerAnimator;
    private GunControl gunControl;

    private void Awake()
    {
        netManager = NetworkManager.singleton;
        playerAnimator = GetComponent<Animator>();
        gunControl = HandItems[1].GetComponent<GunControl>();

    }


    public bool AddItem(Item newItem)
    {
        for(int idx = 0; idx < Items.Length; idx++)
        {
            if(Items[idx] == null)
            {
                Items[idx] = newItem;

                if (SlotManager != null)
                {
                    SlotManager.AddItem(idx , newItem);
                }
                return true;
            }
        }

        return false;
    }

    public void UseItem(int idx)
    {
        Item targetItem = Items[idx];

        if (targetItem == null)
        {
            // nothing works..
        }
        else if(targetItem is HealPack)
        {
            if (playerHealth.health >= PlayerHealth.MAXHP)
            {
                Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
                return;
            }
            else
            {
                playerHealth.Heal();
                RemoveItem(idx);
                netPlayer.ChangeHandItem = false;
            }
            
        }
        else if (targetItem is Gun)
        {
            //gunControl.Shoot();
            //netPlayer.ChangeHandItem = false;

        }
    }
    public void DropItem(int idx)
    {
        Item targetItem = Items[idx];

        Vector3 position = transform.position;
        position += transform.forward;

        Quaternion rotation = Quaternion.identity;
        

        if (targetItem == null)
        {
            // nothing works..
        }
        else if (targetItem is HealPack healPack)
        {
            netPlayer.SpawnObject(healPack, position , rotation);
            RemoveItem(idx);
        }
        else if (targetItem is Gun gun)
        {
            netPlayer.SpawnObject(gun, position, rotation);
            RemoveItem(idx);

            DeactivateGun();
        }

    }
    public void DeactivateHealPack()
    {
        Debug.Log("DEAC");
        netPlayer.ChangeHandItem = true;
        netPlayer.SetActiveHandItem(null);
        
    }


    public void DeactivateGun()
    {
        playerAnimator.SetBool("Gun", false); //gun animation end
        gunControl.Head.SetActive(true);
        gunControl.CountBullet = 0;
        netPlayer.ChangeHandItem = true;
        netPlayer.SetActiveHandItem(null);
        
    }

    public void ActiveHandItem(int idx)
    {
        Item targetItem = Items[idx];
        netPlayer.SetActiveHandItem(targetItem);

        if(targetItem is Gun)
        {
            playerAnimator.SetBool("Gun", true); //gun animation start
        }
    }

    public void RemoveItem(Item targetItem)
    {
        for (int idx = 0; idx < Items.Length; idx++)
        {
            RemoveItem(idx);
            return;
        }
    }

    public void RemoveItem(int idx)
    {
        if(Items[idx] != null)
        {

            
            //netPlayer.SetActiveHandItem(null);
            
            Items[idx] = null;
            
            SlotManager.RemoveItem(idx);
        }
    }

    public bool IsFull()
    {
        for (int idx = 0; idx < Items.Length; idx++)
        {
            if(Items[idx] == null)
            {
                return false;
            }
        }
        return true;
    }

    
}
