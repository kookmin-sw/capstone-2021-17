using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour 
{
    public Image[] slot = new Image[4];
    public Sprite[] sprites = new Sprite[2];
    private Sprite thisImage;
    private bool[] isEmpty = new bool[4];

    public static SlotManager instance;

    public PlayerInventory inventory;


	void Awake () 
    {
        for(int i=0; i<4; i++){
            isEmpty[i] = true;
        }
        instance = this;
    }

    void Update()
    {
        if(inventory == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // 1
        {
            inventory.UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 2
        {
            inventory.UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // 3
        {
            inventory.UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // 4
        {
            inventory.UseItem(3);
        }
    }
    public void AddItem(Item item)
    {
        for (int i=0;i<4 ; i++)
        {
            if (isEmpty[i] == true)
            {
                SetThisImage(item);
                slot[i].sprite = thisImage;
                isEmpty[i] = false;
                break;
            }
            else
            {
                continue;
            }
        }
    }
    public void AddItem(int i, Item item)
    {
        if (isEmpty[i] == true)
        {
            SetThisImage(item);
            slot[i].sprite = thisImage;
            isEmpty[i] = false;
        }
    }

    void SetThisImage(Item item)
    {
        string itemName = item.GetType().Name;
        if(itemName == "HealPack")
        {
            thisImage = sprites[0];
        }
    }

    //아이템 삭제
    public void RemoveItem()
    {
        for(int i=3; i>=0; i--)
        {
            if(isEmpty[i]==false)
            {
                slot[i].sprite = null;
                isEmpty[i] = true;
                break;
            }
            else
            {
                continue;
            }
        }
    }
    public void RemoveItem(int i)
    {
        if (isEmpty[i] == false)
        {
            slot[i].sprite = null;
            isEmpty[i] = true;
        }
    }
}