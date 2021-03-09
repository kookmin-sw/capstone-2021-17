using UnityEngine;

namespace keypadSystem
{
    public class KeypadItemController : MonoBehaviour
    {
        [SerializeField] private KeypadController _keypadController;

        public void ShowKeypad()
        {
            _keypadController.ShowKeypad();
        }
    }
}
