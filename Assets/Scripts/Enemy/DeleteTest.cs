using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어와 충돌 했을 때 플레이어를 없애고 다른 타겟을 찾기 위해 bool로 상태를 넘겨준다
public class DeleteTest : MonoBehaviour
{
    [SerializeField]
    private EnemyControl checkCatch;

    public void OnTriggerEnter(Collider other)   
    {
        //태그가 플레이어면
        if(other.CompareTag("Enemy"))
        {            
            Destroy(gameObject);
        }
    }
}
