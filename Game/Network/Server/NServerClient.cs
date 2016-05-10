using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Server;
using Shanism.Common.Objects;
using IxLog;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using Shanism.Common.Message.Network;

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
        NetConnection ConnectionHandle { get; }

        /// <summary>
        /// Gets the underlying network server. 
        /// </summary>
        NetServer Server { get; }


        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// The receptor serving this connection
        /// </summary>
        INetReceptor gameReceptor;

        bool hasDiconnected = false;


        #region IShanoClient implementation
        public event Action<MoveMessage> MovementStateChanged;
        public event Action<ActionMessage> ActionActivated;
        public event Action<ChatMessage> ChatMessageSent;
        public event Action<MapRequestMessage> MapRequested;
        public event Action HandshakeInit;
        #endregion


        /// <summary>
        /// Gets whether the client is currently connected to a game server instance. 
        /// </summary>
        public bool IsPlaying { get { return gameReceptor != null; } }


        public NServerClient(NetServer serv, NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            
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

        void GameReceptor_AnyUnitAction(IUnit obj, string actionId)
        {
            throw new NotImplementedException();
        }
        #endregion


        internal void HandleMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.MapRequest:    // client wants map chunks
                    MapRequested((MapRequestMessage)msg);
                    break;
                case MessageType.MoveUpdate:    // client wants to move
                    MovementStateChanged((MoveMessage)msg);
                    break;
                case MessageType.SendChat:      // client wants to chat
                    ChatMessageSent((ChatMessage)msg);
                    break;
                case MessageType.Action:        // client wants to do stuff
                    ActionActivated((ActionMessage)msg);
                    break;
            }
        }


        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        void sendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            Log.Default.Debug("Sent a '{0}' message to client {1}", msg.Type, ConnectionHandle.RemoteEndPoint.Address);
        }
    }
}
