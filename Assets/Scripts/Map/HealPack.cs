using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : Item
{
    private PlayerHealth playerHealth;

    public override bool CanUse()
    {
        if (playerHealth.health >= PlayerHealth.MAXHP)
        {
            Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
            return false;
        }
        return true;
    }

    public override void Use()
    {
        playerHealth.Heal();
    }

    public override void OnPlayerOwnItem()
    {
        playerHealth = OwnedPlayer.GetComponent<PlayerHealth>();
    }


}
