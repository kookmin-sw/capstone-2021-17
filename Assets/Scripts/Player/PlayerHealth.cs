using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ThirdPersonUserControl))]
public class PlayerHealth : MonoBehaviour
{
    public int Health = 2;
    bool IsHit;

    Animator PlayerAnimator;

    [SerializeField]
    private NetGamePlayer NetPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
        PlayerAnimator = GetComponent<Animator>();
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
            NetPlayer.ClientChangeHealth(Health);
        }
        if(Health==1)
        {
            GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
            GameObject.FindWithTag("Player").GetComponent<ThirdPersonCharacter>().IsHit=true;            
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
            NetPlayer.ClientChangeHealth(Health);
        }
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonCharacter>().IsHit=false;
    }

    public void Die()
    {
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonCharacter>().IsDie=true;

    }
}
