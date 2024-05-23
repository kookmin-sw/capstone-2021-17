using UnityEngine;
using UnityEngine.UI;

namespace keypadSystem
{
    public class KeypadRaycast : MonoBehaviour
    {
        [Header("Raycast References")]
        [SerializeField] private int rayLength = 5;
        [SerializeField] private LayerMask layerMaskInteract;
        [SerializeField] private LayerMask layerToExclude;
        private KeypadItemController rayCastedObj;

        [Header("Crosshair References")]
        [SerializeField] private Image crosshair;
        private bool isCrosshairActive;
        private bool doOnce;

        private const string keypadTag = "Keypad";

        void Update()
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            {
                int mask = 1 << layerToExclude.value | layerMaskInteract.value;

                if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
                {
                    if (hit.collider.CompareTag(keypadTag))
                    {
                        if (!doOnce)
                        {
                            rayCastedObj = hit.collider.gameObject.GetComponent<KeypadItemController>();
                            CrosshairChange(true);
                        }

                        isCrosshairActive = true;
                        doOnce = true;

                        if (Input.GetKeyDown(KPInputManager.instance.interactKey))
                        {
                            rayCastedObj.ShowKeypad();
                        }
                    }
                }
                else
                {
                    if (isCrosshairActive)
                    {
                        CrosshairChange(false);
                        doOnce = false;
                    }
                }
            }
        }

        void CrosshairChange(bool on)
        {
            if (on && !doOnce)
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
                isCrosshairActive = false;
            }
        }
    }
}
