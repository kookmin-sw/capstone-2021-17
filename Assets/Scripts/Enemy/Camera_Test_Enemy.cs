using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Test_Enemy : MonoBehaviour
{
    // Update is called once per frame
    public Transform pos;

    void Update()
    {
        transform.position = pos.position + new Vector3(-3.339f, 1.75f, 0f);
        transform.rotation = pos.rotation;
    }
}
