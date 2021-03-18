using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{
    GameObject testPlayer;
    GameObject healPack;
    HealTest playerHealth;

    public void UseHealPack()
    {
        testPlayer = GameObject.Find("test_player");
        playerHealth = testPlayer.GetComponent<HealTest>();
        HealPlayer();
    }

    void HealPlayer()
    {
        if (playerHealth.playerHP < playerHealth.maxHP)
        {
            playerHealth.Heal();
            DestroyItem();
            Debug.Log("아이템 사용");
        }
        else
        {
            Debug.Log("아이템 사용 불가");
        }
    }

    void DestroyItem()
    {
        healPack.SetActive(false);
    }

    public void GetHealObject(GameObject gameObj)
    {
        healPack = gameObj;
    }
}
