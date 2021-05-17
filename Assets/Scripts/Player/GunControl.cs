using System.Collections;
using UnityEngine;
using Mirror;

public class GunControl : MonoBehaviour
{
    public Transform BulletPos; //Bullet position
    public GameObject Bullet; //Bullet
    public GameObject Head; //Head of the gun
    public int CountBullet; //Used bullet
    //public int Damage;

    [SerializeField]
    PlayerInventory playerInventory;

    [SerializeField]
    GunControlNetBehaviour gunControlNet;

    
    public void Shoot()
    {
        StopCoroutine("Shot");
        StartCoroutine("Shot");
    }
    IEnumerator Shot() //Bullet Shot
    {
        gunControlNet.SpawnBullet();
        Head.SetActive(false); // Disabled because the head is a bullet
        CountBullet +=1; //CountBullet plus

        if(playerInventory != null)
        {
            playerInventory.RemoveGun();
        }

        yield return null;
    }
    
}



