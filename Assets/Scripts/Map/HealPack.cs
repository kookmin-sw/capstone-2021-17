using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Item
{
    GameObject testPlayer;
    GameObject healPack;
    // Heal test nono
    PlayerHealth playerHealth;

    [SerializeField]
    ItemNetBehaviour itemNet;

    private void Awake()
    {
        if(itemNet == null)
        {
            itemNet = GetComponent<ItemNetBehaviour>();
        }
    }

    public void UseHealPack()
    {
        //playerHealth = testPlayer.GetComponent<HealTest>();
        NetGamePlayer netPlayer = testPlayer.GetComponent<NetGamePlayer>();
        if (netPlayer == null)
        {
            Debug.LogWarning("Player Don't have NetGamePlayer Script - Heeun An");
        }
        playerHealth = netPlayer.PlayerHealth;
        HealPlayer();
    }

    void HealPlayer()
    {
        if (playerHealth.Health < PlayerHealth.MAXHP)
        {
            playerHealth.Heal();
            Destroy();
            Debug.Log("아이템 사용");
        }
        else
        {
            Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
        }
    }

    public void Destroy()
    {
        itemNet.SetActive(false , healPack);
    }

    public void SetHealObject(GameObject gameObj)
    {
        healPack = gameObj;
    }

    public void SetPlayerObject(GameObject gameObj)
    {
        testPlayer = gameObj;
    }
}
