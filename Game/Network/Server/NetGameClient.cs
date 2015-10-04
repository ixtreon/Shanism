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
    public class NetGameClient : IGameClient
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
        public INetworkReceptor GameReceptor { get; private set; }


        string IGameClient.Name
        {
            get { return ClientName; }
        }


        #region State Tracking

        private IHero lastPlayerHero = null;

        #endregion


        public NetGameClient(NetServer serv, NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            ClientName = name;
        }

        public void Initialize(INetworkReceptor receptor)
        {
            GameReceptor = receptor;
            GameReceptor.ChunkReceived += GameReceptor_ChunkReceived;
            GameReceptor.ObjectInVisionRange += GameReceptor_ObjectInVisionRange;
        }

        public void Update(int msElapsed)
        {
            updatePlayerState();
        }

        #region IOMessage Handlers
        public void SendPlayerStatusUpdate()
        {
            IOMessage msg;
            if (GameReceptor.HasHero)
            {
                msg = new PlayerStatusMessage(GameReceptor.MainHero);
                sendObject(GameReceptor.MainHero);
            }
            else
                msg = new PlayerStatusMessage(GameReceptor.CameraPosition);

            sendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void HandleMapRequest(MapRequestMessage msg)
        {
            GameReceptor.RequestChunk(msg.Chunk);
        }

        public void HandleMoveUpdate(MoveMessage msg)
        {
            GameReceptor.MovementState = msg.Direction;
        }

        public void HandleHandshake(bool accepted)
        {
            var ioMsg = new HandshakeReplyMessage(accepted);
            sendMessage(ioMsg);
        }
        #endregion

        #region GameReceptor event handlers
        private void GameReceptor_ChunkReceived(MapChunkId arg1, TerrainType[,] arg2)
        {
            var msg = new MapReplyMessage(arg1, arg2);
            sendMessage(msg);
        }

        private void GameReceptor_ObjectInVisionRange(IGameObject obj)
        {
            Console.WriteLine("{0} came in range!", obj);

            sendObject(obj);
        }
        #endregion

        #region GameReceptor trackers
        /// <summary>
        /// Informs the client whenever its hero has changed. 
        /// </summary>
        private void updatePlayerState()
        {
            var playerHero = GameReceptor.MainHero;
            if (lastPlayerHero != playerHero)
            {
                SendPlayerStatusUpdate();

                lastPlayerHero = playerHero;
            }
        }
        #endregion


        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        private void sendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            Log.Default.Info("Sent a {0} to {1}", msg.Type, ConnectionHandle.RemoteEndPoint.Address);
        }

        private void sendObject(IGameObject obj)
        {
            sendObject(obj, obj.ObjectType);
        }

        private void sendObject(IGameObject obj, ObjectType sendAsType)
        {
            var objData = Serializer.GetWriter(w =>
                Serializer.WriteInterfaceData(w, sendAsType.UnderlyingInterface, obj, skipUnknownFields: true));
            var msg = new ObjectSeenMessage(sendAsType, obj.Guid, objData);

            sendMessage(msg);
        }
    }
}
