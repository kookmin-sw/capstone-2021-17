using UnityEngine;
using System.Collections;

public class RaycastManager_Lever : MonoBehaviour
{
    private GameObject raycasted_obj;

    private int rayLength = 10;
    public LayerMask layerMaskInteract;

    public GameObject cHNormal;
    public GameObject cHHighlight;

    private bool canPull = true;

    private LeverController leverController;

    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Lever"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e") && canPull)
                {
                    raycasted_obj.GetComponentInChildren<Animator>().Play("HandlePull", -1, 0.0f);
                    raycasted_obj.GetComponent<LeverScript>().LeverNumber();
                    leverController = raycasted_obj.GetComponentInParent<LeverController>();
                    StartCoroutine(Timer(1.0f));
                }
            }

            else if (hit.collider.CompareTag("TestButton"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    leverController = raycasted_obj.GetComponentInParent<LeverController>();
                    leverController.LeverCheck();
                }
            }

            else if (hit.collider.CompareTag("Reset"))
            {
                raycasted_obj = hit.collider.gameObject;
                CrosshairActive();

                if (Input.GetKeyDown("e"))
                {
                    leverController = raycasted_obj.GetComponentInParent<LeverController>();
                    leverController.LeverReset();
                    StartCoroutine(Timer(1.0f));
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
