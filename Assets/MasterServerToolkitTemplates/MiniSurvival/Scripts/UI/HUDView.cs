using Aevien.UI;
using MasterServerToolkit.UI;
using TMPro;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class HUDView : UIView
    {
        [Header("Hud Components"), SerializeField]
        private UIProgressProperty healthProgress; 
        [SerializeField]
        private UIProgressProperty staminaProgress;
        [SerializeField]
        private TextMeshProUGUI playerDisplayNameText;

        /// <summary>
        /// Set the user display name
        /// </summary>
        /// <param name="value"></param>
        public void SetDisplayName(string value)
        {
            if (playerDisplayNameText)
                playerDisplayNameText.text = value;
        }

        /// <summary>
        /// Sets the health value
        /// </summary>
        /// <param name="value"></param>
        public void SetHealthValue(float value)
        {
            if (healthProgress) healthProgress.SetProgressValue(value);
        }

        /// <summary>
        /// Sets the stamina value
        /// </summary>
        /// <param name="value"></param>
        public void SetStaminaValue(float value)
        {
            if (staminaProgress) staminaProgress.SetProgressValue(value);
        }
    }
}