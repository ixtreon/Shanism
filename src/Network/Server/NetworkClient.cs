using Shanism.Common;
using Shanism.Common.Messages;
using Lidgren.Network;
using System;
using Shanism.Common.Util;
using System.IO;
using ProtoBuf;

namespace Shanism.Network.Server
{
    /// <summary>
    /// Represents a client connected over the network module. 
    /// <para/>
    /// Looks like a normal IGameClient to the ShanoEngine. 
    /// <para/>
    /// Tightly coupled with the network server. 
    /// </summary>
    public class NetworkClient : IClientReceptor
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
        IEngineReceptor gameReceptor;

        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        public string Name { get; }

        uint LastAckFrame { get; set; }

        public PlayerState PlayerState { get; private set; } = new PlayerState();



        /// <summary>
        /// Gets whether the client is currently connected to a game server instance. 
        /// </summary>
        public bool IsPlaying => gameReceptor != null;


        public NetworkClient(NetServer serv,
            NetConnection conn, string name)
        {
            Server = serv;
            ConnectionHandle = conn;
            Id = GenericId<NetworkClient>.GetNew();

            Name = name;
        }


        /// <summary>
        /// Hooks up the net client to the server receptor's events.
        /// </summary>
        public void Initialize(IEngineReceptor receptor)
        {
            gameReceptor = receptor;
        }

        /// <summary>
        /// Handles a server message by sending it via the network.
        /// </summary>
        public void HandleMessage(ServerMessage msg)
        {
            var netMessage = msg.ToNetMessage(Server);
            sendMessageToClient(netMessage, NetDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        /// Reads a message coming by the client (on the other side of the network) 
        /// by passing it to the receptor.
        /// </summary>
        /// <param name="netMsg"></param>
        public void ReadServerMessage(NetIncomingMessage netMsg)
        {
            var msgType = (NetMessageHeader)netMsg.ReadByte();
            switch(msgType)
            {
                case NetMessageHeader.ProtoMessage:
                    var ioMsg = netMsg.ProtoDeserialize<ClientMessage>();
                    if(ioMsg == null)
                    {
                        NetLog.Default.Warning($"Bad proto-message content ({netMsg.LengthBytes}B).");
                        return;
                    }

                    gameReceptor.HandleMessage(ioMsg);
                    break;

                case NetMessageHeader.GameFrame:
                    ReadClientFrame(netMsg);
                    break;

                default:
                    NetLog.Default.Warning($"Bad message header ({netMsg.LengthBytes}B).");
                    break;
            }

        }

        public void Update(int msElapsed, uint gameFrame)
        {
            if(ConnectionHandle.Status == NetConnectionStatus.Disconnected)
            {
                NetLog.Default.Warning("Trying to update a closed client connection!");
                return;
            }

            //send a GameFrame to the client
            var msg = Server.CreateMessage();
            msg.Write((byte)NetMessageHeader.GameFrame);
            stateTracker.WriteFrame(msg, gameFrame, gameReceptor.VisibleEntities);

            sendMessageToClient(msg, NetDeliveryMethod.Unreliable);
        }

        internal void ReadClientFrame(NetIncomingMessage msg)
        {
            uint frameId = 0;
            PlayerState state = null;

            try
            {
                frameId = msg.ReadUInt32();
                using(var ms = new MemoryStream(msg.Data, msg.PositionInBytes, msg.LengthBytes - msg.PositionInBytes))
                    state = Serializer.Deserialize<PlayerState>(ms);
            }
            catch
            {
                //invalid frame: do not do anything, hope we get a proper frame eventually
                NetLog.Default.Warning($"Unable to deserialize a frame from {msg.SenderEndPoint} (length: {msg.LengthBytes} bytes)");
            }

            stateTracker.SetLastAck(frameId);
            PlayerState = state;
        }

        /// <summary>
        /// Sends the given message to the game client. 
        /// </summary>
        void sendMessageToClient(NetOutgoingMessage msg, NetDeliveryMethod deliveryMethod)
        {
            Server.SendMessage(msg, ConnectionHandle, deliveryMethod);
        }
    }
}
