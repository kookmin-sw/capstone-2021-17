﻿using MasterServerToolkit.Logging;
using MasterServerToolkit.Networking;
using System;
using UnityEngine.SceneManagement;

namespace MasterServerToolkit.MasterServer
{
    /// <summary>
    /// Room access provider callback
    /// </summary>
    /// <param name="access"></param>
    /// <param name="error"></param>
    public delegate void RoomAccessProviderCallback(RoomAccessPacket access, string error);

    /// <summary>
    /// Room access provider factory
    /// </summary>
    /// <param name="accessCheckOptions">Options you may use to dive access</param>
    /// <param name="giveAccess"></param>
    public delegate void RoomAccessProvider(RoomAccessProviderCheck accessCheckOptions, RoomAccessProviderCallback giveAccess);

    /// <summary>
    /// Instance of this class will be created when room registration is completed.
    /// It acts as a helpful way to manage a registered room.
    /// </summary>
    public class RoomController
    {
        /// <summary>
        /// Access provider
        /// </summary>
        private RoomAccessProvider accessProvider;

        /// <summary>
        /// Connection of current room controller
        /// </summary>
        public IClientSocket Connection { get; private set; }

        /// <summary>
        /// Room Id
        /// </summary>
        public int RoomId { get; private set; } = -1;

        /// <summary>
        /// Options of current room controller
        /// </summary>
        public RoomOptions Options { get; private set; }

        /// <summary>
        /// Logger of all room controllers
        /// </summary>
        public static Logger Logger { get; private set; }

        /// <summary>
        /// Check if room is active
        /// </summary>
        public bool IsActive => Connection != null && RoomId > -1;

        /// <summary>
        /// Access provider of current room controller
        /// </summary>
        public RoomAccessProvider AccessProvider
        {
            get
            {
                return accessProvider ?? DefaultAccessProvider;
            }
            set
            {
                accessProvider = value;
            }
        }

        public RoomController(int roomId, IClientSocket connection, RoomOptions options)
        {
            Logger = Mst.Create.Logger(typeof(RoomController).Name, LogLevel.Warn);

            Connection = connection;
            RoomId = roomId;
            Options = options;

            // Add handlers
            connection.SetHandler((short)MstMessageCodes.ProvideRoomAccessCheck, ProvideRoomAccessCheckHandler);
        }

        /// <summary>
        /// Destroys and unregisters the room
        /// </summary>
        public void Destroy()
        {
            Destroy(null);
        }

        /// <summary>
        /// Destroys and unregisters the room
        /// </summary>
        public void Destroy(SuccessCallback callback)
        {
            Mst.Server.Rooms.DestroyRoom(RoomId, (isSuccess, error) => {

                if (!isSuccess)
                {
                    callback?.Invoke(false, error);
                    Logger.Error(error);
                    return;
                }

                Logger.Debug($"Room {RoomId} was successfully unregistered");

                callback?.Invoke(true, string.Empty);

                Connection = null;
                RoomId = -1;

            }, Connection);
        }

        /// <summary>
        /// Send's current options to master server
        /// </summary>
        public void SaveOptions()
        {
            SaveOptions(Options);
        }

        /// <summary>
        /// Send's new options to master server
        /// </summary>
        public void SaveOptions(RoomOptions options)
        {
            SaveOptions(options, (successful, error) =>
            {
                if (!successful)
                {
                    Logger.Error(error);
                }
                else
                {
                    Logger.Debug("Room " + RoomId + " options changed successfully");
                    Options = options;
                }
            });
        }

        /// <summary>
        /// Send's new options to master server
        /// </summary>
        public void SaveOptions(RoomOptions options, SuccessCallback callback)
        {
            Mst.Server.Rooms.SaveOptions(RoomId, options, (successful, error) =>
            {
                if (successful)
                {
                    Options = options;
                }

                callback.Invoke(successful, error);

            }, Connection);
        }

        /// <summary>
        /// Sends the token to "master" server to see if it's valid. If it is -
        /// callback will be invoked with peer id of the user, whos access was confirmed.
        /// This peer id can be used to retrieve users data from master server
        /// </summary>
        /// <param name="token"></param>
        /// <param name="callback"></param>
        public void ValidateAccess(string token, RoomAccessValidateCallback callback)
        {
            Mst.Server.Rooms.ValidateAccess(RoomId, token, callback, Connection);
        }
        
        /// <summary>
        /// Call this method when one of the players left current room
        /// </summary>
        /// <param name="peerId"></param>
        public void NotifyPlayerLeft(int peerId)
        {
            Mst.Server.Rooms.NotifyPlayerLeft(RoomId, peerId, (successful, error) =>
            {
                if (!successful)
                {
                    Logger.Error(error);
                }

                Logger.Info($"Player {peerId} left room");
            });
        }

        /// <summary>
        /// Default access provider, which always confirms access requests
        /// </summary>
        /// <param name="accessCheckOptions"></param>
        /// <param name="callback"></param>
        public void DefaultAccessProvider(RoomAccessProviderCheck accessCheckOptions, RoomAccessProviderCallback callback)
        {
            callback.Invoke(new RoomAccessPacket()
            {
                RoomId = RoomId,
                RoomIp = Options.RoomIp,
                RoomPort = Options.RoomPort,
                CustomOptions = Options.CustomOptions,
                Token = Mst.Helper.CreateGuidString(),
                SceneName = SceneManager.GetActiveScene().name
            }, null);
        }

        /// <summary>
        /// Makes the room public
        /// </summary>
        public void MakePublic()
        {
            Options.IsPublic = true;
            SaveOptions(Options);
        }

        /// <summary>
        /// Makes the room public
        /// </summary>
        public void MakePublic(Action callback)
        {
            Options.IsPublic = true;
            SaveOptions(Options, (successful, error) =>
            {
                callback.Invoke();
            });
        }

        #region MESSAGE HANDLERS

        private void ProvideRoomAccessCheckHandler(IIncomingMessage message)
        {
            var provideRoomAccessCheckPacket = message.Deserialize(new ProvideRoomAccessCheckPacket());
            var roomController = Mst.Server.Rooms.GetRoomController(provideRoomAccessCheckPacket.RoomId);

            if (roomController == null)
            {
                message.Respond($"There's no room controller with room id {provideRoomAccessCheckPacket.RoomId}", ResponseStatus.NotHandled);
                return;
            }

            var isProviderDone = false;

            var requester = new UsernameAndPeerIdPacket()
            {
                PeerId = provideRoomAccessCheckPacket.PeerId,
                Username = provideRoomAccessCheckPacket.Username
            };

            // Create access provider check options
            var roomAccessProviderCheck = new RoomAccessProviderCheck()
            {
                PeerId = provideRoomAccessCheckPacket.PeerId,
                Username = provideRoomAccessCheckPacket.Username,
                CustomOptions = provideRoomAccessCheckPacket.CustomOptions
            };

            // Invoke the access provider
            roomController.AccessProvider.Invoke(roomAccessProviderCheck, (access, error) =>
            {
                // In case provider timed out
                if (isProviderDone)
                {
                    return;
                }

                isProviderDone = true;

                if (access == null)
                {
                    // If access is not provided
                    message.Respond(string.IsNullOrEmpty(error) ? "" : error, ResponseStatus.Failed);
                    return;
                }

                message.Respond(access, ResponseStatus.Success);

                if (Logger.IsLogging(LogLevel.Trace))
                {
                    Logger.Trace("Room controller gave address to peer " + provideRoomAccessCheckPacket.PeerId + ":" + access);
                }
            });

            // Timeout the access provider
            MstTimer.WaitForSeconds(Mst.Server.Rooms.AccessProviderTimeout, () =>
            {
                if (!isProviderDone)
                {
                    isProviderDone = true;
                    message.Respond("Timed out", ResponseStatus.Timeout);
                    Logger.Error($"Access provider took longer than {Mst.Server.Rooms.AccessProviderTimeout} seconds to provide access. " +
                               "If it's intended, increase the threshold at Msf.Server.Rooms.AccessProviderTimeout");
                }
            });
        }

        #endregion
    }
}