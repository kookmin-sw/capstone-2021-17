﻿using MasterServerToolkit.Logging;
using MasterServerToolkit.Networking;
using System;
using UnityEngine;

namespace MasterServerToolkit.MasterServer
{
    public class MstAuthClient : MstBaseClient
    {
        public delegate void SignInCallback(ClientAccountInfo accountInfo, string error);

        /// <summary>
        /// Check if user is signed in
        /// </summary>
        public bool IsSignedIn { get; protected set; }

        /// <summary>
        /// Check if user is now logging in
        /// </summary>
        public bool IsNowSigningIn { get; protected set; }

        /// <summary>
        /// Remember user after he logged in
        /// </summary>
        public bool RememberMe { get; set; } = false;

        /// <summary>
        /// Current useraccount info
        /// </summary>
        public ClientAccountInfo AccountInfo { get; protected set; }

        /// <summary>
        /// Invokes when successfully signed in
        /// </summary>
        public event Action OnSignedInEvent;

        /// <summary>
        /// Invokes when successfully signed up
        /// </summary>
        public event Action OnSignedUpEvent;

        /// <summary>
        /// Invokes when successfully signed out
        /// </summary>
        public event Action OnSignedOutEvent;

        /// <summary>
        /// Invokes when email successfully confirmed
        /// </summary>
        public event Action OnEmailConfirmedEvent;

        /// <summary>
        /// Invokes when password successfully changed
        /// </summary>
        public event Action OnPasswordChangedEvent;

        /// <summary>
        /// Invokes when properties successfully changed
        /// </summary>
        public event Action OnPropertiesChangedEvent;

        public MstAuthClient(IClientSocket connection) : base(connection) { }

        /// <summary>
        /// Save authentication token
        /// </summary>
        private void SaveAuthToken(string token)
        {
            PlayerPrefs.SetString(MstDictKeys.USER_AUTH_TOKEN, token);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Clear authentication token
        /// </summary>
        private void ClearAuthToken()
        {
            if (HasAuthToken())
            {
                PlayerPrefs.DeleteKey(MstDictKeys.USER_AUTH_TOKEN);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// Check if we have auth token after last login in player prefs
        /// </summary>
        /// <returns></returns>
        public bool HasAuthToken()
        {
            if (PlayerPrefs.HasKey(MstDictKeys.USER_AUTH_TOKEN))
            {
                string key = PlayerPrefs.GetString(MstDictKeys.USER_AUTH_TOKEN);
                return !string.IsNullOrEmpty(key);
            }

            return false;
        }

        /// <summary>
        /// Sends a registration request to server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void SignUp(MstProperties data, SuccessCallback callback)
        {
            SignUp(data, callback, Connection);
        }

        /// <summary>
        /// Sends a registration request to given connection
        /// </summary>
        public void SignUp(MstProperties data, SuccessCallback callback, IClientSocket connection)
        {
            if (IsNowSigningIn)
            {
                callback.Invoke(false, "Signing in is already in progress");
                return;
            }

            if (IsSignedIn)
            {
                callback.Invoke(false, "Already signed in");
                return;
            }

            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected to server");
                return;
            }

            // We first need to get an aes key 
            // so that we can encrypt our login data
            Mst.Security.GetAesKey(aesKey =>
            {
                if (aesKey == null)
                {
                    callback.Invoke(false, "Failed to register due to security issues");
                    return;
                }

                var encryptedData = Mst.Security.EncryptAES(data.ToBytes(), aesKey);

                connection.SendMessage((short)MstMessageCodes.SignUp, encryptedData, (status, response) =>
                {
                    if (status != ResponseStatus.Success)
                    {
                        callback.Invoke(false, response.AsString("Unknown error"));
                        return;
                    }

                    callback.Invoke(true, null);

                    OnSignedUpEvent?.Invoke();
                });
            }, connection);
        }

        /// <summary>
        /// Initiates a log out. In the process, disconnects and connects
        /// back to the server to ensure no state data is left on the server.
        /// </summary>
        /// <param name="permanent">If you wish to delete auth token</param>
        public void SignOut(bool permanent = false)
        {
            SignOut(Connection, permanent);
        }

        /// <summary>
        /// Initiates a log out. In the process, disconnects and connects
        /// back to the server to ensure no state data is left on the server.
        /// </summary>
        public void SignOut(IClientSocket connection, bool permanent = false)
        {
            if (!IsSignedIn)
            {
                return;
            }

            IsSignedIn = false;
            AccountInfo = null;

            if (permanent)
                ClearAuthToken();

            if (connection != null && connection.IsConnected)
            {
                connection.Reconnect();
            }

            OnSignedOutEvent?.Invoke();
        }

        /// <summary>
        /// Sends a request to server, to log in as a guest
        /// </summary>
        /// <param name="callback"></param>
        public void SignInAsGuest(SignInCallback callback)
        {
            SignInAsGuest(callback, Connection);
        }

        /// <summary>
        /// Sends a request to server, to log in as a guest
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="connection"></param>
        public void SignInAsGuest(SignInCallback callback, IClientSocket connection)
        {
            var credentials = new MstProperties();
            credentials.Add(MstDictKeys.USER_IS_GUEST);
            credentials.Add(MstDictKeys.USER_DEVICE_NAME, SystemInfo.deviceName);
            credentials.Add(MstDictKeys.USER_DEVICE_ID, SystemInfo.deviceUniqueIdentifier);

            SignIn(credentials, callback, connection);
        }

        /// <summary>
        /// Sends a request to server, to log in with auth token
        /// </summary>
        /// <param name="callback"></param>
        public void SignInWithToken(SignInCallback callback)
        {
            SignInWithToken(callback, Connection);
        }

        /// <summary>
        /// Sends a request to server, to log in with auth token
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="connection"></param>
        public void SignInWithToken(SignInCallback callback, IClientSocket connection)
        {
            if (!HasAuthToken())
            {
                throw new Exception("You have no auth token!");
            }

            SignInWithToken(PlayerPrefs.GetString(MstDictKeys.USER_AUTH_TOKEN), callback, connection);
        }

        /// <summary>
        /// Sends a login request, using given token
        /// </summary>
        public void SignInWithToken(string token, SignInCallback callback)
        {
            SignInWithToken(token, callback, Connection);
        }

        /// <summary>
        /// Sends a login request, using given token
        /// </summary>
        public void SignInWithToken(string token, SignInCallback callback, IClientSocket connection)
        {
            var credentials = new MstProperties();
            credentials.Add(MstDictKeys.USER_AUTH_TOKEN, token);

            SignIn(credentials, callback, connection);
        }

        /// <summary>
        /// Sends a login request, using given credentials
        /// </summary>
        public void SignInWithLoginAndPassword(string username, string password, SignInCallback callback)
        {
            SignInWithLoginAndPassword(username, password, callback, Connection);
        }

        /// <summary>
        /// Sends a login request, using given credentials
        /// </summary>
        public void SignInWithLoginAndPassword(string username, string password, SignInCallback callback, IClientSocket connection)
        {
            var credentials = new MstProperties();
            credentials.Add(MstDictKeys.USER_NAME, username);
            credentials.Add(MstDictKeys.USER_PASSWORD, password);

            SignIn(credentials, callback, connection);
        }

        /// <summary>
        /// Sends a login request, using given credentials
        /// </summary>
        public void SignInWithEmail(string email, SignInCallback callback)
        {
            SignInWithEmail(email, callback, Connection);
        }

        /// <summary>
        /// Sends a login request, using given credentials
        /// </summary>
        public void SignInWithEmail(string email, SignInCallback callback, IClientSocket connection)
        {
            var credentials = new MstProperties();
            credentials.Add(MstDictKeys.USER_EMAIL, email);

            SignIn(credentials, callback, connection);
        }

        /// <summary>
        /// Sends a login request, using given credentials
        /// </summary>
        public void SignInWithPhoneNumber(string phoneNumber, SignInCallback callback)
        {
            SignInWithPhoneNumber(phoneNumber, callback, Connection);
        }

        /// <summary>
        /// Sends a login request, using given credentials
        /// </summary>
        public void SignInWithPhoneNumber(string phoneNumber, SignInCallback callback, IClientSocket connection)
        {
            var credentials = new MstProperties();
            credentials.Add(MstDictKeys.USER_PHONE_NUMBER, phoneNumber);

            SignIn(credentials, callback, connection);
        }

        /// <summary>
        /// Sends a generic login request
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void SignIn(MstProperties data, SignInCallback callback)
        {
            SignIn(data, callback, Connection);
        }

        /// <summary>
        /// Sends a generic login request
        /// </summary>
        public void SignIn(MstProperties data, SignInCallback callback, IClientSocket connection)
        {
            Logs.Debug("Signing in...");

            if (!connection.IsConnected)
            {
                callback.Invoke(null, "Not connected to server");
                return;
            }

            IsNowSigningIn = true;

            // We first need to get an aes key 
            // so that we can encrypt our login data
            Mst.Security.GetAesKey(aesKey =>
            {
                if (aesKey == null)
                {
                    IsNowSigningIn = false;
                    callback.Invoke(null, "Failed to log in due to security issues");
                    return;
                }

                var encryptedData = Mst.Security.EncryptAES(data.ToBytes(), aesKey);

                connection.SendMessage((short)MstMessageCodes.SignIn, encryptedData, (status, response) =>
                {
                    IsNowSigningIn = false;

                    if (status != ResponseStatus.Success)
                    {
                        ClearAuthToken();

                        callback.Invoke(null, response.AsString("Unknown error"));
                        return;
                    }

                    // Parse account info
                    var accountInfoPacket = response.Deserialize(new AccountInfoPacket());

                    AccountInfo = new ClientAccountInfo()
                    {
                        Id = accountInfoPacket.Id,
                        Username = accountInfoPacket.Username,
                        Email = accountInfoPacket.Email,
                        PhoneNumber = accountInfoPacket.PhoneNumber,
                        Facebook = accountInfoPacket.Facebook,
                        Token = accountInfoPacket.Token,
                        IsAdmin = accountInfoPacket.IsAdmin,
                        IsGuest = accountInfoPacket.IsGuest,
                        IsEmailConfirmed = accountInfoPacket.IsEmailConfirmed,
                        Properties = accountInfoPacket.Properties,
                    };

                    // If RememberMe is checked on and we are not guset, then save auth token
                    if (RememberMe && !AccountInfo.IsGuest && !string.IsNullOrEmpty(AccountInfo.Token))
                    {
                        SaveAuthToken(AccountInfo.Token);
                    }
                    else
                    {
                        ClearAuthToken();
                    }

                    IsSignedIn = true;

                    callback.Invoke(AccountInfo, null);

                    OnSignedInEvent?.Invoke();
                });
            }, connection);
        }

        /// <summary>
        /// Sends an e-mail confirmation code to the server
        /// </summary>
        /// <param name="code"></param>
        /// <param name="callback"></param>
        public void ConfirmEmail(string code, SuccessCallback callback)
        {
            ConfirmEmail(code, callback, Connection);
        }

        /// <summary>
        /// Sends an e-mail confirmation code to the server
        /// </summary>
        public void ConfirmEmail(string code, SuccessCallback callback, IClientSocket connection)
        {
            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected to server");
                return;
            }

            if (!IsSignedIn)
            {
                callback.Invoke(false, "You're not logged in");
                return;
            }

            connection.SendMessage((short)MstMessageCodes.ConfirmEmail, code, (status, response) =>
            {
                if (status != ResponseStatus.Success)
                {
                    callback.Invoke(false, response.AsString("Unknown error"));
                    return;
                }

                callback.Invoke(true, null);

                OnEmailConfirmedEvent?.Invoke();
            });
        }

        /// <summary>
        /// Sends a request to server, to ask for an e-mail confirmation code
        /// </summary>
        /// <param name="callback"></param>
        public void RequestEmailConfirmationCode(SuccessCallback callback)
        {
            RequestEmailConfirmationCode(callback, Connection);
        }

        /// <summary>
        /// Sends a request to server, to ask for an e-mail confirmation code
        /// </summary>
        public void RequestEmailConfirmationCode(SuccessCallback callback, IClientSocket connection)
        {
            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected to server");
                return;
            }

            if (!IsSignedIn)
            {
                callback.Invoke(false, "You're not logged in");
                return;
            }

            connection.SendMessage((short)MstMessageCodes.GetEmailConfirmationCode, (status, response) =>
            {
                if (status != ResponseStatus.Success)
                {
                    callback.Invoke(false, response.AsString("Unknown error"));
                    return;
                }

                callback.Invoke(true, null);
            });
        }

        /// <summary>
        /// Sends a request to server, to ask for a password reset
        /// </summary>
        public void RequestPasswordReset(string email, SuccessCallback callback)
        {
            RequestPasswordReset(email, callback, Connection);
        }

        /// <summary>
        /// Sends a request to server, to ask for a password reset
        /// </summary>
        public void RequestPasswordReset(string email, SuccessCallback callback, IClientSocket connection)
        {
            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected to server");
                return;
            }

            connection.SendMessage((short)MstMessageCodes.GetPasswordResetCode, email, (status, response) =>
            {
                if (status != ResponseStatus.Success)
                {
                    callback.Invoke(false, response.AsString("Unknown error"));
                    return;
                }

                callback.Invoke(true, null);
            });
        }

        /// <summary>
        /// Sends a new password to server
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <param name="newPassword"></param>
        /// <param name="callback"></param>
        public void ChangePassword(string email, string code, string newPassword, SuccessCallback callback)
        {
            var options = new MstProperties();
            options.Add("email", email);
            options.Add("code", code);
            options.Add("password", newPassword);

            ChangePassword(options, callback, Connection);
        }

        /// <summary>
        /// Sends a new password to server
        /// </summary>
        public void ChangePassword(MstProperties data, SuccessCallback callback, IClientSocket connection)
        {
            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected to server");
                return;
            }

            connection.SendMessage((short)MstMessageCodes.ChangePassword, data.ToBytes(), (status, response) =>
            {
                if (status != ResponseStatus.Success)
                {
                    callback.Invoke(false, response.AsString("Unknown error"));
                    return;
                }

                callback.Invoke(true, null);

                OnPasswordChangedEvent?.Invoke();
            });
        }

        /// <summary>
        /// Send request to update account properties
        /// </summary>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void UpdateAccountInfo(SuccessCallback callback)
        {
            UpdateAccountInfo(callback, Connection);
        }

        /// <summary>
        /// Send request to update account properties
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="connection"></param>
        public void UpdateAccountInfo(SuccessCallback callback, IClientSocket connection)
        {
            if (!connection.IsConnected)
            {
                callback.Invoke(false, "Not connected to server");
                return;
            }

            // We first need to get an aes key 
            // so that we can encrypt our login data
            Mst.Security.GetAesKey(aesKey =>
            {
                if (aesKey == null)
                {
                    callback.Invoke(false, "Failed to update properties due to security issues");
                    return;
                }

                var updateAccountPropertiesPacket = new UpdateAccountInfoPacket
                {
                    Id = AccountInfo.Id,
                    Username = AccountInfo.Username,
                    Password = AccountInfo.Password,
                    Email = AccountInfo.Email,
                    PhoneNumber = AccountInfo.PhoneNumber,
                    Facebook = AccountInfo.Facebook,
                    Properties = AccountInfo.Properties
                };

                var encryptedData = Mst.Security.EncryptAES(updateAccountPropertiesPacket.ToBytes(), aesKey);

                connection.SendMessage((short)MstMessageCodes.UpdateAccountInfo, encryptedData, (status, response) =>
                {
                    IsNowSigningIn = false;

                    if (status != ResponseStatus.Success)
                    {
                        callback.Invoke(false, response.AsString("Unknown error"));
                        return;
                    }

                    AccountInfo.MarkAsDirty(false);

                    callback.Invoke(true, null);

                    OnPropertiesChangedEvent?.Invoke();
                });
            }, connection);
        }
    }
}