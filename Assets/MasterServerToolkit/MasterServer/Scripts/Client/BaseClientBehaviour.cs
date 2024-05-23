﻿using MasterServerToolkit.Logging;
using MasterServerToolkit.Networking;
using System.Collections.Generic;
using UnityEngine;

namespace MasterServerToolkit.MasterServer
{
    public abstract class BaseClientBehaviour : MonoBehaviour, IMstBaseClient
    {
        /// <summary>
        /// Client handlers list. Requires for connection changing process. <seealso cref="ChangeConnection(IClientSocket)"/>
        /// </summary>
        protected Dictionary<short, IPacketHandler> handlers;

        /// <summary>
        /// Logger assigned to this module
        /// </summary>
        protected Logging.Logger logger;

        /// <summary>
        /// Log levelof this module
        /// </summary>
        [Header("Base Settings"), SerializeField]
        protected LogLevel logLevel = LogLevel.Info;
        [SerializeField]
        protected bool initModulesAtStart = true;

        /// <summary>
        /// Current module connection
        /// </summary>
        public IClientSocket Connection { get; protected set; }

        /// <summary>
        /// Check if current module is connected to server
        /// </summary>
        public bool IsConnected => Connection != null && Connection.IsConnected;

        protected virtual void Awake()
        {
            handlers = new Dictionary<short, IPacketHandler>();

            logger = Mst.Create.Logger(GetType().Name);
            logger.LogLevel = logLevel;
        }

        protected virtual void Start()
        {
            ChangeConnection(ConnectionFactory());
            OnInitialize();

            if (initModulesAtStart)
                InitializeAllModules();
        }

        protected void InitializeAllModules()
        {
            foreach (IBaseClientModule module in GetComponentsInChildren<IBaseClientModule>())
            {
                module.ClientBehaviour = this;
                module.OnInitialize(this);
            }
        }

        protected virtual void OnDestroy()
        {
            ClearConnection();
        }

        /// <summary>
        /// Returns the connection to server
        /// </summary>
        /// <returns></returns>
        protected virtual IClientSocket ConnectionFactory()
        {
            return Mst.Client.Connection;
        }

        /// <summary>
        /// Clears connection and all its handlers if <paramref name="clearHandlers"/> is true
        /// </summary>
        public virtual void ClearConnection(bool clearHandlers = true)
        {
            if (Connection != null)
            {
                if (handlers != null && clearHandlers)
                {
                    foreach (var handler in handlers.Values)
                    {
                        Connection.RemoveHandler(handler);
                    }

                    handlers.Clear();
                }

                Connection.OnStatusChangedEvent -= OnConnectionStatusChanged;
            }
        }

        /// <summary>
        /// Sets a message handler to connection, which is used by this this object
        /// to communicate with server
        /// </summary>
        /// <param name="handler"></param>
        public void SetHandler(IPacketHandler handler)
        {
            handlers[handler.OpCode] = handler;
            Connection?.SetHandler(handler);
        }

        /// <summary>
        /// Sets a message handler to connection, which is used by this this object
        /// to communicate with server 
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="handler"></param>
        public void SetHandler(short opCode, IncommingMessageHandler handler)
        {
            SetHandler(new PacketHandler(opCode, handler));
        }

        /// <summary>
        /// Removes the packet handler, but only if this exact handler
        /// was used
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveHandler(IPacketHandler handler)
        {
            Connection?.RemoveHandler(handler);
        }

        /// <summary>
        /// Changes the connection object, and sets all of the message handlers of this object
        /// to new connection.
        /// </summary>
        /// <param name="socket"></param>
        public void ChangeConnection(IClientSocket socket)
        {
            // Clear just connection but not handlers
            ClearConnection(false);

            // Change connections
            Connection = socket;

            // Override packet handlers
            foreach (var packetHandler in handlers.Values)
            {
                socket.SetHandler(packetHandler);
            }

            Connection.OnStatusChangedEvent += OnConnectionStatusChanged;
            OnConnectionSocketChanged(Connection);
        }

        /// <summary>
        /// Cast this client behaviour to derived class <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CastTo<T>() where T: class
        {
            return this as T;
        }

        /// <summary>
        /// Fires when this module is started
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Fires when connection of this module is changing
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void OnConnectionSocketChanged(IClientSocket socket) { }

        /// <summary>
        /// Fires each time the connection status is changing
        /// </summary>
        /// <param name="status"></param>
        protected virtual void OnConnectionStatusChanged(ConnectionStatus status) { }
    }
}
