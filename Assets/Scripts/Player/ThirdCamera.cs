using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ThirdCamera : MonoBehaviour {
 
    public GameObject target;
 
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    private void Awake()
    {
        // 만약에 카메라가 플레이어의 자식일경우 카메라 Transform도 동시에 뒤바뀌기에
        // 원치 않는 방향으로 카메라가 움직이기에 수정함
        if (transform.parent.gameObject == target)
        {    
            transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update () {
        Vector3 FixedPos =
            new Vector3(
            target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);
        transform.position = FixedPos;
    }
}

