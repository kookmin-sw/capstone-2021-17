using UnityEditor;
using UnityEngine;


public class PlayerBehaviourOnItem : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ActOnItem(Item item)
    {
        if(item is HealPack)
        {
            if (playerHealth.health >= PlayerHealth.MAXHP)
            {
                Debug.Log("아이템 사용 불가 - 체력이 가득찬 상태");
                return;
            }
            else
            {
                UseHealPack();
            }
        }
        
    }
    

    public void UseHealPack()
    {
        playerHealth.Heal();
    }

}
