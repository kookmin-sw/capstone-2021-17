using System.Collections;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Transform BulletPos; //Bullet position
    public GameObject Bullet; //Bullet
    public GameObject Head; //Head of the gun
    public int CountBullet; //Used bullet
    //public int Damage;

    [SerializeField]
    PlayerInventory playerInventory;

    void Update()
    {
        //왼쪽 마우스 버튼을 클릭하면 & 사용한 총알이 0개이면 발사된다
        if(Input.GetMouseButtonUp(0)&&CountBullet==0)
        {
            StopCoroutine("Shot");
            StartCoroutine("Shot");
        }
    }
    public void Shoot()
    {
        StopCoroutine("Shot");
        StartCoroutine("Shot");
    }
    IEnumerator Shot() //Bullet Shot
    { 
        GameObject IntantBullet = Instantiate(Bullet,BulletPos.position,BulletPos.rotation); //Bullet creation
        Rigidbody BulletRigid = IntantBullet.GetComponent<Rigidbody>(); //Rigidbody creation
        BulletRigid.velocity=BulletPos.forward*15;
        Head.SetActive(false); // Disabled because the head is a bullet
        CountBullet +=1; //CountBullet plus

        if(playerInventory != null)
        {
            playerInventory.RemoveGun();
        }

        yield return null;
    }
}



