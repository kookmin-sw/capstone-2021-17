using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.Utils;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public enum ObservablePropertiyCodes { DisplayName, Avatar, Money, Health, Thirst, Hunger, Stamina, ZombiesKilled, PlayersKilled }

    public class MiniSurvivalProfilesModule : ProfilesModule
    {
        [Header("Start Profile Values"), SerializeField, Range(0, 1000)]
        private int money = 100;
        [SerializeField, Range(10, 100)]
        private float health = 100;
        [SerializeField, Range(10, 100)]
        private float thirst = 100;
        [SerializeField, Range(10, 100)]
        private float hunger = 100;
        [SerializeField, Range(10, 100)]
        private float stamina = 100;
        [SerializeField]
        private string avatarUrl = "https://i.imgur.com/JQ9pRoD.png";

        public HelpBox _header = new HelpBox()
        {
            Text = "This script is a custom module, which sets up profiles values for new users"
        };

        public override void Initialize(IServer server)
        {
            base.Initialize(server);

            // Set the new factory in ProfilesModule
            ProfileFactory = CreateProfileInServer;

            server.RegisterMessageHandler((short)MiniSurvivalMessageCodes.ChangeDisplayData, ChangeDisplayDataRequestHandler);
            server.RegisterMessageHandler((short)MiniSurvivalMessageCodes.CharacterDied, CharacterDiedHandler);
        }

        /// <summary>
        /// This method is just for creation of profile on server side as default for users that are logged in for the first time
        /// </summary>
        /// <param name="username"></param>
        /// <param name="clientPeer"></param>
        /// <returns></returns>
        private ObservableServerProfile CreateProfileInServer(string username, IPeer clientPeer)
        {
            return new ObservableServerProfile(username, clientPeer)
            {
                new ObservableString((short)ObservablePropertiyCodes.DisplayName, SimpleNameGenerator.Generate(Gender.Male)),
                new ObservableString((short)ObservablePropertiyCodes.Avatar, avatarUrl),
                new ObservableInt((short)ObservablePropertiyCodes.Money, money),
                new ObservableFloat((short)ObservablePropertiyCodes.Health, health),
                new ObservableFloat((short)ObservablePropertiyCodes.Thirst, thirst),
                new ObservableFloat((short)ObservablePropertiyCodes.Hunger, hunger),
                new ObservableFloat((short)ObservablePropertiyCodes.Stamina, stamina),
                new ObservableInt((short)ObservablePropertiyCodes.ZombiesKilled),
                new ObservableInt((short)ObservablePropertiyCodes.PlayersKilled)
            };
        }

        #region MESSAGE HANDLERS

        private void CharacterDiedHandler(IIncomingMessage message)
        {
            var userProfileExtension = message.Peer.GetExtension<ProfilePeerExtension>();

            if (userProfileExtension != null && message.Peer.GetExtension<IUserPeerExtension>() is UserPeerExtension userAccounExtension)
            {
                userProfileExtension.Profile.FromBytes(CreateProfile(userAccounExtension.Username, message.Peer).ToBytes());
            }
            else
            {
                logger.Error("We cannot update user profile");
            }
        }

        protected virtual void ChangeDisplayDataRequestHandler(IIncomingMessage message)
        {
            //var userExtension = message.Peer.GetExtension<IUserPeerExtension>();

            //if (userExtension == null || userExtension.Account == null)
            //{
            //    message.Respond("Invalid session", ResponseStatus.Unauthorized);
            //    return;
            //}

            //var newProfileData = new Dictionary<string, string>().FromBytes(message.AsBytes());

            //try
            //{
            //    if (ProfilesList.TryGetValue(userExtension.Username, out ObservableServerProfile profile))
            //    {
            //        profile.GetProperty<ObservableString>((short)ObservablePropertiyCodes.DisplayName).Set(newProfileData["displayName"]);
            //        profile.GetProperty<ObservableString>((short)ObservablePropertiyCodes.Avatar).Set(newProfileData["avatarUrl"]);

            //        message.Respond(ResponseStatus.Success);
            //    }
            //    else
            //    {
            //        message.Respond("Invalid session", ResponseStatus.Unauthorized);
            //    }
            //}
            //catch (Exception e)
            //{
            //    message.Respond($"Internal Server Error: {e}", ResponseStatus.Error);
            //}
        }

        #endregion
    }
}