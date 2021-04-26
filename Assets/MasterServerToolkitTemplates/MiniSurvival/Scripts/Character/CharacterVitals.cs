using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using MasterServerToolkit.Games;
using MasterServerToolkit.Networking;
using Mirror;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class CharacterVitals : PlayerCharacterVitals, IDamageable, IHealable
    {
        #region INSPECTOR

        [Header("Profile Components"), SerializeField]
        protected PlayerCharacterMovement movementController;
        [SerializeField]
        protected PlayerCharacterLook characterLook;

        [Header("Stamina Settings"), SerializeField]
        protected float maxStaminaValue = 100f;
        [SerializeField, Range(1f, 100f)]
        protected float staminaRestoreValue = 1f;
        [SerializeField, Range(0.1f, 100f)]
        protected float staminaRestoreInterval = 1f;
        [SerializeField, Range(0.1f, 100f)]
        protected float staminaSpendingValue = 1f;
        [SerializeField, Range(0.1f, 100f)]
        protected float staminaSpendingInterval = 0.1f;
        [SerializeField, Range(1f, 100f)]
        protected float staminaMinNormalThreashold = 15f;

        [Header("Health Settings"), SerializeField]
        protected float maxHealthValue = 100f;

        #endregion

        public override bool IsReady => movementController && characterLook;

        /// <summary>
        /// Curren value of stamina
        /// </summary>
        public float CurrentStaminaValue { get; set; }

        /// <summary>
        /// Curren value of health
        /// </summary>
        public float CurrentHealthValue { get; set; }

        /// <summary>
        /// Check if character is tired
        /// </summary>
        protected bool characterIsTired = false;

        public override void OnStartServer()
        {
            base.OnStartServer();

            MstTimer.WaitForSeconds(0.2f, () =>
            {
                NotifyVitalChanged((short)CharacterVitalKeys.Health, CurrentHealthValue);
                NotifyVitalChanged((short)CharacterVitalKeys.Stamina, CurrentStaminaValue);

                // Start updating stamina restoration
                InvokeRepeating(nameof(RestoreStamina), 1f, staminaRestoreInterval);

                // Start updating stamina spending
                InvokeRepeating(nameof(SpendStamina), 1f, staminaSpendingInterval);
            });
        }

        protected virtual void Update()
        {
            if (!IsReady) return;

            if (isServer)
            {
                if (characterIsTired && CurrentStaminaValue > staminaMinNormalThreashold)
                {
                    characterIsTired = false;
                    movementController.AllowRunning(true);
                }
            }
        }

        /// <summary>
        /// Restores character stamina value on server
        /// </summary>
        protected virtual void RestoreStamina()
        {
            if (isServer)
            {
                if (!IsReady || movementController.IsRunning) return;

                if (CurrentStaminaValue < maxStaminaValue)
                {
                    CurrentStaminaValue += staminaRestoreValue;
                    if (CurrentStaminaValue > maxStaminaValue) CurrentStaminaValue = maxStaminaValue;
                }

                NotifyVitalChanged((short)CharacterVitalKeys.Stamina, CurrentStaminaValue);
            }
        }


        /// <summary>
        /// Spends character stamina value on server
        /// </summary>
        protected virtual void SpendStamina()
        {
            if (isServer)
            {
                if (!IsReady) return;

                if (CurrentStaminaValue > 0f && movementController.IsRunning)
                {
                    CurrentStaminaValue -= staminaSpendingValue;

                    if (CurrentStaminaValue < 1f)
                    {
                        CurrentStaminaValue = 0;
                        characterIsTired = true;
                        movementController.AllowRunning(false);
                    }

                    NotifyVitalChanged((short)CharacterVitalKeys.Stamina, CurrentStaminaValue);
                }
            }
        }

        /// <summary>
        /// Make damage to this character
        /// </summary>
        /// <param name="value"></param>
        [Server]
        public virtual void TakeDamage(float value)
        {
            if (IsAlive)
            {
                float absValue = Mathf.Abs(value);
                CurrentHealthValue = Mathf.Clamp(CurrentHealthValue - absValue, 0f, maxHealthValue);
                NotifyVitalChanged((short)CharacterVitalKeys.Health, CurrentHealthValue);

                if (CurrentHealthValue < 1)
                {
                    NotifyDied();
                    movementController.AllowMoving(false);
                    Destroy(gameObject, 5f);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public virtual void Heal(float value)
        {
            if (isServer && IsAlive)
            {
                float absValue = Mathf.Abs(value);
                CurrentHealthValue = Mathf.Clamp(CurrentHealthValue + absValue, 0f, maxHealthValue);
                NotifyVitalChanged((short)CharacterVitalKeys.Health, CurrentHealthValue);
            }
        }
    }
}