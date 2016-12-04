﻿using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Lidgren.Network;
using System;
using Shanism.Common.Util;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Network;
using Shanism.Network.Common;

namespace Shanism.Network.Server
{
    /// <summary>
    /// Represents a client connected over the network module. 
    /// <para/>
    /// Looks like a normal IGameClient to the ShanoEngine. 
    /// <para/>
    /// Tightly coupled with the network server. 
    /// </summary>
    public class NServerClient : IShanoClient
    {
        readonly ServerFrameBuilder serverFrameWriter = new ServerFrameBuilder();
        readonly ClientFrameBuilder clientFrameReader = new ClientFrameBuilder();


        /// <summary>
        /// Gets the underlying NetConnection. 
        /// </summary>
        readonly NetConnection ConnectionHandle;

        /// <summary>
        /// Gets the underlying network server. 
        /// </summary>
        readonly NetServer Server;

        readonly uint Id;

        /// <summary>
        /// The receptor serving this connection
        /// </summary>
        IReceptor gameReceptor;

        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public string Name { get; }

        uint LastAckFrame { get; set; }

        public ClientState State { get; private set; } = new ClientState();

        public event Action<IOMessage> MessageSent;


        /// <summary>
        /// Gets whether the client is currently connected to a game server instance. 
        /// </summary>
        public bool IsPlaying => gameReceptor != null;


        public NServerClient(NetServer serv, 
            NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            Id = GenericId<NServerClient>.GetNew();
            
            Name = name;
        }


        /// <summary>
        /// Hooks up the net client to the server receptor's events.
        /// </summary>
        public void Initialize(IReceptor receptor)
        {
            gameReceptor = receptor;

            //attach to the game receptor events
            gameReceptor.MessageSent += sendMessage;
        }


        public void Update(int msElapsed)
        {
            if(ConnectionHandle.Status == NetConnectionStatus.Disconnected)
            {
                Log.Default.Warning("Trying to update a closed client connection!");
                return;
            }

            //send a GameFrame to the client
            var msg = serverFrameWriter.Write(gameReceptor);
            sendMessage(msg, NetDeliveryMethod.Unreliable);
        }


        #region Outgoing message handlers

        void GameReceptor_MessageSent(IOMessage msg)
        {
            sendMessage(msg);
        }
        #endregion


        internal void handleClientMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.GameFrame:
                    ClientState newState;
                    uint lastFrame;
                    if (clientFrameReader.TryRead((GameFrameMessage)msg, out lastFrame, out newState))
                    {
                        State = newState;
                        LastAckFrame = lastFrame;
                    }
                    break;

                default:
                    Log.Default.Info($"[{Id}] Received a {msg.Type}");
                    MessageSent?.Invoke(msg);
                    break;
            }
        }


        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        void sendMessage(IOMessage msg)
            => sendMessage(msg, NetDeliveryMethod.ReliableUnordered);

        void sendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            if(msg.Type != MessageType.GameFrame)
                Log.Default.Info($"[{Id}] Sent a {msg.Type}");
        }
    }
}
