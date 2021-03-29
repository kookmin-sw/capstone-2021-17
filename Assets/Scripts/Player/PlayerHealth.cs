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
    bool IsHit;

    Animator PlayerAnimator;

    [SerializeField]
    private NetGamePlayer NetPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Player=GameObject.Find("Chibi_boy");
        PlayerHealthCheck=Player.GetComponent<ThirdPersonUserControl>();
        PlayerHealthState=Player.GetComponent<ThirdPersonCharacter>();
        PlayerAnimator = GetComponent<Animator>();
        HealthChange();
    }

    // Update is called once per frame
    void Update()
    {
       // print(Health);
    }

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
        print(PlayerHealthCheck.HealthCheck);
    }
}
