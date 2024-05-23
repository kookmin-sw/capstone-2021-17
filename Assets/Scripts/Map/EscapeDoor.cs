using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Escape(other.gameObject);
        }
    }

    public void Escape (GameObject player)
    {
        NetGamePlayer netGamePlayer = player.GetComponentInParent<NetGamePlayer>();

        if (netGamePlayer != null)
        {
            netGamePlayer.Escape();
        }
        
    }
}
