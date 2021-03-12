using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어와 충돌 했을 때 플레이어를 없애고 다른 타겟을 찾기 위해 bool로 상태를 넘겨준다
public class Delete_Test : MonoBehaviour
{
    
    public Enemy_Chase Check_Catch;

    public void OnTriggerEnter(Collider other)   
    {
        //태그가 플레이어면
        if(other.CompareTag("Player"))
        {
            //잡았다는 걸 넘겨준다.
            Check_Catch.Check_Catched = true;
            Destroy(gameObject);
        }
    }
}
