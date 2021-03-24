using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

namespace keypadSystem
{
    public class KPDisableManager : MonoBehaviour
    {
        public static KPDisableManager instance;

        public GameObject player;

        [SerializeField] 
        private Image crosshair; 

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void DisablePlayer(bool disable)
        {
            Behaviour playerBehaviour = player.GetComponent<ThirdPersonUserControl>();
             
            if(playerBehaviour == null)
            {
                playerBehaviour = player.GetComponent<FirstPersonController>();
            }
            
            
            if (disable)
            {
                playerBehaviour.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                crosshair.enabled = false;
            }

            if (!disable)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerBehaviour.enabled = true;
                crosshair.enabled = true;
            }
        }
    }
}
