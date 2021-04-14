using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] Items = new Item[4];

    public SlotManager SlotManager; // Assigned in NetGamePlayer OnStartClient

    

    private void Awake()
    {
        SlotManager = SlotManager.instance;
        if (SlotManager != null)
        {
            SlotManager.inventory = this;
        }
        else
        {
            Debug.LogError("SlotManager not detected! - PlayerInventory");
        }
    }


    public bool AddItem(Item newItem)
    {
        for(int idx = 0; idx < Items.Length; idx++)
        {
            if(Items[idx] == null)
            {
                Items[idx] = newItem;
                newItem.OwnedPlayer = this.gameObject;

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
        else if(targetItem.GetType().Name == "HealPack")
        {
            HealPack healPack = targetItem.GetComponent<HealPack>();
            if (healPack.CanUse())
            {
                healPack.Use();
                RemoveItem(idx);
            }
        }
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
