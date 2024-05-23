using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;

    void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject,2);
        }

    }
}
