using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam : MonoBehaviour{
    public GameObject Cam1;
    public GameObject Cam2;
    bool isCam2 = false;

    private void Cam1ON() //카메라1 키기
    {
        Cam1.SetActive(true);
        Cam2.SetActive(false);
    }
    private void Cam2ON() //카메라2 키기
    {
        Cam2.SetActive(true);
        Cam1.SetActive(false);
    }

    void Start()
    {
        CheckCam();
    }

    void CheckCam()
    {
        if (isCam2 == true)
        {
            isCam2  = false;
            Cam2ON();
        }
        else
        {
            isCam2  = true;
            Cam1ON();
        }
    }
}
