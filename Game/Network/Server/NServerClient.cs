using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Lidgren.Network;
using System;
using Shanism.Common.Util;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Network;
using Shanism.Network.Common;
using System.IO;

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

        readonly ClientStateTracker stateTracker = new ClientStateTracker(8);

        /// <summary>
        /// The receptor serving this connection
        /// </summary>
        IReceptor gameReceptor;

        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public string Name { get; }

        uint LastAckFrame { get; set; }

        public PlayerState State { get; private set; } = new PlayerState();

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


        public void Update(int msElapsed, uint gameFrame)
        {
            if(ConnectionHandle.Status == NetConnectionStatus.Disconnected)
            {
                Log.Default.Warning("Trying to update a closed client connection!");
                return;
            }

            //send a GameFrame to the client
            var msg = Server.CreateMessage();
            msg.Write((byte)1);
            stateTracker.WriteFrame(msg, gameFrame, gameReceptor.VisibleEntities);

            Server.SendMessage(msg, ConnectionHandle, NetDeliveryMethod.Unreliable);
        }


        #region Outgoing message handlers

        void GameReceptor_MessageSent(IOMessage msg)
        {
            sendMessage(msg);
        }
        #endregion


        internal void handleProtoMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.GameFrame:
                    break;

                default:
                    //Log.Default.Info($"[{Id}] Received a {msg.Type}");
                    MessageSent?.Invoke(msg);
                    break;
            }
        }

        internal void readClientFrame(NetIncomingMessage msg)
        {
            uint frameId = 0;
            PlayerState state = null;

            try
            {
                frameId = msg.ReadUInt32();
                using (var ms = new MemoryStream(msg.Data, msg.PositionInBytes, msg.LengthBytes - msg.PositionInBytes))
                    state = ProtoBuf.Serializer.Deserialize<PlayerState>(ms);
            }
            catch
            {
                //invalid frame: do not do anything, hope we get a proper frame eventually
                Log.Default.Warning($"Unable to deserialize a frame from {msg.SenderEndPoint} (length: {msg.LengthBytes} bytes)");
            }

            //try
            //{
                stateTracker.SetLastAck(frameId);
            //}
            //catch (Exception e)
            //{
            //    Log.Default.Warning($"Client {msg.SenderEndPoint} sent invalid frame:\n{e}");
            //}
            this.State = state;
        }

        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        void sendMessage(IOMessage msg)
            => sendMessage(msg, NetDeliveryMethod.ReliableUnordered);

        void sendMessage(IOMessage msg, NetDeliveryMethod deliveryMethod)
        {
            Server.SendMessage(msg.ToNetMessage(Server), ConnectionHandle, deliveryMethod);
            //Log.Default.Info($"[{Id}] Sent a {msg.Type}");
        }
    }
}
