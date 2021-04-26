using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using Mirror;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    [DisallowMultipleComponent]
    public class CharacterAnimator : PlayerCharacterBehaviour
    {
        [Header("Components"), SerializeField]
        protected Animator animatorController;
        [SerializeField]
        protected PlayerCharacterMovement movementController;
        [SerializeField]
        protected NetworkAnimator networkAnimatorController;
        [SerializeField]
        protected CharacterVitals characterVitals;
        [SerializeField]
        protected PlayerCharacterInput characterInput;

        public override bool IsReady => animatorController
            && movementController
            && networkAnimatorController
            && characterVitals
            && characterInput;

        private void Update()
        {
            if (isLocalPlayer && IsReady)
            {
                SetIsMoving(movementController.IsWalking);
                SetIsRunning(movementController.IsRunning);

                //if (characterInput.IsAtack())
                //    SetIsAttacking();
            }
        }

        private void OnDestroy()
        {
            if (characterVitals)
                characterVitals.OnDieEvent -= CharacterVitals_EventOnDie;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            characterVitals.OnDieEvent += CharacterVitals_EventOnDie;
        }

        private void CharacterVitals_EventOnDie()
        {
            networkAnimatorController.SetTrigger("Die");
        }

        public void SetIsMoving(bool value)
        {
            animatorController.SetBool("IsMoving", value);
        }

        public void SetIsRunning(bool value)
        {
            animatorController.SetBool("IsRunning", value);
        }

        public void SetIsAttacking()
        {
            networkAnimatorController.SetTrigger("Attack");
        }

        public void SetAnimator(Animator animator)
        {
            animatorController = animator;
            networkAnimatorController.animator = animator;
        }
    }
}