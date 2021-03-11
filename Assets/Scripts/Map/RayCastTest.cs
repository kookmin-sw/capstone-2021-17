using UnityEngine;
using System.Collections;
using keypadSystem;

public class RayCastTest : MonoBehaviour
{
    private GameObject raycasted_obj;

    private int rayLength = 2;
    public LayerMask layerMaskInteract;

    public GameObject cHNormal;
    public GameObject cHHighlight;

    private bool canPull = true;
    private KeypadItemController rayCastedKeypad;

    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Lever"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e") && canPull)
                {
                    raycasted_obj.GetComponentInChildren<Animator>().Play("HandlePull", -1, 0.0f);
                    raycasted_obj.GetComponent<LeverScript>().LeverNumber();
                    raycasted_obj.layer = 0;
                    StartCoroutine(Timer(1.0f));
                }
            }
            else if (hit.collider.CompareTag("ItemBox"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    rayCastedKeypad = raycasted_obj.GetComponent<KeypadItemController>();
                    rayCastedKeypad.ShowKeypad();
                    raycasted_obj.layer = 0;
                }
            }

            else if (hit.collider.CompareTag("ItemHeal"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    //raycasted_obj.GetComponent<HealPack>().HealPlayer();
                    raycasted_obj.SetActive(false);
                }
            }
        }

        else
        {
            CrosshairNormal();
        }
    }

    void CrosshairActive()
    {
        cHNormal.SetActive(false);
        cHHighlight.SetActive(true);
    }

    void CrosshairNormal()
    {
        cHNormal.SetActive(true);
        cHHighlight.SetActive(false);
    }

    IEnumerator Timer(float waitTime)
    {
        canPull = false;
        yield return new WaitForSeconds(waitTime);
        canPull = true;
    }
}
