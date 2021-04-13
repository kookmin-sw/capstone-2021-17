using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemNetBehaviour))]
public abstract class Item : MonoBehaviour
{

    public NetGamePlayer OwnedPlayer; // owner

    [SerializeField]
    protected ItemNetBehaviour itemNet;

    private void Awake()
    {
        if (itemNet == null)
        {
            itemNet = GetComponent<ItemNetBehaviour>();
        }
    }

    public abstract void Use();

    public void DestroyObj()
    {
        itemNet.SetActive(false, gameObject);
    }
    
    

    
}
