﻿using MasterServerToolkit.Networking;
using System.Collections.Generic;

namespace MasterServerToolkit.MasterServer
{
    public class ChatChannel
    {
        private Dictionary<string, ChatUserPeerExtension> channelUsers;

        /// <summary>
        /// Name of the channel
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Users connected to chis channel
        /// </summary>
        public IEnumerable<ChatUserPeerExtension> Users => channelUsers.Values;

        public ChatChannel(string name)
        {
            Name = name;
            channelUsers = new Dictionary<string, ChatUserPeerExtension>();
        }

        /// <summary>
        /// Returns true, if user successfully joined a channel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(ChatUserPeerExtension user)
        {
            if (!IsUserAllowed(user))
            {
                return false;
            }

            // Add disconnect listener
            user.Peer.OnPeerDisconnectedEvent += OnUserDisconnect;

            // Add user
            channelUsers.Add(user.Username, user);

            // Add channel to users collection
            user.CurrentChannels.Add(this);

            // Notify about new user
            NotifyOnJoined(user);

            return true;
        }

        /// <summary>
        /// Notify all users about new user who joined this channel
        /// </summary>
        /// <param name="newUser"></param>
        protected virtual void NotifyOnJoined(ChatUserPeerExtension newUser)
        {
            var data = new List<string>() { Name, newUser.Username };
            var msg = Mst.Create.Message((short)MstMessageCodes.UserJoinedChannel, data.ToBytes());

            foreach (var user in channelUsers.Values)
            {
                if (user.Username != newUser.Username)
                {
                    user.Peer.SendMessage(msg, DeliveryMethod.Reliable);
                }
            }
        }

        /// <summary>
        /// Notify all users about user who left this channel
        /// </summary>
        /// <param name="removedUser"></param>
        protected virtual void NotifyOnLeft(ChatUserPeerExtension removedUser)
        {
            var data = new List<string>() { Name, removedUser.Username };
            var msg = Mst.Create.Message((short)MstMessageCodes.UserLeftChannel, data.ToBytes());

            foreach (var user in channelUsers.Values)
            {
                if (user.Username != removedUser.Username)
                {
                    user.Peer.SendMessage(msg, DeliveryMethod.Reliable);
                }
            }
        }

        /// <summary>
        /// Check if user is allowed to be added to channel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual bool IsUserAllowed(ChatUserPeerExtension user)
        {
            // Can't join if already here
            return !channelUsers.ContainsKey(user.Username);
        }

        /// <summary>
        /// Invoked, when user, who is connected to this channel, leaves
        /// </summary>
        /// <param name="peer"></param>
        protected virtual void OnUserDisconnect(IPeer peer)
        {
            var extension = peer.GetExtension<ChatUserPeerExtension>();

            if (extension == null)
            {
                return;
            }

            RemoveUser(extension);
        }

        /// <summary>
        /// Removes user from channel
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(ChatUserPeerExtension user)
        {
            // Remove disconnect listener
            user.Peer.OnPeerDisconnectedEvent -= OnUserDisconnect;

            // Remove channel from users collection
            user.CurrentChannels.Remove(this);

            // Remove user
            channelUsers.Remove(user.Username);

            if (user.DefaultChannel == this)
            {
                user.DefaultChannel = null;
            }

            NotifyOnLeft(user);
        }

        /// <summary>
        /// Handle messages
        /// </summary>
        public virtual void BroadcastMessage(ChatMessagePacket packet)
        {
            // Override name to be in a "standard" format (uppercase letters and etc.)
            packet.Receiver = Name;

            var msg = Mst.Create.Message((short)MstMessageCodes.ChatMessage, packet);

            foreach (var user in channelUsers.Values)
            {
                user.Peer.SendMessage(msg, DeliveryMethod.Reliable);
            }
        }
    }
}