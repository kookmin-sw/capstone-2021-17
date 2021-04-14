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
        GetInput();
        PickUpGun();
    }
    void GetInput()
    {
        pDown = Input.GetButtonDown("PickUp");
    }
    void PickUpGun(){
        if (pDown&&nearObject!=null) 
        {
            if(nearObject.name =="Gun")
            {
                hasWeapons[0]=true;
                PlayerAnimator.SetBool("Gun", hasWeapons[0]);
                Weapons[0].SetActive(true);
            }
            else
            {

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
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Gun")
        {
            nearObject=null;
        }
    }
}
