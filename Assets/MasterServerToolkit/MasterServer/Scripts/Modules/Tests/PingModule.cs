﻿using MasterServerToolkit.Networking;
using UnityEngine;

namespace MasterServerToolkit.MasterServer
{
    public class PingModule : BaseServerModule
    {
        [SerializeField, TextArea(3, 5)]
        private string pongMessage = "Hello, Pong!";

        public override void Initialize(IServer server)
        {
            server.RegisterMessageHandler((short)MstMessageCodes.Ping, OnPingRequestListener);
        }

        private void OnPingRequestListener(IIncomingMessage message)
        {
            message.Respond(pongMessage, ResponseStatus.Success);
        }
    }
}