using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public GameObject nearObject; // A reference to the nearobject
    public GameObject[] Items;   //Items placed in the player's hand  , 0=gun 1=healpotion
    public bool[] hasItems;      //Items state bool in the player  , 0=gun 1=healpotion
    public GameObject gun;       //The gun object to fall on the floor
    Animator playerAnimator;     //Reference for gun animation
    private GunControl gunControl; // A reference to theGunControl on the object

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        gunControl = Items[0].GetComponent<GunControl>(); //Get the component of the gun object
    }
    void Update()
    {
        PickUp();
        Drop();
    }
    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.E)&&nearObject!=null) //When there are no nearObject
        {
            if(nearObject.tag =="Gun") //When the tag is a gun
            {
                hasItems[0]=true; //have a gun
                playerAnimator.SetBool("Gun", hasItems[0]); //gun animation start
                Items[0].SetActive(true); //active the player's hand item

            }
            else if(nearObject.tag =="HealPotion") //When the tag is a healpotion
            {
                hasItems[1]=true; //have a healpotion
            }
            Destroy(nearObject); //Object disappears
        } 
    }
    void Drop()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Items[0]) //If player have a gun
            {
                hasItems[0] = false; //do not have a gun
                playerAnimator.SetBool("Gun", hasItems[0]); //gun animation end
                Items[0].SetActive(false); //Disabled the player's hand item
                if (gunControl.CountBullet == 0) //When the used bullet is 0
                {
                    //Create a gun object on the floor
                    Instantiate(gun, transform.position, gun.transform.rotation *= Quaternion.Euler(0, 0, 0));
                }
                //gun reset
                gunControl.Head.SetActive(true);
                gunControl.CountBullet = 0; 
            }
        }
    }

    void OnTriggerStay(Collider other) //When the trigger stay, Change nearObject
    {
        if(other.tag == "Gun") 
        {
            nearObject=other.gameObject;
        }
        else if(other.tag == "HealPotion")
        {
            nearObject=other.gameObject;
        }
    }
    void OnTriggerExit(Collider other) //When the trigger exit , Change nearObject
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