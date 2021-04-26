using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using MasterServerToolkit.MasterServer;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class CharacterProfile : PlayerCharacterProfile
    {
        #region INSPECTOR

        [Header("Profile Components"), SerializeField]
        protected CharacterVitals playerCharacterVitals;

        [Header("Profile Settings"), SerializeField, Range(0.1f, 100f)]
        protected float updateProfileInterval = 1f;

        #endregion

        public override bool IsReady => playerCharacterVitals;

        /// <summary>
        /// Stamina observable property
        /// </summary>
        protected ObservableFloat staminaProfileProperty;

        /// <summary>
        /// Health observable property
        /// </summary>
        protected ObservableFloat healthProfileProperty;

        public override void OnStartServer()
        {
            base.OnStartServer();

            Bridges.MirrorNetworking.RoomServer.Instance.LoadPlayerProfile(roomPlayer.Username, (isSuccess, error) =>
            {
                // Get stamina profile property
                staminaProfileProperty = roomPlayer.Profile.GetProperty<ObservableFloat>((short)ObservablePropertiyCodes.Stamina);

                // Set initial stamina value from profile
                playerCharacterVitals.CurrentStaminaValue = staminaProfileProperty.GetValue();

                // Get health profile property
                healthProfileProperty = roomPlayer.Profile.GetProperty<ObservableFloat>((short)ObservablePropertiyCodes.Health);

                // Set initial health value from profile
                playerCharacterVitals.CurrentHealthValue = healthProfileProperty.GetValue();

                // Update all the properties of profile
                InvokeRepeating(nameof(UpdateAllProperties), 1f, updateProfileInterval);
            });
        }

        /// <summary>
        /// Updates all profile properties
        /// </summary>
        protected virtual void UpdateAllProperties()
        {
            if (!IsReady) return;

            staminaProfileProperty.Set(playerCharacterVitals.CurrentStaminaValue);
            healthProfileProperty.Set(playerCharacterVitals.CurrentHealthValue);
        }
    }
}