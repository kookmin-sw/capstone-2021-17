using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameBoard : MonoBehaviour
{
    public GameObject cam;
    void LateUpdate()
    {
        if (cam == null)
        {
            transform.LookAt(Camera.main.transform.position);
        }
        else
        {
            transform.LookAt(cam.transform.position);
        }
        
    }
}
