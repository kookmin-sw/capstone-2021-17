using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

namespace keypadSystem
{
    public class KPDisableManager : MonoBehaviour
    {
        public static KPDisableManager instance;

        public GameObject player; // Set By NetGamePlayer Or Self

        [SerializeField] 
        private Image crosshair; 

        void Awake()
        {
            instance = this;
        }

        public void DisablePlayer(bool disable)
        {
            Behaviour playerBehaviour;
            
            NetGamePlayer netGamePlayer = player.GetComponent<NetGamePlayer>();
            Animator animator = player.GetComponentInChildren<Animator>();
            if (netGamePlayer == null)
            {
                playerBehaviour = player.GetComponent<FirstPersonController>();
            }
            else
            {
                playerBehaviour = netGamePlayer.ThirdControl;
            }
            
            
            if (disable)
            {
                animator.SetFloat("Forward", 0.0f);
                animator.SetFloat("JumpLeg", 0.0f);
                animator.SetBool("isPlayWalk", false);
                //animator.speed = 0.0f;
                playerBehaviour.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                crosshair.enabled = false;
            }

            if (!disable)
            {
                //animator.speed = 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerBehaviour.enabled = true;
                crosshair.enabled = true;
            }
        }
    }
}
