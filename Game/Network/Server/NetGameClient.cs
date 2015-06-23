using IO;
using IO.Common;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using Lidgren.Network;
using System;
using System.Collections.Generic;
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
        public IGameReceptor GameReceptor { get; private set; }


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

        public void Initialize(IGameReceptor receptor)
        {
            GameReceptor = receptor;
            GameReceptor.ChunkReceived += GameReceptor_ChunkReceived;
        }


        public void Update(int msElapsed)
        {
            updatePlayerState();
        }

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

        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        public void SendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            Log.Info("Sent a {0} to {1}", msg.Type, ConnectionHandle.RemoteEndPoint.Address);
        }


        public void SendPlayerStatusUpdate()
        {
            IOMessage msg;
            if (GameReceptor.HasHero)
                msg = new PlayerStatusMessage(GameReceptor.MainHero);
            else
                msg = new PlayerStatusMessage(GameReceptor.CameraPosition);

            SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void HandleMapRequest(MapRequestMessage msg)
        {
            GameReceptor.RequestChunk(msg.Chunk);
        }

        public void HandleMoveUpdate(MoveMessage msg)
        {
            GameReceptor.MovementState = msg.Direction;
        }

        private void GameReceptor_ChunkReceived(MapChunkId arg1, TerrainType[,] arg2)
        {
            var msg = new MapReplyMessage(arg1, arg2);
            SendMessage(msg);
        }
    }
}
