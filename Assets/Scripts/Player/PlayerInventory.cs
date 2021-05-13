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

    private void Awake()
    {


        netManager = NetworkManager.singleton;

        
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
            }
            
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

    }

    public void ActiveHandItem(int idx)
    {
        Item targetItem = Items[idx];

        if (targetItem == null)
        {
            netPlayer.CmdSetActiveHandItem(-1);
        }
        else if (targetItem is HealPack)
        {
            netPlayer.CmdSetActiveHandItem(0);
        }   
    }

    public void ActiveHandItemFromNet(int idx, bool isActive)
    {
        HandItems[idx].SetActive(isActive);
    }

    public void RemoveItem()
    {
        for(int idx = 3; idx >= 0; idx--)
        {
            if(Items[idx] != null)
            {
                Items[idx] = null;
                SlotManager.RemoveItem(idx);
            }
        }
    }

    public void RemoveItem(Item targetItem)
    {
        for (int idx = 0; idx < Items.Length; idx++)
        {
            if (Items[idx] == targetItem)
            {
                Items[idx] = null;
                SlotManager.RemoveItem(idx);
                return;
            }
        }
    }

    public void RemoveItem(int idx)
    {
        if(Items[idx] != null)
        {

            
            netPlayer.SetActiveHandItem(null);
            
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
