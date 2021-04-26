using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using MasterServerToolkit.Games;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.UI;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class MiniSurvivalProfilesManager : ProfilesBehaviour
    {
        protected HUDView hudView;

        /// <summary>
        /// Client side player character
        /// </summary>
        protected PlayerCharacterVitals playerCharacterVitals;

        protected override void Awake()
        {
            base.Awake();

            Mst.Events.AddEventListener(MstEventKeys.playerStartedGame, OnPlayerStartedGameEventHandler);
            Mst.Events.AddEventListener(MstEventKeys.playerFinishedGame, OnPlayerFinishedGameEventHandler);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Mst.Events.RemoveEventListener(MstEventKeys.playerStartedGame, OnPlayerStartedGameEventHandler);
            Mst.Events.RemoveEventListener(MstEventKeys.playerFinishedGame, OnPlayerFinishedGameEventHandler);

            // Register profile property update event handler
            Profile.OnPropertyUpdatedEvent -= Profile_OnPropertyUpdatedEvent;
        }

        protected override void OnInitialize()
        {
            Profile = new ObservableProfile
            {
                new ObservableString((short)ObservablePropertiyCodes.DisplayName),
                new ObservableString((short)ObservablePropertiyCodes.Avatar),
                new ObservableFloat((short)ObservablePropertiyCodes.Money),
                new ObservableFloat((short)ObservablePropertiyCodes.Health),
                new ObservableFloat((short)ObservablePropertiyCodes.Thirst),
                new ObservableFloat((short)ObservablePropertiyCodes.Hunger),
                new ObservableFloat((short)ObservablePropertiyCodes.Stamina),
                new ObservableFloat((short)ObservablePropertiyCodes.ZombiesKilled),
                new ObservableFloat((short)ObservablePropertiyCodes.PlayersKilled)
            };

            // Register profile property update event handler
            Profile.OnPropertyUpdatedEvent += Profile_OnPropertyUpdatedEvent;
        }

        private void Profile_OnPropertyUpdatedEvent(short key, IObservableProperty property)
        {
            ObservablePropertiyCodes propertyKey = (ObservablePropertiyCodes)key;

            switch (propertyKey)
            {
                case ObservablePropertiyCodes.DisplayName:
                    hudView?.SetDisplayName(property.CastTo<ObservableString>().GetValue());
                    break;
                case ObservablePropertiyCodes.Stamina:
                    
                    break;
            }
        }

        protected virtual void OnPlayerStartedGameEventHandler(EventMessage message)
        {
            hudView = ViewsManager.GetView<HUDView>("HUDView");
            hudView.Show();

            LoadProfile();

            playerCharacterVitals = message.GetData<PlayerCharacter>().GetComponent<PlayerCharacterVitals>();
            playerCharacterVitals.OnVitalChangedEvent += OnVitalChangeEventHandler;
            playerCharacterVitals.OnDieEvent += OnDieEventHandler;
        }

        protected virtual void OnDieEventHandler()
        {
            hudView?.Hide();

            MstTimer.WaitForSeconds(1f, () => {
                Connection.SendMessage((short)MiniSurvivalMessageCodes.CharacterDied);
            });
        }

        protected virtual void OnPlayerFinishedGameEventHandler(EventMessage message)
        {
            if (playerCharacterVitals)
            {
                playerCharacterVitals.OnVitalChangedEvent -= OnVitalChangeEventHandler;
                playerCharacterVitals.OnDieEvent -= OnDieEventHandler;
            }
        }

        protected virtual void OnVitalChangeEventHandler(short key, float value)
        {
            var vitalKey = (CharacterVitalKeys)key;

            if (hudView)
            {
                switch (vitalKey)
                {
                    case CharacterVitalKeys.Stamina:
                        hudView.SetStaminaValue(value);
                        break;
                    case CharacterVitalKeys.Health:
                        hudView.SetHealthValue(value);
                        break;
                }
            }
        }
    }
}