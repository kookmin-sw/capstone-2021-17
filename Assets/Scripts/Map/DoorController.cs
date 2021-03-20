using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    // Public Variables
    [Tooltip("Mesh Door that woulbe be animated")]
    public GameObject Door;
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
    public bool CharKeyOpen = false;

    // Private Variables
    float closePosition = 0.76f;
    float openPosition = 2.22f;
    bool enterTrigger;
    bool isClosing;
    bool pressKey;
    LeverController lever;

    // Use this for initialization
    void Start()
    {
        lever = gameObject.GetComponentInChildren<LeverController>();
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
        if (lever.open == true)
        {
            CharKeyOpen = true;
        }

        if (CharKeyOpen == true)
        {
            if ((isClosing == false) && useKeyToOpen && enterTrigger)
            {
                pressKey = true;
            }

        }


        if (KeepClosed == false)
        {
            if (useKeyToOpen == false)
            {

                if (enterTrigger)
                {
                    if (Door.transform.localPosition.x < openPosition)
                    {
                        float translation = Time.deltaTime * (openPosition - closePosition) * OpenCloseVelocity;
                        Door.transform.Translate(translation, 0, 0);
                    }
                    else
                    {
                        Door.transform.localPosition = new Vector3(openPosition, 0, 0);
                    }
                }
            }
            else
            {
                if (pressKey)
                {

                    if (Door.transform.localPosition.x < openPosition)
                    {
                        float translation = Time.deltaTime * (openPosition - closePosition) * OpenCloseVelocity;
                        Door.transform.Translate(translation, 0, 0);
                    }
                    else
                    {
                        Door.transform.localPosition = new Vector3(openPosition, 0, 0);
                        pressKey = false;
                    }

                }

            }


        }



        if (KeepClosed == false && KeepDoorOpen == false)
        {
            if (isClosing)
            {
                if (Door.transform.localPosition.x > closePosition)
                {
                    float translation = Time.deltaTime * (openPosition - closePosition) * OpenCloseVelocity;
                    Door.transform.Translate(-translation, 0, 0);
                    isClosing = true;
                }
                else
                {
                    Door.transform.localPosition = new Vector3(closePosition, 0, 0);
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
            {
                enterTrigger = true;
            }
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