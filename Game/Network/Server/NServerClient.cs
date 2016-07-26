using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Lidgren.Network;
using System;
using Shanism.Common.Util;

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
        INetReceptor gameReceptor;

        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public string Name { get; }


        public event Action<IOMessage> MessageSent;


        /// <summary>
        /// Gets whether the client is currently connected to a game server instance. 
        /// </summary>
        public bool IsPlaying => gameReceptor != null;


        public NServerClient(NetServer serv, NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            Id = GenericId<NServerClient>.GetNew();
            
            Name = name;
        }


        /// <summary>
        /// Hooks up the net client to the server receptor's events.
        /// </summary>
        public void Initialize(INetReceptor receptor)
        {
            gameReceptor = receptor;

            //attach to the game receptor events
            gameReceptor.MessageSent += GameReceptor_MessageSent;
        }


        public void Update(int msElapsed)
        {
            if(ConnectionHandle.Status == NetConnectionStatus.Disconnected)
            {
                Log.Default.Warning("Trying to update a closed client connection!");
                return;
            }

            //send gameframe
            var msg = gameReceptor.GetCurrentFrame();
            sendMessage(msg, NetDeliveryMethod.Unreliable);
        }


        #region Outgoing message handlers

        void GameReceptor_MessageSent(IOMessage msg)
        {
            switch(msg.Type)
            {
                //send most receptor messages directly via the network interface. 
                case MessageType.HandshakeReply:
                case MessageType.PlayerStatusUpdate:
                case MessageType.MapReply:
                case MessageType.ObjectUnseen:
                    sendMessage(msg);
                    break;

                // special handling for objectseen which contains 
                // a direct reference to a gameobject
                case MessageType.ObjectSeen:
                    //TODO: send both objectdata and objectseen
                    var seenMsg = (ObjectSeenMessage)msg;

                    sendMessage(seenMsg);
                    break;
            }
            sendMessage(msg);
        }
        #endregion


        internal void handleClientMessage(IOMessage msg)
        {
            Log.Default.Info($"{StringId} Received a {msg.Type}");
            MessageSent?.Invoke(msg);
        }


        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        void sendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            if(msg.Type != MessageType.GameFrame)
                Log.Default.Info($"{StringId} Sent a {msg.Type}");
        }

        string StringId => $"[#{Id}]";
    }
}
