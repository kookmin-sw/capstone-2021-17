using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Item
{
    private PlayerHealth playerHealth;

    public override void Use()
    {
        playerHealth = OwnedPlayer.PlayerHealth;
        HealPlayer();
    }

    void HealPlayer()
    {
        if (playerHealth.Health < PlayerHealth.MAXHP)
        {
            playerHealth.Heal();
            Debug.Log("아이템 사용");
        }
        else
        {
            Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
        }
    }
}
