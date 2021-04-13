using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public Item[] Items = new Item[4];

    [SerializeField]
    private NetGamePlayer netPlayer;

    public bool AddItem(Item newItem)
    {
        for(int idx = 0; idx < Items.Length; idx++)
        {
            if(Items[idx] == null)
            {
                Items[idx] = newItem;
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
            healPack.Use();

            RemoveItem(idx);
        }


    }

    public void RemoveItem(Item targetItem)
    {
        for (int idx = 0; idx < Items.Length; idx++)
        {
            if (Items[idx] == targetItem)
            {
                Items[idx] = null;
            }
        }
    }

    public void RemoveItem(int idx)
    {
        if(Items[idx] != null)
        {
            Items[idx] = null;
        }
    }

    public bool IsFullInventory()
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
