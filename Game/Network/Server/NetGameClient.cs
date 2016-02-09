using IO;
using IO.Common;
using IO.Content;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using IO.Objects;
using IxLog;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using IO.Message.Network;

namespace Network.Server
{
    /// <summary>
    /// Represents a client connected over the network module. 
    /// <para/>
    /// Looks like a normal IGameClient to the ShanoEngine. 
    /// <para/>
    /// Tightly coupled with the network server. 
    /// </summary>
    public class NetGameClient : IClient
    {
        /// <summary>
        /// Gets the underlying NetConnection. 
        /// </summary>
        public NetConnection ConnectionHandle { get; }

        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the underlying network server. 
        /// </summary>
        public NetServer Server { get; }



        /// <summary>
        /// Gets the GameServer instance serving this connection
        /// </summary>
        public IReceptor GameReceptor { get; private set; }


        #region Events that inform our GameReceptor
        public event Action<MoveMessage> MovementStateChanged;
        public event Action<ActionMessage> ActionActivated;
        public event Action<ChatMessage> ChatMessageSent;
        public event Action<MapRequestMessage> MapRequested;
        public event Action HandshakeInit;
        #endregion


        /// <summary>
        /// Gets whether the client is currently connected to a game server instance. 
        /// </summary>
        public bool IsPlaying { get { return GameReceptor != null; } }



        public NetGameClient(NetServer serv, NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            Name = name;
        }

        public void Update(int msElapsed)
        {

        }

        /// <summary>
        /// Hooks up the net client to the server receptor's events.
        /// </summary>
        public void Initialize(IReceptor receptor)
        {
            GameReceptor = receptor;

            //attach to the game receptor events
            GameReceptor.MessageSent += GameReceptor_MessageSent;

            //GameReceptor.AnyUnitAction += GameReceptor_AnyUnitAction;
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

                    var objDatas = ObjectTracker.Default.GetBytes(seenMsg.Object);
                    var dataMsg = new ObjectDataMessage(seenMsg.Guid, seenMsg.Object.Type, objDatas);

                    sendMessage(seenMsg);
                    sendMessage(dataMsg);
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
            Log.Default.Info("Sent a '{0}' message to client {1}", msg.Type, ConnectionHandle.RemoteEndPoint.Address);
        }
    }
}
