using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    GameObject testPlayer;
    GameObject healPack;
    // Heal test nono
    PlayerHealth playerHealth;

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
            DestroyItem();
            Debug.Log("아이템 사용");
        }
        else
        {
            Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
        }
    }

    void DestroyItem()
    {
        healPack.SetActive(false);
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
