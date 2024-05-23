﻿using MasterServerToolkit.Logging;
using MasterServerToolkit.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MasterServerToolkit.MasterServer
{
    /// <summary>
    /// Chat module gives your availability to send chat messages to clients, create chat channels etc.
    /// </summary>
    public class ChatModule : BaseServerModule
    {
        /// <summary>
        /// If true, chat module will subscribe to auth module, and automatically setup chat users when they log in
        /// </summary>
        [Header("General Settings")]
        [SerializeField, Tooltip("If true, chat module will subscribe to auth module, and automatically setup chat users when they log in")]
        protected bool useAuthModule = true;

        [SerializeField, Tooltip("If false, chat channel name will be checked through CensorModule to find use of forbidden word in it")]
        protected bool ignoreForbiddenChannelNames = false;

        /// <summary>
        /// If true, the first channel that user joins will be set as hist local channel
        /// </summary>
        [SerializeField, Tooltip("If true, the first channel that user joins will be set as his local channel")]
        protected bool setFirstChannelAsLocal = true;

        /// <summary>
        /// If true, when user leaves all of the channels except for one, that one channel will be set to be his local channel
        /// </summary>
        [SerializeField, Tooltip("If true, when user leaves all of the channels except for one, that one channel will be set to be his local channel")]
        protected bool setLastChannelAsLocal = true;

        /// <summary>
        /// If true, users will be allowed to choose usernames
        /// </summary>
        [SerializeField, Tooltip("If true, users will be allowed to choose usernames")]
        protected bool allowUsernamePicking = true;

        /// <summary>
        /// Min number of character a channel name must consist of
        /// </summary>
        [SerializeField, Tooltip("Min number of character a channel name must consist of")]
        public int minChannelNameLength = 5;

        /// <summary>
        /// Max number of character a channel name must consist of
        /// </summary>
        [SerializeField, Tooltip("Max number of character a channel name must consist of")]
        public int maxChannelNameLength = 25;

        /// <summary>
        /// Censor module for bad words checking :)
        /// </summary>
        protected CensorModule censorModule;

        /// <summary>
        /// Auth module
        /// </summary>
        protected AuthModule authModule;

        /// <summary>
        /// Users connected to chats
        /// </summary>
        public Dictionary<string, ChatUserPeerExtension> ChatUsers { get; protected set; }

        /// <summary>
        /// List of available chat channels
        /// </summary>
        public Dictionary<string, ChatChannel> ChatChannels { get; protected set; }

        protected override void Awake()
        {
            base.Awake();

            ChatUsers = new Dictionary<string, ChatUserPeerExtension>();
            ChatChannels = new Dictionary<string, ChatChannel>();

            // Optional AuthModule dependency if "useAuthModule" is true
            AddOptionalDependency<AuthModule>();

            // Optional CensorModule for to forbade bad words in chats
            AddOptionalDependency<CensorModule>();
        }

        public override void Initialize(IServer server)
        {
            // Set handlers
            server.RegisterMessageHandler((short)MstMessageCodes.PickUsername, OnPickUsernameRequestHandler);
            server.RegisterMessageHandler((short)MstMessageCodes.JoinChannel, OnJoinChannelRequestHandler);
            server.RegisterMessageHandler((short)MstMessageCodes.LeaveChannel, OnLeaveChannelRequestHandler);
            server.RegisterMessageHandler((short)MstMessageCodes.GetCurrentChannels, OnGetCurrentChannelsRequestHandler);
            server.RegisterMessageHandler((short)MstMessageCodes.ChatMessage, OnChatMessageHandler);
            server.RegisterMessageHandler((short)MstMessageCodes.GetUsersInChannel, OnGetUsersInChannelRequestHandler);
            server.RegisterMessageHandler((short)MstMessageCodes.SetDefaultChannel, OnSetDefaultChannelRequestHandler);

            // Setup authModule dependencies
            authModule = server.GetModule<AuthModule>();

            // Setup censorModule
            censorModule = server.GetModule<CensorModule>();

            if (useAuthModule && authModule)
            {
                authModule.OnUserLoggedInEvent += OnUserLoggedInEventHandler;
                authModule.OnUserLoggedOutEvent += OnUserLoggedOutEventHandler;
            }
            else if (useAuthModule && !authModule)
            {
                logger.Error("Chat module was set to use Auth module, but Auth module was not found");
            }
        }

        /// <summary>
        /// Add new user to chat
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual bool AddChatUser(ChatUserPeerExtension user)
        {
            string username = user.Username;

            if (ChatUsers.ContainsKey(username))
            {
                logger.Error($"Trying to add user {username} to chat, but one is already connected");

                return false;
            }
            else
            {
                // Add the new user
                ChatUsers[user.Username] = user;

                // Start listening user disconnection
                user.Peer.OnPeerDisconnectedEvent += OnClientDisconnected;

                logger.Debug($"User {username} has been successfully added to chat");

                return true;
            }
        }

        /// <summary>
        /// Remove user from chat
        /// </summary>
        /// <param name="user"></param>
        protected virtual void RemoveChatUser(ChatUserPeerExtension user)
        {
            string username = user.Username;

            // Remove from chat users list
            ChatUsers.Remove(username);

            var channels = user.CurrentChannels.ToList();

            // Remove from channels
            foreach (var chatChannel in channels)
            {
                chatChannel.RemoveUser(user);
            }

            // Stop listening this removed user disconnection
            user.Peer.OnPeerDisconnectedEvent -= OnClientDisconnected;

            logger.Debug($"User {username} has been successfully removed from chat");
        }

        /// <summary>
        /// Creates chat user as <see cref="ChatUserPeerExtension"/>
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        protected virtual ChatUserPeerExtension CreateChatUser(IPeer peer, string username)
        {
            logger.Debug($"Created new chat user {username}");
            return new ChatUserPeerExtension(peer, username);
        }

        /// <summary>
        /// Retrieves an existing channel or creates a new one
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public virtual ChatChannel GetOrCreateChannel(string channelName)
        {
            return GetOrCreateChannel(channelName, ignoreForbiddenChannelNames);
        }

        /// <summary>
        /// Retrieves an existing channel or creates a new one.
        /// If <see cref="ignoreForbidden"/> value is set to false,
        /// before creating a channel, a check will be executed to make sure that
        /// no forbidden words are used in the name
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="ignoreForbidden"></param>
        /// <returns></returns>
        protected virtual ChatChannel GetOrCreateChannel(string channelName, bool ignoreForbidden)
        {
            var lowercaseName = channelName;

            logger.Debug($"Trying to get channel {channelName}");

            if (!ChatChannels.TryGetValue(lowercaseName, out ChatChannel channel))
            {
                // Check if our new channel name is incorrect
                if (channelName.Length < minChannelNameLength || channelName.Length > maxChannelNameLength)
                {
                    return null;
                }

                // There's no such channel, but we might be able to create one
                if (!ignoreForbidden && censorModule != null && censorModule.HasCensoredWord(channelName))
                {
                    // Channel contains a forbidden word
                    return null;
                }

                // Create new channel
                channel = new ChatChannel(channelName);

                // Add this channel to list
                ChatChannels.Add(lowercaseName, channel);
            }
            else
            {
                logger.Debug($"Channel with name {channelName} is already created");
            }

            return channel;
        }

        /// <summary>
        /// Removes existing chat user from all the channels, and creates a new 
        /// <see cref="ChatUserPeerExtension"/> with new username. If <see cref="joinSameChannels"/> is true, 
        /// user will be added to same channels
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="newUsername"></param>
        /// <param name="joinSameChannels"></param>
        public void ChangeUsername(IPeer peer, string newUsername, bool joinSameChannels = true)
        {
            var chatUser = peer.GetExtension<ChatUserPeerExtension>();

            if (chatUser == null)
            {
                return;
            }

            // Get previous chat user channels that one is connected to
            var prevChannels = chatUser.CurrentChannels.ToList();

            // Get his default chat channel
            var defaultChannel = chatUser.DefaultChannel;

            // Remove the user from chat
            RemoveChatUser(chatUser);

            // Create a new chat user
            var newExtension = CreateChatUser(peer, newUsername);

            // Replace with new user
            peer.AddExtension(newExtension);

            if (joinSameChannels)
            {
                foreach (var prevChannel in prevChannels)
                {
                    var channel = GetOrCreateChannel(prevChannel.Name);
                    if (channel != null)
                    {
                        channel.AddUser(newExtension);
                    }
                }

                if (defaultChannel != null && defaultChannel.Users.Contains(newExtension))
                {
                    // If we were added to the chat, which is now set as our default chat
                    // It's safe to set the default channel
                    newExtension.DefaultChannel = defaultChannel;
                }
            }
        }

        /// <summary>
        /// Handles chat message.
        /// Returns true, if message was handled
        /// If it returns false, message sender will receive a "Not Handled" response.
        /// </summary>
        protected virtual bool TryHandleChatMessage(ChatMessagePacket message, ChatUserPeerExtension sender, IIncomingMessage rawMessage)
        {
            try
            {
                // Set a true sender
                message.Sender = sender.Username;

                switch (message.MessageType)
                {
                    case ChatMessageType.ChannelMessage:

                        if (string.IsNullOrEmpty(message.Receiver))
                        {
                            // If this is a local chat message (no receiver is provided)
                            if (sender.DefaultChannel == null)
                            {
                                throw new MstMessageHandlerException("No channel is set to be your local channel", ResponseStatus.Failed);
                            }

                            sender.DefaultChannel.BroadcastMessage(message);
                            rawMessage.Respond(ResponseStatus.Success);
                            return true;
                        }

                        // Find the channel
                        if (!ChatChannels.TryGetValue(message.Receiver, out ChatChannel channel) || !sender.CurrentChannels.Contains(channel))
                        {
                            throw new MstMessageHandlerException($"You're not in the '{message.Receiver}' channel", ResponseStatus.Failed);
                        }

                        channel.BroadcastMessage(message);
                        rawMessage.Respond(ResponseStatus.Success);
                        return true;

                    case ChatMessageType.PrivateMessage:

                        if (!ChatUsers.TryGetValue(message.Receiver, out ChatUserPeerExtension receiver))
                        {
                            throw new MstMessageHandlerException($"User '{message.Receiver}' is not online", ResponseStatus.Failed);
                        }

                        receiver.Peer.SendMessage((short)MstMessageCodes.ChatMessage, message);
                        rawMessage.Respond(ResponseStatus.Success);
                        return true;
                }

                return false;
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                rawMessage.Respond(e.Message, e.Status);
                return false;
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                rawMessage.Respond(e.Message, ResponseStatus.Error);
                return false;
            }
        }

        #region Event Handlers

        /// <summary>
        /// Fired when new user logged in and <see cref="useAuthModule"/> is set to true
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserLoggedInEventHandler(IUserPeerExtension account)
        {
            // Create new chat user
            var chatUser = CreateChatUser(account.Peer, account.Username);

            // Add him to chat users list
            if (AddChatUser(chatUser))
            {
                // Add the extension
                account.Peer.AddExtension(chatUser);
            }
        }

        /// <summary>
        /// Fired if existing user logged out and <see cref="useAuthModule"/> is set to true
        /// </summary>
        /// <param name="account"></param>
        protected virtual void OnUserLoggedOutEventHandler(IUserPeerExtension account)
        {
            // Get chat user from extensions list
            var chatUser = account.Peer.GetExtension<ChatUserPeerExtension>();

            // Remove it from chat users list if is not null
            if (chatUser != null)
            {
                RemoveChatUser(chatUser);
            }
        }

        /// <summary>
        /// Fired if existing client disconnected
        /// </summary>
        /// <param name="peer"></param>
        protected virtual void OnClientDisconnected(IPeer peer)
        {
            peer.OnPeerDisconnectedEvent -= OnClientDisconnected;

            var chatUser = peer.GetExtension<ChatUserPeerExtension>();

            if (chatUser != null)
            {
                RemoveChatUser(chatUser);
            }
        }

        #endregion

        #region Message Handlers

        protected virtual void OnPickUsernameRequestHandler(IIncomingMessage message)
        {
            try
            {
                if (!allowUsernamePicking)
                {
                    throw new MstMessageHandlerException("Username picking is disabled", ResponseStatus.Failed);
                }

                var username = message.AsString();

                if (username.Contains(" "))
                {
                    throw new MstMessageHandlerException("Username cannot contain whitespaces", ResponseStatus.Failed);
                }

                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                if (chatUser != null)
                {
                    throw new MstMessageHandlerException($"You're already identified as: {chatUser.Username}", ResponseStatus.Failed);
                }

                if (ChatUsers.ContainsKey(username))
                {
                    throw new MstMessageHandlerException("There's already a user who has the same username", ResponseStatus.Failed);
                }

                chatUser = CreateChatUser(message.Peer, username);

                if (!AddChatUser(chatUser))
                {
                    throw new MstMessageHandlerException("Failed to add user to chat", ResponseStatus.Failed);
                }

                // Add the extension
                message.Peer.AddExtension(chatUser);

                // Send response
                message.Respond(ResponseStatus.Success);
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        protected virtual void OnJoinChannelRequestHandler(IIncomingMessage message)
        {
            try
            {
                // Get user from peer
                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                // If peer has no user
                if (chatUser == null)
                {
                    throw new MstMessageHandlerException("Chat cannot identify you", ResponseStatus.Unauthorized);
                }

                // Get channel name
                var channelName = message.AsString();

                // Trying to create channel with given name
                var channel = GetOrCreateChannel(channelName);

                if (channel == null)
                {
                    throw new MstMessageHandlerException("This channel is forbidden", ResponseStatus.Failed);
                }

                if (!channel.AddUser(chatUser))
                {
                    throw new MstMessageHandlerException("Failed to join a channel", ResponseStatus.Failed);
                }

                if (setFirstChannelAsLocal && chatUser.CurrentChannels.Count == 1)
                {
                    chatUser.DefaultChannel = channel;
                }

                message.Respond(ResponseStatus.Success);
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        protected virtual void OnLeaveChannelRequestHandler(IIncomingMessage message)
        {
            try
            {
                // Get user from peer
                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                // If peer has no user
                if (chatUser == null)
                {
                    throw new MstMessageHandlerException("Chat cannot identify you", ResponseStatus.Unauthorized);
                }

                // Get channel name
                var channelName = message.AsString();

                // Trying to get channel by name
                if (!ChatChannels.TryGetValue(channelName, out ChatChannel channel))
                {
                    throw new MstMessageHandlerException("This channel does not exist", ResponseStatus.Failed);
                }

                // Remove user from channel
                channel.RemoveUser(chatUser);

                if (setLastChannelAsLocal && chatUser.CurrentChannels.Count == 1)
                {
                    chatUser.DefaultChannel = chatUser.CurrentChannels.First();
                }

                message.Respond(ResponseStatus.Success);
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        protected virtual void OnSetDefaultChannelRequestHandler(IIncomingMessage message)
        {
            try
            {
                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                // If peer has no user
                if (chatUser == null)
                {
                    throw new MstMessageHandlerException("Chat cannot identify you", ResponseStatus.Unauthorized);
                }

                var channelName = message.AsString();
                var channel = GetOrCreateChannel(channelName);

                if (channel == null)
                {
                    throw new MstMessageHandlerException("This channel is forbidden", ResponseStatus.Failed);
                }

                // Add user to channel
                channel.AddUser(chatUser);

                // Set the property of default chat channel
                chatUser.DefaultChannel = channel;

                // Respond with a "success" status
                message.Respond(ResponseStatus.Success);
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        protected virtual void OnGetUsersInChannelRequestHandler(IIncomingMessage message)
        {
            try
            {
                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                // If peer has no user
                if (chatUser == null)
                {
                    throw new MstMessageHandlerException("Chat cannot identify you", ResponseStatus.Unauthorized);
                }

                var channelName = message.AsString();
                var channel = GetOrCreateChannel(channelName);

                if (channel == null)
                {
                    throw new MstMessageHandlerException("This channel is forbidden", ResponseStatus.Failed);
                }

                var users = channel.Users.Select(u => u.Username);
                message.Respond(users.ToBytes(), ResponseStatus.Success);
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        protected virtual void OnChatMessageHandler(IIncomingMessage message)
        {
            try
            {
                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                // If peer has no user
                if (chatUser == null)
                {
                    throw new MstMessageHandlerException("Chat cannot identify you", ResponseStatus.Unauthorized);
                }

                var packet = message.Deserialize(new ChatMessagePacket());

                if (!TryHandleChatMessage(packet, chatUser, message))
                {
                    throw new MstMessageHandlerException("Invalid message", ResponseStatus.NotHandled);
                }
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        protected virtual void OnGetCurrentChannelsRequestHandler(IIncomingMessage message)
        {
            try
            {
                var chatUser = message.Peer.GetExtension<ChatUserPeerExtension>();

                // If peer has no user
                if (chatUser == null)
                {
                    throw new MstMessageHandlerException("Chat cannot identify you", ResponseStatus.Unauthorized);
                }

                var channels = chatUser.CurrentChannels.Select(c =>
                {
                    return new ChatChannelInfo
                    {
                        Name = c.Name,
                        OnlineCount = (ushort)c.Users.Count()
                    };
                });

                message.Respond(new ChatChannelsListPacket() { Channels = channels.ToList() }, ResponseStatus.Success);
            }
            // If we got system exception
            catch (MstMessageHandlerException e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, e.Status);
            }
            // If we got another exception
            catch (Exception e)
            {
                logger.Error(e.Message);
                message.Respond(e.Message, ResponseStatus.Error);
            }
        }

        #endregion
    }
}