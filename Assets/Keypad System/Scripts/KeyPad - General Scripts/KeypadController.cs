using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace keypadSystem
{
    public class KeypadController : MonoBehaviour
    {
        [Header("Keypad Parameters")]
        [SerializeField] public string validCode;
        public int characterLim;
        [HideInInspector] public bool firstClick;
        
        [Header("UI Elements")]
        public InputField codeText;
        [SerializeField] private GameObject keyPadCanvas;

        [Header("GameObjects")]
        [SerializeField] private GameObject keypadModel;

        [Header("Unlock Event")]
        [SerializeField] private UnityEvent unlock;

        public void CheckCode()
        {
            if (codeText.text == validCode)
            {
                keypadModel.tag = "Untagged";
                keypadModel.layer = 0;
                ValidCode();
            }

            else
            {
                KPAudioManager.instance.Play("KeypadDenied"); //0.2f
            }
        }

        void ValidCode() 
        {
            //IF YOUR CODE IS CORRECT!
            unlock.Invoke();
        }

        public void ShowKeypad()
        {
            KPDisableManager.instance.DisablePlayer(true);
            keyPadCanvas.SetActive(true);
        }

        public void CloseKeypad()
        {
            KPDisableManager.instance.DisablePlayer(false);
            keyPadCanvas.SetActive(false);
        }

        public void SingleBeep()
        {
            KPAudioManager.instance.Play("KeypadBeep");
        }

        public void SetPassword()
        {
            validCode = GameMgr.GeneratePassword(characterLim);
            //Debug.Log(validCode);
        }
    }
}
