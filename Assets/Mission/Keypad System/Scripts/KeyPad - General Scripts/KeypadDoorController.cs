using UnityEngine;

namespace keypadSystem
{
    public class KeypadDoorController : MonoBehaviour
    {
        private Animator doorAnim;

        private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            doorAnim.Play("OpenDoor", 0, 0.0f);
        }
    }
}
