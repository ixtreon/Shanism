using IO;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IO.Common;
using IO.Message.Client;
using IO.Message;
using IO.Message.Server;
using Network.Objects;
using System.IO;
using ProtoBuf;
using IO.Objects;
using IxLog;

namespace Network
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class LClient : LPeer, IReceptor
    {
        public readonly string PlayerName;

        NetClient NetClient { get { return (NetClient)peer; } }

        bool isConnected = false;

        #region GameReceptor fields and properties
        public event Action<IUnit, string> AnyUnitAction;
        public event Action<IOMessage> MessageSent;

        #endregion

        public IHero MainHero { get; private set; }

        public IClient GameClient { get; private set; }


        static LClient()
        {
            Log.Init("client");
        }


        public LClient(string hostAddress, string name)
            : base(new NetClient(new NetPeerConfiguration(AppIdentifier)))
        {
            PlayerName = name;

            //initiate a connection
            var conn = NetClient.Connect(hostAddress, NetworkPort);
        }

        public void SetClient(IClient client)
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
            //send a handshake init no matter if the client requested it or not, lel
            HandshakeInitMessage ioMsg = new HandshakeInitMessage(PlayerName);
            SendMessage(ioMsg);
        }

        //do stuff when the link drops. (forcefully or not)
        internal override void OnDisconnected(NetConnection conn)
        {
            isConnected = false;

            Console.WriteLine("We got d/c :(");
        }


        internal override void HandleDataMessage(NetIncomingMessage msg)
        {
            var ioMsg = msg.ToIOMessage();

            switch(ioMsg.Type)
            {
                //relay most messages to the IClient
                case MessageType.HandshakeReply:
                case MessageType.PlayerStatusUpdate:
                case MessageType.MapReply:
                case MessageType.ObjectSeen:
                case MessageType.ObjectUnseen:
                    MessageSent(ioMsg);
                    break;

                case MessageType.ObjectData:
                    //TODO: decode and do 'objectseen'
                    break;

                default:
                    Log.Default.Warning("Unrecognized message type: {0}", ioMsg.Type.ToString());
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
            NetClient.SendMessage(msg, deliveryMethod);
            Log.Default.Info("Sent a {0}", ioMsg.Type);
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
