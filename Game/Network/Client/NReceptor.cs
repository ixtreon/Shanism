using Shanism.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.Interfaces.Entities;
using System.IO;

namespace Shanism.Network.Client
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Represents the IReceptor assigned to the <see cref="NClient.GameClient"/>. 
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class NReceptor : IReceptor
    {
        const int StateRefreshFps = 60;


        readonly Counter stateRefreshCounter = new Counter(1000 / StateRefreshFps);

        readonly NClient server;

        readonly ObjectCache objects = new ObjectCache();


        public uint Id { get; private set; }

        public string Name { get; }

        public IHero MainHero { get; private set; }

        public IShanoClient GameClient { get; private set; }

        public IReadOnlyCollection<IEntity> VisibleEntities => objects.VisibleEntities;

        /// <summary>
        /// Gets the last ack'd frame.
        /// </summary>
        public uint CurrentFrame => objects.CurrentFrame;


        /// <summary>
        /// Raised whenever a non-network message is received from the remote server.
        /// </summary>
        public event Action<IOMessage> MessageSent;


        public NReceptor(NClient server, IShanoClient client)
        {
            this.server = server;
            server.ConnectionLost += onConnectionLost;

            GameClient = client;
            Name = client.Name;
        }

        void onConnectionLost()
        {
            MessageSent?.Invoke(new DisconnectedMessage(DisconnectReason.TimeOut));
        }

        public void HandleMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                //a server game frame
                case MessageType.GameFrame:
                    throw new InvalidOperationException();

                //grab the player id from a handshake reply message
                case MessageType.HandshakeReply:
                    Id = ((HandshakeReplyMessage)msg).PlayerId;
                    MessageSent(msg);
                    break;

                //relay all other messages to the IClient
                default:
                    MessageSent(msg);
                    break;
            }
            Log.Default.Info($"Received a {msg.Type}. ");
        }

        public void HandleGameFrame(NetIncomingMessage msg)
        {
            objects.ReadFrame(msg);
        }

        public void Update(int msElapsed)
        {
            //send current state to server
            if (stateRefreshCounter.Tick(msElapsed))
            {
                NetOutgoingMessage msg;

                using (var ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, GameClient.State);

                    msg = server.NetClient.CreateMessage(4 + (int)ms.Length);
                    msg.Write(CurrentFrame);
                    msg.Write(ms.ToArray());
                }

                server.NetClient.SendMessage(msg, NetDeliveryMethod.UnreliableSequenced);
            }
        }

        public string GetDebugString()
        {
            return $"Latency: {server.NetClient.ServerConnection.AverageRoundtripTime:0}ms";
        }
    }
}
