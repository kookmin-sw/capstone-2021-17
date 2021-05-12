using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ThirdPersonUserControl))]
public class PlayerHealth : MonoBehaviour
{
    public const int MAXHP = 2;
    public int Health = 2;
    private GameObject Player;
    private ThirdPersonUserControl PlayerHealthCheck;
    private ThirdPersonCharacter PlayerHealthState;
    private PlayerItem PlayerHealItem;
    Animator PlayerAnimator;

    public GameObject HealPortion;
    public int CountHeal;

    [SerializeField]
    private NetGamePlayer NetPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Player=GameObject.Find("Chibi_boy");
        PlayerHealthCheck=Player.GetComponent<ThirdPersonUserControl>();
        PlayerHealthState=Player.GetComponent<ThirdPersonCharacter>();
        PlayerHealItem = Player.GetComponent<PlayerItem>();
        PlayerAnimator = GetComponent<Animator>();
        HealthChange();
    }
    /*
    void Update()
    {
        //왼쪽 마우스 버튼을 클릭하면 & 힐아이템이 활성화가 되면 힐을 한다. 
        if (Input.GetMouseButtonUp(0) && HealPortion.activeSelf == true)
        {
            StartCoroutine("Healing");
        }
    }
    */
    public void Hit()
    {
        Health -= 1;

        if(NetPlayer != null)
        {
            NetPlayer.ChangeHealth(Health);
        }
        if(Health==1)
        {
            HealthChange();
            PlayerHealthState.IsHit=true;            
        }
        else if (Health==0)
        {
            Die();
        }        
    }

    public void Heal()
    {
        Health += 1;
        if (NetPlayer != null)
        {
            NetPlayer.ChangeHealth(Health);
        }
        HealthChange();
        PlayerHealthState.IsHit=false;
    }

    public void Die()
    {
        HealthChange();
        PlayerHealthState.IsDie=true;

    }
    public void HealthChange()
    {
        PlayerHealthCheck.HealthCheck=Health;
    }
    IEnumerator Healing()
    {
        PlayerAnimator.SetBool("Heal", true);
        Heal();
        yield return new WaitForSeconds(4);
        PlayerHealItem.hasWeapons[1] = false;
        PlayerHealItem.Weapons[1].SetActive(false);
        PlayerAnimator.SetBool("Heal", false);
    }
}
