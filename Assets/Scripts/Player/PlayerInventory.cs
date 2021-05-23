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

    int lastUsedItemIdx = -1;

    public void UseItem(int idx)
    {
        Item targetItem = Items[idx];

        if (targetItem == null)
        {
            return;
        }

        lastUsedItemIdx = idx;

        if (targetItem is HealPack)
        {
            if (playerHealth.health >= PlayerHealth.MAXHP)
            {
                Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
                return;
            }
            else
            {
                netPlayer.IsItemUsing = true;
                playerHealth.Heal();
            } 
        }
        else if (targetItem is Gun)
        {
            netPlayer.IsItemUsing = true;
            gunControl.Shoot();
          
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
            DeActivateHealPack();
            RemoveItem(idx);
        }
        else if (targetItem is Gun gun)
        {
            netPlayer.SpawnObject(gun, position, rotation);
            DeActivateGun();
            RemoveItem(idx);
        }

    }

    public void DeActivateHealPack()
    {
        netPlayer.SetActiveHandItem(null);
    }

    public void DeActivateGun()
    {
        playerAnimator.SetBool("Gun", false); //gun animation end
        gunControl.Head.SetActive(true);
        gunControl.CountBullet = 0;

        netPlayer.SetActiveHandItem(null);
    }
    public void RemoveHealPack()
    {
        if (netPlayer.IsItemUsing == false) return;
        netPlayer.IsItemUsing = false;

        DeActivateHealPack();
        RemoveItem(lastUsedItemIdx);
    }


    public void RemoveGun()
    {
        if (netPlayer.IsItemUsing == false) return;
        netPlayer.IsItemUsing = false;

        DeActivateGun();
        RemoveItem(lastUsedItemIdx);
        
    }

    public void ActiveHandItem(int idx)
    {
        Item targetItem = Items[idx];
        netPlayer.SetActiveHandItem(targetItem);

        if(targetItem is Gun)
        {
            playerAnimator.SetBool("Gun", true); //gun animation start
        }
        else
        {
            playerAnimator.SetBool("Gun", false); //gun animation start
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
