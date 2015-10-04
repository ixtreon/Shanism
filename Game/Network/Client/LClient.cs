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
    public class LClient : LPeer, IGameReceptor
    {

        public readonly string PlayerName;

        NetClient NetClient { get { return (NetClient)peer; } }

        bool isConnected = false;

        #region GameReceptor fields and properties
        public event Action<MapChunkId, TerrainType[,]> ChunkReceived;

        public MovementState MovementState { get; set; }

        public Vector CameraPosition { get; private set; }

        public IHero MainHero { get; private set; }

        public IEnumerable<IGameObject> VisibleObjects
        {
            get
            {
                return ObjectFactory.RangeQuery(CameraPosition - Constants.Client.WindowSize / 2, Constants.Client.WindowSize);
            }
        }

        bool IGameReceptor.Connected { get { return isConnected; } }

        public bool HasHero { get { return MainHero != null; } }

        #endregion

        #region State Tracking Vars

        Vector lastCameraPosition = new Vector();
        MovementState lastMovementState = MovementState.Stand;

        #endregion

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

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            updateMovementState();
        }

        internal override void OnConnected(NetConnection conn)
        {
            //send a handshake init
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
                    handleHandshake((HandshakeReplyMessage)ioMsg);
                    break;

                case MessageType.PlayerStatusUpdate:
                    handleStatusUpdate((PlayerStatusMessage)ioMsg);
                    break;

                case MessageType.MapReply:
                    handleMapReply((MapReplyMessage)ioMsg);
                    break;

                case MessageType.ObjectSeen:
                    handleObjectSeen((ObjectSeenMessage)ioMsg);
                    break;

                default:
                    Log.Default.Warning("Unrecognized message type: {0}", ioMsg.Type.ToString());
                    break;
            }
        }

        void updateMovementState()
        {
            if (lastMovementState != MovementState)
            {
                var msg = new MoveMessage(MovementState);
                SendMessage(msg);

                lastMovementState = MovementState;
            }
        }

        void handleObjectSeen(ObjectSeenMessage ioMsg)
        {
            IGameObject obj = null;
            Serializer.GetReader(ioMsg.Data, r =>
            {
                var objType = ioMsg.ObjectType;
                if (objType == ObjectType.Hero)
                    obj = Serializer.ReadInterfaceData<IHero, HeroStub>(r, skipUnknownFields: true);

                else if (objType == ObjectType.Unit)
                    obj = Serializer.ReadInterfaceData<IUnit, UnitStub>(r, skipUnknownFields: true);

                else if (objType == ObjectType.Doodad)
                    obj = Serializer.ReadInterfaceData<IDoodad, DoodadStub>(r, skipUnknownFields: true);

                else
                    Log.Default.Warning("Unrecognized object type: {0}", objType.ToString());
            });

            ObjectFactory.AddOrUpdate(ioMsg.ObjectType, ioMsg.Guid, obj);
        }

        void handleMapReply(MapReplyMessage msg)
        {
            //invoke the ChunkReceived event
            ChunkReceived?.Invoke(msg.Chunk, msg.Data);
        }

        void handleStatusUpdate(PlayerStatusMessage m)
        {
            //update the hero instance
            if (m.HeroId >= 0)
                MainHero = (IHero)ObjectFactory.GetOrCreate(ObjectType.Hero, m.HeroId);

            Console.WriteLine("Got a hero message!");
        }

        void handleHandshake(HandshakeReplyMessage m)
        {
            Console.WriteLine("Got a handshake reply. Everything OK? {0}", m.Success);

            if(m.Success)
            {
                isConnected = true;

            }
        }

        public void RequestChunk(MapChunkId chunk)
        {
            if(!isConnected)
            {
                Console.WriteLine("No server connection!");
                return;
            }

            //ask the server for a chunk
            var ioMsg = new MapRequestMessage(chunk);
            SendMessage(ioMsg);
        }

        public void RegisterAction(ActionMessage p)
        {
            //inform the server about an action to be performed
            //NYI
        }

        void SendMessage(IOMessage ioMsg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            var msg = ioMsg.ToNetMessage(NetClient);
            NetClient.SendMessage(msg, deliveryMethod);
            Log.Default.Info("Sent a {0}", ioMsg.Type);
        }
    }
}
