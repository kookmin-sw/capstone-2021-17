using UnityEngine;

namespace keypadSystem
{
    public class KeypadItemController : MonoBehaviour
    {
        [SerializeField] private KeypadController _keypadController;

        public void ShowKeypad()
        {
            GameMgr.lockKey = true;
            _keypadController.ShowKeypad();
        }

        public void CloseKeypad()
        {
            GameMgr.lockKey = false;
            _keypadController.CloseKeypad();
        }
    }
}
