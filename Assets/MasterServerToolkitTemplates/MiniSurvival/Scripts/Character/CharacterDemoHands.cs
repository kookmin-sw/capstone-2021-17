using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class CharacterDemoHands : PlayerCharacterBehaviour
    {
        #region INSPECTOR

        [Header("Components"), SerializeField]
        private Transform handPivot;
        [SerializeField]
        private PlayerCharacterInput playerCharacterInput;
        [SerializeField]
        private PlayerCharacterTopDownLook playerCharacterTdLook;

        #endregion

        public override bool IsReady => handPivot && playerCharacterTdLook && playerCharacterInput;

        private void Update()
        {
            if (isLocalPlayer && IsReady)
            {
                if (playerCharacterInput.IsArmed())
                {
                    handPivot.rotation = Quaternion.LookRotation(playerCharacterTdLook.AimDirection());
                    //leftHandPivot.rotation = Quaternion.LookRotation(playerCharacterTdLook.AimDirection());
                }
                else
                {
                    handPivot.localRotation = Quaternion.LookRotation(Vector3.down);
                }
            }
        }
    }
}
