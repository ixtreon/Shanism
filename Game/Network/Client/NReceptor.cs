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
using Shanism.Common.Message.Network;
using Shanism.Common.Interfaces.Entities;
using Shanism.Network.Common;

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


        public uint Id { get; private set; }

        public string Name { get; }

        readonly NClient server;

        readonly ObjectCache objects = new ObjectCache();

        readonly ClientFrameBuilder frameWriter = new ClientFrameBuilder();
        readonly ServerFrameBuilder frameReader = new ServerFrameBuilder();

        public IHero MainHero { get; private set; }

        public IShanoClient GameClient { get; private set; }

        public IReadOnlyCollection<IEntity> VisibleEntities => objects.VisibleEntities;

        /// <summary>
        /// Gets the last ack'd frame.
        /// </summary>
        public uint CurrentFrame { get; set; }


        /// <summary>
        /// Raised whenever a non-network message is received from the remote server.
        /// </summary>
        public event Action<IOMessage> MessageSent;


        public NReceptor(NClient server, IShanoClient client)
        {
            GameClient = client;
            Name = client.Name;
            this.server = server;
        }


        public void HandleMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                //a server game frame
                case MessageType.GameFrame:
                    var frameMsg = (GameFrameMessage)msg;
                    objects.ReadServerFrame(frameReader, frameMsg);

                    break;

                //grab the player id from a handshake reply message
                case MessageType.HandshakeReply:
                    Id = ((HandshakeReplyMessage)msg).PlayerId;
                    Log.Default.Info($"Received a {msg.Type}. ");
                    MessageSent(msg);
                    return;

                //relay all other messages to the IClient
                default:
                    Log.Default.Info($"Received a {msg.Type}. ");
                    MessageSent(msg);
                    return;
            }
        }

        public void Update(int msElapsed)
        {
            //send state to server
            if (stateRefreshCounter.Tick(msElapsed))
            {
                var msg = frameWriter.Write(CurrentFrame, GameClient.State);
                server.SendMessage(msg, NetDeliveryMethod.UnreliableSequenced);
            }
        }

        public string GetDebugString()
        {
            return string.Empty;
        }
    }
}
