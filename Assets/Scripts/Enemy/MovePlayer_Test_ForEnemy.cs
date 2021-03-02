using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer_Test_ForEnemy : MonoBehaviour
{  
    //화살표 입력 받으면 큐브 움직이기 임시로 쫓아가는 기능 테스트
    void Update()
    {  
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z + 5.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x - 5.0f * Time.deltaTime, this.transform.position.y, this.transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x + 5.0f * Time.deltaTime, this.transform.position.y, this.transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 5.0f * Time.deltaTime);
        }                
    }    
}
