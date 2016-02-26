using IO;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IO.Message.Client;
using IO.Message;
using IO.Message.Server;
using IO.Objects;
using IO.Message.Network;

namespace Network.Client
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class NClient : NPeer, IReceptor
    {
        public readonly string PlayerName;

        NetClient NetClient { get { return (NetClient)peer; } }

        bool isConnected = false;

        #region GameReceptor fields and properties
        public event Action<IUnit, string> AnyUnitAction;
        public event Action<IOMessage> MessageSent;

        #endregion

        public IHero MainHero { get; private set; }

        public IShanoClient GameClient { get; private set; }

        ObjectCache objects { get; } = new ObjectCache();

        static NClient()
        {
            Log.Init("client");
        }


        public NClient(string hostAddress, string name)
            : base(new NetClient(new NetPeerConfiguration(AppIdentifier)))
        {
            PlayerName = name;

            //initiate a connection
            var conn = NetClient.Connect(hostAddress, NetworkPort);
        }

        public void SetClient(IShanoClient client)
        {
            this.GameClient = client;

            //event handling lol
            client.ActionActivated += SendMessage;
            client.ChatMessageSent += SendMessage;
            client.MapRequested += SendMessage;
            client.MovementStateChanged += SendMessage;
            client.HandshakeInit += () => { };
        }


        internal override void OnConnected(NetConnection conn)
        {


            //send a handshake init to the server
            // no matter if the client requested it or not, lel
            var ioMsg = new HandshakeInitMessage(PlayerName);
            SendMessage(ioMsg);
        }

        //do stuff when the link drops. (forcefully or not)
        internal override void OnDisconnected(NetConnection conn)
        {
            isConnected = false;

            Log.Default.Info("Disconnected");
        }


        internal override void HandleDataMessage(NetIncomingMessage msg)
        {
            var ioMsg = msg.ToIOMessage();

            if (ioMsg == null)
            {
                Log.Default.Warning($"Received a faulty message of length {msg.LengthBytes}. ");
                return;
            }

            switch(ioMsg.Type)
            {
                //relay most messages to the IClient
                case MessageType.HandshakeReply:
                case MessageType.PlayerStatusUpdate:
                case MessageType.MapReply:
                    Log.Default.Info($"Received a {ioMsg.Type}. ");

                    MessageSent(ioMsg);
                    break;

                case MessageType.ObjectUnseen:
                    Log.Default.Info($"Received a {ioMsg.Type}. ");

                    objects.UnseeObject((ObjectUnseenMessage)ioMsg);

                    MessageSent(ioMsg);
                    break;

                case MessageType.ObjectSeen:
                    Log.Default.Info($"Received a {ioMsg.Type}. ");

                    var obj = objects.SeeObject((ObjectSeenMessage)ioMsg) as IEntity;
                    if (obj != null)
                        MessageSent(new ObjectSeenMessage(obj));

                    break;

                case MessageType.GameFrame:
                    objects.UpdateGame((GameFrameMessage)ioMsg);
                    break;

                default:
                    Log.Default.Warning($"Unrecognized message type: {ioMsg.Type}");
                    break;
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

                Log.Default.Info($"Connection was dropped. ");
                return;
            }

            Log.Default.Info($"Sent a {ioMsg.Type}. ");
        }

        public void UpdateServer(int msElapsed)
        {
            Update(msElapsed);
        }

        public string GetPerfData()
        {
            return string.Empty;
        }
    }
}
