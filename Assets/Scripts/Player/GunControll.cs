using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    public int Damage;
    public TrailRenderer Effect;
    public Transform BulletPos;
    public GameObject Bullet;
    public int CountBullet;
    public GameObject Head;

    void Update()
    {
        if(Input.GetMouseButtonUp(0)&&CountBullet==0)
        {
            StopCoroutine("Shot");
            StartCoroutine("Shot");
        }
    }

    IEnumerator Shot()
    {
        //총알 발사
        GameObject IntantBullet = Instantiate(Bullet,BulletPos.position,BulletPos.rotation);
        Rigidbody BulletRigid = IntantBullet.GetComponent<Rigidbody>();
        BulletRigid.velocity=BulletPos.forward*15;
        Head.SetActive(false);
        CountBullet+=1;
        yield return null;
    }
}



