using Shanism.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Common.Messages;
using Shanism.Common.Entities;
using System.IO;
using Shanism.Network.Common;
using ProtoBuf;

namespace Shanism.Network.Client
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Represents the IReceptor assigned to the <see cref="NetworkEngine.GameClient"/>. 
    /// <para/>
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class NReceptor : IEngineReceptor
    {
        const int StateRefreshFps = 60;

        readonly Counter stateRefreshCounter = new Counter(1000 / StateRefreshFps);

        readonly NetworkEngine server;
        readonly IClientReceptor client;

        readonly ObjectCache objects = new ObjectCache(8);

        // WILL crash if sizeof(PlayerState) > size of the array
        readonly byte[] gameFrameBuffer = new byte[1024];

        int gameFrameBufferLength;


        public uint PlayerId { get; private set; }

        public ClientState State { get; private set; }

        public bool IsHost => false;

        public IReadOnlyCollection<IEntity> VisibleEntities => objects.VisibleEntities;


        public NReceptor(NetworkEngine server, IClientReceptor client)
        {
            this.server = server;
            this.client = client;

            State = ClientState.Connecting;
        }

        public bool TrySendHandshake()
        {
            if(State != ClientState.Connecting)
                return false;

            server.SendMessageReliable(new HandshakeInit(server.GameClient.Name));
            State = ClientState.AwaitingHandshake;
            return true;
        }

        void parseHandshake(HandshakeReply msg)
        {
            if(!msg.Success)
                return;

            State = ClientState.Playing;
            PlayerId = msg.PlayerId;
        }

        public void Disconnect() => Disconnect(DisconnectReason.Manual);

        public void Disconnect(DisconnectReason reason)
        {
            if(State != ClientState.Playing)
                return;

            if(server.State == ServerState.Playing)
                server.Disconnect();

            State = ClientState.Disconnected;
            client.HandleMessage(new Disconnected(reason));
        }

        /// <summary>
        /// Sends a client message to the server.
        /// </summary>
        public void HandleMessage(ClientMessage msg)
            => server.SendMessageReliable(msg);

        /// <summary>
        /// Parses or relays a server message. 
        /// </summary>
        public void ReadServerMessage(NetIncomingMessage netMsg)
        {
            var msgType = (NetMessageHeader)netMsg.ReadByte();
            switch(msgType)
            {
                case NetMessageHeader.GameFrame:
                    objects.ReadFrame(netMsg);
                    break;

                case NetMessageHeader.ProtoMessage:
                    var ioMsg = netMsg.ProtoDeserialize<ServerMessage>();
                    if(ioMsg == null)
                    {
                        NetLog.Default.Warning($"Received bad message of length {netMsg.LengthBytes:N0} bytes.");
                        break;
                    }

                    // update state if player just got accepted
                    if(State == ClientState.AwaitingHandshake && (ioMsg is HandshakeReply hsMsg))
                        parseHandshake(hsMsg);

                    // pass to client otherwise
                    client.HandleMessage(ioMsg);
                    break;

                default:
                    NetLog.Default.Warning($"Received bad message header: {(int)msgType}.");
                    break;
            }
        }


        public void Update(int msElapsed)
        {
            // sends the current player state as a game frame to server
            if(State != ClientState.Playing || !stateRefreshCounter.Tick(msElapsed))
                return;

            // serialize the player state
            using(var ms = new MemoryStream(gameFrameBuffer, true))
            {
                Serializer.Serialize(ms, client.PlayerState);
                gameFrameBufferLength = (int)ms.Position;
            }

            // construct the message
            var msg = server.NetClient.CreateMessage(sizeof(byte) + sizeof(uint) + gameFrameBufferLength);
            msg.Write((byte)NetMessageHeader.GameFrame);
            msg.Write((uint)objects.CurrentFrame);
            msg.Write(gameFrameBuffer, 0, gameFrameBufferLength);
            // send it
            server.NetClient.SendMessage(msg, NetDeliveryMethod.UnreliableSequenced);
        }

        public string GetDebugString()
        {
            return $"Latency: {server.NetClient.ServerConnection.AverageRoundtripTime:0}ms";
        }

        public void StartPlaying()
        {
            // do nothing: server side informs the engine we'll be playing right away
        }
    }
}
