using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemNetBehaviour))]
public abstract class Item : MonoBehaviour
{
    private GameObject ownedPlayer; // owner
    public GameObject OwnedPlayer
    {
        get
        {
            return ownedPlayer;
        }
        set
        {
            ownedPlayer = value;
            OnPlayerOwnItem();
        }
    }

    [SerializeField]
    protected ItemNetBehaviour itemNet;

    private void Awake()
    {
        if (itemNet == null)
        {
            itemNet = GetComponent<ItemNetBehaviour>();
        }
    }

    public abstract bool CanUse();
    public abstract void Use();

    public void DestroyObj()
    {
        itemNet.SetActive(false, gameObject);
    }

    public virtual void OnPlayerOwnItem()
    {
        return;
    }



}
