using UnityEngine;

namespace keypadSystem
{
    public class KPInputManager : MonoBehaviour
    {
        [Header("Raycast Pickup Input")]
        public KeyCode interactKey;

        [Header("Trigger Inputs")]
        public KeyCode triggerInteractKey;

        public static KPInputManager instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }
    }
}
