﻿using MasterServerToolkit.Networking;

namespace MasterServerToolkit.MasterServer
{
    public class MstProfilesClient : MstBaseClient
    {
        public MstProfilesClient(IClientSocket connection) : base(connection) { }

        /// <summary>
        /// Sends a request to server, retrieves all profile values, and applies them to a provided
        /// profile
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="callback"></param>
        public void GetProfileValues(ObservableProfile profile, SuccessCallback callback)
        {
            GetProfileValues(profile, callback, Connection);
        }

        /// <summary>
        /// Sends a request to server, retrieves all profile values, and applies them to a provided profile
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="callback"></param>
        /// <param name="connection"></param>
        public void GetProfileValues(ObservableProfile profile, SuccessCallback callback, IClientSocket connection)
        {
            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected");
                return;
            }

            connection.SendMessage((short)MstMessageCodes.ClientProfileRequest, profile.PropertyCount, (status, response) =>
            {
                if (status != ResponseStatus.Success)
                {
                    callback.Invoke(false, response.AsString("Unknown error"));
                    return;
                }

                // Use the bytes received, to replicate the profile
                profile.FromBytes(response.AsBytes());

                // Listen to profile updates, and apply them
                connection.SetHandler((short)MstMessageCodes.UpdateClientProfile, message =>
                {
                    // UnityEngine.Debug.LogError($"Profile Updated from server");
                    profile.ApplyUpdates(message.AsBytes());
                });

                callback.Invoke(true, null);
            });
        }
    }
}