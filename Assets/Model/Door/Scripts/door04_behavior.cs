using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door04_behavior : MonoBehaviour {

    // Public Variables
    [Tooltip("Mesh Door Left that woulbe be animated")]
    public GameObject DoorLeft;
    [Tooltip("Mesh Door Right that woulbe be animated")]
    public GameObject DoorRight;
    [Tooltip("Mesh Door Top that woulbe be animated")]
    public GameObject DoorTop;
    [Tooltip("Mesh Door Down that woulbe be animated")]
    public GameObject DoorBottom;
    [Tooltip("Velocity to open and close the door")]
    public float OpenCloseVelocity = 1.0f;
    [Tooltip("Player Controller")]
    public GameObject GameObjectPlayer;
    [Tooltip("Keep the door Open")]
    public bool KeepDoorOpen = false;
    [Tooltip("The door doesn't open")]
    public bool KeepClosed = false;
    [Tooltip("Use a key to open the door")]
    public bool useKeyToOpen = false;
    [Tooltip("Char to open the door")]
    public string CharKeyOpen = "e";


    // Private Variables
    float closePositionLateral = 2.61f;
    float openPositionLateral = 4.40f;
    float closePositionTop = 3.36f;
    float openPositionTop = 5.05f;
    float closePositionBottom = 0.07f;
    float openPositionBottom = -1.13f;
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
            if ((isClosing== false) && useKeyToOpen && enterTrigger)
                pressKey = true;

        }


        if (KeepClosed == false)
        {

            if (useKeyToOpen == false)
            {

                if (enterTrigger)
                {

                    if (DoorRight.transform.localPosition.x < openPositionLateral)
                    {
                        float translation = Time.deltaTime * (openPositionLateral - closePositionLateral) * OpenCloseVelocity;
                        float translationTop = Time.deltaTime * (openPositionTop - closePositionTop) * OpenCloseVelocity;
                        float translationBottom = Time.deltaTime * (openPositionBottom - closePositionBottom) * OpenCloseVelocity;
                        DoorRight.transform.Translate(translation, 0, 0);
                        DoorLeft.transform.Translate(-translation, 0, 0);
                        DoorTop.transform.Translate(0, translationTop, 0);
                        DoorBottom.transform.Translate(0, translationBottom, 0);
                    }
                    else
                    {
                        DoorRight.transform.localPosition = new Vector3(openPositionLateral, 0.07f, 0);
                        DoorLeft.transform.localPosition = new Vector3(-openPositionLateral, 0.07f, 0);
                        DoorTop.transform.localPosition = new Vector3(0, openPositionTop, 0);
                        DoorBottom.transform.localPosition = new Vector3(0, openPositionBottom, 0);
                    }

                }

            }
            else
            {
                if (pressKey)
                {

                    if (DoorRight.transform.localPosition.x < openPositionLateral)
                    {
                        float translation = Time.deltaTime * (openPositionLateral - closePositionLateral) * OpenCloseVelocity;
                        float translationTop = Time.deltaTime * (openPositionTop - closePositionTop) * OpenCloseVelocity;
                        float translationBottom = Time.deltaTime * (openPositionBottom - closePositionBottom) * OpenCloseVelocity;

                        DoorRight.transform.Translate(translation, 0, 0);
                        DoorLeft.transform.Translate(-translation, 0, 0);
                        DoorTop.transform.Translate(0, translationTop, 0);
                        DoorBottom.transform.Translate(0, translationBottom, 0);

                    }
                    else
                    {
                        DoorRight.transform.localPosition = new Vector3(openPositionLateral, 0.07f, 0);
                        DoorLeft.transform.localPosition = new Vector3(-openPositionLateral, 0.07f, 0);
                        DoorTop.transform.localPosition = new Vector3(0, openPositionTop, 0);
                        DoorBottom.transform.localPosition = new Vector3(0, openPositionBottom, 0);
                        pressKey = false;
                    }

                }

            }


        }



        if (KeepClosed == false && KeepDoorOpen == false)
        {

            if (isClosing)
            {

                if (DoorRight.transform.localPosition.x > closePositionLateral)
                {
                    float translation = Time.deltaTime * (openPositionLateral - closePositionLateral) * OpenCloseVelocity;
                    float translationTop = Time.deltaTime * (openPositionTop - closePositionTop) * OpenCloseVelocity;
                    float translationBottom = Time.deltaTime * (openPositionBottom - closePositionBottom) * OpenCloseVelocity;

                    DoorRight.transform.Translate(-translation, 0, 0);
                    DoorLeft.transform.Translate(translation, 0, 0);
                    DoorTop.transform.Translate(0, -translationTop, 0);
                    DoorBottom.transform.Translate(0, -translationBottom, 0);
                    isClosing = true;
                }
                else
                {
                    DoorRight.transform.localPosition = new Vector3(closePositionLateral, 0.07f, 0);
                    DoorLeft.transform.localPosition = new Vector3(-closePositionLateral, 0.07f, 0);
                    DoorTop.transform.localPosition = new Vector3(0, closePositionTop, 0);
                    DoorBottom.transform.localPosition = new Vector3(0,closePositionBottom, 0);
                    isClosing = false;
                }

            }

        }



    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObjectPlayer)
        {
            if (isClosing==false)
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
