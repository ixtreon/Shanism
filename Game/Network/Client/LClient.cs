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
using Network.Objects.Serializers;
using System.IO;
using IxSerializer;
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
        public event Action<HandshakeReplyMessage> HandshakeReplied;
        public event Action<PlayerStatusMessage> MainHeroChanged;
        public event Action<MapReplyMessage> MapChunkReceived;
        public event Action<IGameObject> ObjectUnseen;
        public event Action<IGameObject> ObjectSeen;
        public event Action<IUnit, string> AnyUnitAction;

        #endregion

        public IHero MainHero { get; private set; }

        public IClient GameClient { get; private set; }


        static LClient()
        {
            Log.Default.Name = "client";

            SerializerModules.Init();
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

        //do shit when the link drops. (forcefully or not)
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
                case MessageType.HandshakeReply:
                    HandshakeReplied((HandshakeReplyMessage)ioMsg);
                    break;

                case MessageType.PlayerStatusUpdate:
                    MainHeroChanged((PlayerStatusMessage)ioMsg);
                    break;

                case MessageType.MapReply:
                    MapChunkReceived((MapReplyMessage)ioMsg);
                    break;

                case MessageType.ObjectSeen:
                    handleObjectSeen((ObjectSeenMessage)ioMsg);
                    break;

                default:
                    Log.Default.Warning("Unrecognized message type: {0}", ioMsg.Type.ToString());
                    break;
            }
        }

        void handleObjectSeen(ObjectSeenMessage ioMsg)
        {
            IGameObject obj = null;
            Serializer.GetReader(ioMsg.Data, r =>
            {
                var objType = ioMsg.ObjectType;
                if (objType == ObjectType.Hero)
                    obj = InterfaceSerializer.ReadInterfaceData<IHero, HeroStub>(r, skipUnknownFields: true);

                else if (objType == ObjectType.Unit)
                    obj = InterfaceSerializer.ReadInterfaceData<IUnit, UnitStub>(r, skipUnknownFields: true);

                else if (objType == ObjectType.Doodad)
                    obj = InterfaceSerializer.ReadInterfaceData<IDoodad, DoodadStub>(r, skipUnknownFields: true);

                else
                    Log.Default.Warning("Unrecognized object type: {0}", objType.ToString());
            });

            ObjectSeen(obj);
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
    }
}
