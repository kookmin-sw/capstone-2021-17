﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door03_behavior : MonoBehaviour {

    // Public Variables
    [Tooltip("Mesh Door Left that woulbe be animated")]
    public GameObject DoorLeft;
    [Tooltip("Mesh Door Right that woulbe be animated")]
    public GameObject DoorRight;
    [Tooltip("Velocity to open and close the door")]
    public float OpenCloseVelocity = 1.0f;
    [Tooltip("Player Controller")]
    public GameObject GameObjectPlayer;
    [Tooltip("Keep the door Open")]
    public bool KeepDoorOpen = false;
    [Tooltip("The door doesn't open")]
    public bool KeepClosed = false;
    [Tooltip("Use a key to open the door / Defualt Key = e")]
    public bool useKeyToOpen = false;
    [Tooltip("Char to open the door")]
    public string CharKeyOpen = "e";

    // Private Variables
    float closePosition = 1.58f;
    float openPosition = 3.08f;
    bool enterTrigger;
    bool isClosing;
    bool pressKey;

    // Use this for initialization
    void Start()
    {
        enterTrigger = false;        

        if (KeepClosed == true)
        {
            KeepDoorOpen = false;
        }

        isClosing = false;
        pressKey = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(CharKeyOpen))
        {
            if ((isClosing == false) && useKeyToOpen && enterTrigger)
                pressKey = true;
        }



        if (KeepClosed == false)
        {
            if (useKeyToOpen == false)
            {
                if (enterTrigger)
                {
                    if (DoorRight.transform.localPosition.x < openPosition)
                    {
                        float translation = Time.deltaTime * (openPosition - closePosition) * OpenCloseVelocity;
                        DoorRight.transform.Translate(translation, 0, 0);
                        DoorLeft.transform.Translate(-translation, 0, 0);
                    }
                    else
                    {
                        DoorRight.transform.localPosition = new Vector3(openPosition, 0, 0);
                        DoorLeft.transform.localPosition = new Vector3(-openPosition, 0, 0);
                    }

                }
            }
            else
            {
                if (pressKey)
                {
                    if (DoorRight.transform.localPosition.x < openPosition)
                    {
                        float translation = Time.deltaTime * (openPosition - closePosition) * OpenCloseVelocity;
                        DoorRight.transform.Translate(translation, 0, 0);
                        DoorLeft.transform.Translate(-translation, 0, 0);
                    }
                    else
                    {
                        DoorRight.transform.localPosition = new Vector3(openPosition, 0, 0);
                        DoorLeft.transform.localPosition = new Vector3(-openPosition, 0, 0);
                        pressKey = false;
                    }

                }

            }


        }


        if (KeepClosed == false && KeepDoorOpen == false)
        {
            if (isClosing)
            {
                if (DoorRight.transform.localPosition.x > closePosition)
                {
                    float translation = Time.deltaTime * (openPosition - closePosition) * OpenCloseVelocity;
                    DoorRight.transform.Translate(-translation, 0, 0);
                    DoorLeft.transform.Translate(translation, 0, 0);
                    isClosing = true;
                }
                else
                {
                    DoorRight.transform.localPosition = new Vector3(closePosition, 0, 0);
                    DoorLeft.transform.localPosition = new Vector3(-closePosition, 0, 0);
                    isClosing = false;
                }

            }

        }



    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObjectPlayer)
        {
            if (isClosing == false)
                enterTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectPlayer)
        {
            StartCoroutine(DelayClosedDoor());
        }
    }

    IEnumerator DelayClosedDoor()
    {
        yield return new WaitForSeconds(1);
        enterTrigger = false;
        isClosing = true;
    }

}

