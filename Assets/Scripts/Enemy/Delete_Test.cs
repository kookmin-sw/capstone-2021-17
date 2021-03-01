using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_Test : MonoBehaviour
{
    //When Catched
    public Enemy_Chase Check_Catch;

    public void OnTriggerEnter(Collider other)   
    {
        if(other.CompareTag("Player")){
            Check_Catch.Check_Catched = true;
            Destroy(gameObject);
        }
    }
}
