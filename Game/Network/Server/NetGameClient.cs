using IO;
using IO.Common;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using IO.Objects;
using IxLog;
using IxSerializer;
using Lidgren.Network;
using Network.Objects.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public readonly NetConnection ConnectionHandle;

        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public readonly string ClientName;

        /// <summary>
        /// Gets the underlying network server. 
        /// </summary>
        public readonly NetServer Server;

        /// <summary>
        /// Gets whether the client is currently connected to a game server instance. 
        /// </summary>
        public bool IsPlaying { get { return GameReceptor != null; } }

        /// <summary>
        /// Gets the GameServer instance serving this connection
        /// </summary>
        public IReceptor GameReceptor { get; private set; }


        string IClient.Name
        {
            get { return ClientName; }
        }
        

        #region Receptor events
        public event Action<MoveMessage> MovementStateChanged;
        public event Action<ActionMessage> ActionActivated;
        public event Action<ChatMessage> ChatMessageSent;
        public event Action<MapRequestMessage> MapRequested;
        public event Action HandshakeInit;
        #endregion

        public NetGameClient(NetServer serv, NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            ClientName = name;
        }

        public void Update(int msElapsed)
        {

        }

        public void SendHandshake(bool accepted)
        {
            var ioMsg = new HandshakeReplyMessage(accepted);
            sendMessage(ioMsg);
        }


        /// <summary>
        /// Hooks up the net client to the server receptor's events.
        /// </summary>
        public void Initialize(IReceptor receptor)
        {
            GameReceptor = receptor;

            //attach to the game receptor events
            GameReceptor.AnyUnitAction += GameReceptor_AnyUnitAction;
            GameReceptor.HandshakeReplied += GameReceptor_ConnectionChanged;

            GameReceptor.MainHeroChanged += GameReceptor_MainHeroChanged;
            GameReceptor.MapChunkReceived += GameReceptor_MapChunkReceived;

            GameReceptor.ObjectSeen += GameReceptor_ObjectSeen;
            GameReceptor.ObjectUnseen += GameReceptor_ObjectUnseen;
        }


        #region Outgoing message handlers
        private void GameReceptor_MapChunkReceived(MapReplyMessage msg)
        {
            sendMessage(msg);
        }

        private void GameReceptor_ConnectionChanged(HandshakeReplyMessage msg)
        {
            sendMessage(msg);
        }


        private void GameReceptor_ObjectSeen(IGameObject obj)
        {
            sendObjectSeenMessage(obj);
        }

        private void GameReceptor_AnyUnitAction(IUnit obj, string actionId)
        {
            throw new NotImplementedException();
        }

        private void GameReceptor_MainHeroChanged(PlayerStatusMessage msg)
        {
            sendMessage(msg);
        }

        private void GameReceptor_ObjectUnseen(IGameObject obj)
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
        private void sendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            Log.Default.Info("Sent a {0} to {1}", msg.Type, ConnectionHandle.RemoteEndPoint.Address);
        }

        private void sendObjectSeenMessage(IGameObject obj)
        {
            var sendAsType = obj.ObjectType;
            var objData = Serializer.GetWriter(w =>
                InterfaceSerializer.WriteInterfaceData(w, sendAsType.UnderlyingInterface, obj, skipUnknownFields: true));
            var msg = new ObjectSeenMessage(sendAsType, obj.Guid, objData);

            sendMessage(msg);
        }
    }
}
