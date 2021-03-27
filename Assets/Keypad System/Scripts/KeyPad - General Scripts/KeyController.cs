using UnityEngine;

namespace keypadSystem
{
    public class KeyController : MonoBehaviour
    {
        private KeypadController _keypadController;

        void Awake()
        {
            _keypadController = gameObject.GetComponent<KeypadController>();
        }

        public void KeyPressString(string keyString)
        {
            _keypadController.SingleBeep();

            if (!_keypadController.firstClick)
            {
                _keypadController.codeText.text = string.Empty;
                _keypadController.firstClick = true;
            }

            if (_keypadController.codeText.characterLimit <= (_keypadController.characterLim - 1))
            {
                _keypadController.codeText.characterLimit++;
                _keypadController.codeText.text += keyString;
            }
        }

        public void KeyPressEnt()
        {
            _keypadController.SingleBeep();
            _keypadController.CheckCode();
        }

        public void KeyPressClr()
        {
            _keypadController.SingleBeep();
            _keypadController.codeText.characterLimit = 0;
            _keypadController.codeText.text = string.Empty;
            _keypadController.firstClick = false;
        }

        public void KeyPressClose()
        {
            _keypadController.SingleBeep();
            KPDisableManager.instance.DisablePlayer(false);
            KeyPressClr();
            _keypadController.CloseKeypad();
        }
    }
}
