using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Shift
{
    public class LayoutGroupPositionFix : MonoBehaviour
    {
        public bool forceRect = true;

        public void FixPos()
        {
            if (forceRect == true)
            {
                try
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                }

                catch { }
            }

            else
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }
    }
}