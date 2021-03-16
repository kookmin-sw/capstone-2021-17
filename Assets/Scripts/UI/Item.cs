using UnityEngine;

public enum ItemType
{
    Equipment,//장비
    Consumption//소모
}

[System.Serializable]
public class Item { //이름, 설명, 타입, 이미지

    public string itemName;
    public string itemDesc;
    public ItemType itemType;
    public Sprite itemImage;

    public Item(string _itemName, string _itemDesc, ItemType _itemType, Sprite _itemImage)
    {
        itemName = _itemName;
        itemDesc = _itemDesc;
        itemType = _itemType;
        itemImage = _itemImage;
    }

    public Item()
    {

    }
}