using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam : MonoBehaviour{
    public GameObject Cam1;
    public GameObject Cam2;
    bool isCam2 = true;

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
        Cam1ON(); //시작할땐 Cam1을 틈
    }

    void Update()
    {
        //Y키를 누르면 전환
        if (Input.GetKeyDown(KeyCode.Y))
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
}
