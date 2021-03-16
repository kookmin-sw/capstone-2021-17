using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>();

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        Add("heal", "체력 1칸 회복", ItemType.Consumption);
    }    
	
void Add(string itemName, string itemDesc, ItemType itemType)
    {
        items.Add(new Item(itemName,itemDesc, itemType,Mokup.Load<Sprite>("ItemImages/" + itemName)));
    }
}