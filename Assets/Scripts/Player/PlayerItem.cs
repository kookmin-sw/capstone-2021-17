using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public GameObject nearObject;
    public GameObject[] Weapons;
    public bool[] hasWeapons;

    bool pDown;

    Animator PlayerAnimator;

    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        PickUpGun();
    }
    void PickUpGun()
    {
        if (Input.GetKeyDown(KeyCode.E)&&nearObject!=null)
        {
            if(nearObject.name =="Gun")
            {
                hasWeapons[0]=true;
                PlayerAnimator.SetBool("Gun", hasWeapons[0]);
                Weapons[0].SetActive(true);
            }
            else if(nearObject.name =="HealPotion")
            {
                hasWeapons[1]=true;
               // PlayerAnimator.SetBool("Heal", hasWeapons[0]);
                Weapons[1].SetActive(true);
            }
            Destroy(nearObject);
            
        } 
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Gun")
        {
            nearObject=other.gameObject;
        }
        else if(other.name == "HealPotion")
        {
            nearObject=other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Gun")
        {
            nearObject=null;
        }
        else if(other.tag == "HealPotion")
        {
            nearObject=null;
        }
    }
}
