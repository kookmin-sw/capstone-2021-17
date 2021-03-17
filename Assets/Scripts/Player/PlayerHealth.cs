using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ThirdPersonUserControl))]
public class PlayerHealth : MonoBehaviour
{
    public int Health = 2;
    bool IsHit;

    Animator PlayerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
    }

    // Update is called once per frame
    void Update()
    {
        print(Health);
    }

    public void Hit()
    {
        Health -= 1;
        if(Health==0)
        {
            Die();
        }
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
        //IsHit=true;

       // PlayerAnimator.SetBool("Hit", IsHit);

    }

    public void Heal()
    {
        Health += 1;
        GameObject.FindWithTag("Player").GetComponent<ThirdPersonUserControl>().HealthCheck=Health;
    }

    public void Die()
    {
        Health=0;

    }
}
