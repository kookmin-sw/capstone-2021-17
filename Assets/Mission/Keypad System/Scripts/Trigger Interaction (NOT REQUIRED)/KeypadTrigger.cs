using UnityEngine;

namespace keypadSystem
{
    public class KeypadTrigger : MonoBehaviour
    {
        [Header("Keypad Object")]
        [SerializeField] private KeypadItemController myKeypad;

        [Header("UI Prompt")]
        [SerializeField] private GameObject interactPrompt;    

        private const string playerTag = "Player";
        private bool canUse;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = true;
                interactPrompt.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                canUse = false;
                interactPrompt.SetActive(false);
            }
        }

        private void Update()
        {
            if (canUse)
            {
                if (Input.GetKeyDown(KPInputManager.instance.triggerInteractKey))
                {
                    myKeypad.ShowKeypad();
                    KPDisableManager.instance.DisablePlayer(true);
                }
            }
        }
    }
}
