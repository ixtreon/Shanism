using Shanism.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Common.Message.Client;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.StubObjects;
using Shanism.Common.Message.Network;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Network.Client
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class NClient : NPeer, IReceptor
    {
        public uint Id { get; private set; }

        public string Name { get; }


        readonly ObjectCache objects = new ObjectCache();

        readonly ClientSerializer serializer = new ClientSerializer();

        bool isConnected;

        public IHero MainHero { get; private set; }

        public IShanoClient GameClient { get; private set; }

        public IReadOnlyCollection<IEntity> VisibleEntities => objects.VisibleEntities;

        public event Action<IOMessage> MessageSent;

        NetClient NetClient => (NetClient)peer;

        public bool IsConnected => isConnected;

        static NClient()
        {
            Log.Init("client");
        }


        public NClient(string hostAddress, string name)
            : base(new NetClient(new NetPeerConfiguration(AppIdentifier)))
        {
            Name = name;

            //initiate a connection
            var conn = NetClient.Connect(hostAddress, NetworkPort);
        }

        public void SetClient(IShanoClient client)
        {
            this.GameClient = client;

            //beam events thru the network
            client.MessageSent += SendMessage;
        }


        internal override void OnConnected(NetConnection conn)
        {
            isConnected = true;

            //send a handshake init to the server
            var ioMsg = new HandshakeInitMessage(Name);
            SendMessage(ioMsg);
        }

        internal override void OnDisconnected(NetConnection conn)
        {
            isConnected = false;

            //do stuff when the link drops (whatever the reason)

            Log.Default.Info("Disconnected");
        }

        //got a message from the server
        internal override void HandleDataMessage(NetIncomingMessage msg)
        {
            //parse it as an IOMessage
            var ioMsg = msg.ToIOMessage();
            if (ioMsg == null)
            {
                Log.Default.Warning($"Received a faulty message of length {msg.LengthBytes}.");
                return;
            }

            switch (ioMsg.Type)
            {
                //a server game frame
                case MessageType.GameFrame:
                    objects.ReadServerFrame(serializer, (GameFrameMessage)ioMsg);
                    break;

                //grab the player id from a handshake reply message
                case MessageType.HandshakeReply:
                    Id = ((HandshakeReplyMessage)ioMsg).PlayerId;

                    goto case MessageType.PlayerStatusUpdate;

                //relay all other messages to the IClient
                case MessageType.PlayerStatusUpdate:
                case MessageType.MapReply:
                    Log.Default.Info($"Received a {ioMsg.Type}. ");
                    MessageSent(ioMsg);
                    break;

                default:
                    Log.Default.Warning($"Unrecognized message type: {ioMsg.Type}");
                    return;
            }
        }

        void SendMessage(IOMessage ioMsg)
        {
            SendMessage(ioMsg, NetDeliveryMethod.ReliableUnordered);
        }

        void SendMessage(IOMessage ioMsg, NetDeliveryMethod deliveryMethod)
        {
            var msg = ioMsg.ToNetMessage(NetClient);
            var result = NetClient.SendMessage(msg, deliveryMethod);

            if(result == NetSendResult.FailedNotConnected)
            {
                //TODO: initiate shutdown sequence
                Log.Default.Info($"Connection dropped. ");
                return;
            }

            Log.Default.Debug($"Sent a {ioMsg.Type}. ");
        }

        public void UpdateServer(int msElapsed)
        {
            //send client frame
            var msg = serializer.WriteClientFrame(GameClient.State);
            SendMessage(msg, NetDeliveryMethod.UnreliableSequenced);

            Update(msElapsed);
        }

        public string GetDebugString()
        {
            return string.Empty;
        }
    }
}
