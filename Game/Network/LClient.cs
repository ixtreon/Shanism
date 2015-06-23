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

namespace Network
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class LClient : IGameReceptor
    {
        //TODO: these are a duplicate of LServer constants
        const string AppIdentifier = "ShanoRpg";
        const int NetworkPort = 6969;
        

        public readonly string PlayerName;



        NetPeerConfiguration netConfig;
        NetClient netClient;

        #region GameReceptor fields and properties
        public event Action<MapChunkId, TerrainType[,]> ChunkReceived;

        public event Action<IHero> MainHeroChanged;

        public bool Connected { get; private set; }

        public bool HasHero { get { return MainHero != null; } }

        public Vector CameraPosition { get; private set; }

        public IHero MainHero { get; private set; }

        public MovementState MovementState { get; set; }

        #endregion

        #region State Tracking

        private Vector lastCameraPosition = new Vector();
        private MovementState lastMovementState = MovementState.Stand;

        #endregion

        public LClient(string hostAddress, string name)
        {
            PlayerName = name;
            netConfig = new NetPeerConfiguration(AppIdentifier);

            netClient = new NetClient(netConfig);
            netClient.Start();

            netClient.RegisterReceivedCallback(new SendOrPostCallback(onMessageReceived));
            var conn = netClient.Connect(hostAddress, NetworkPort);
        }


        public void Update(int msElapsed)
        {
            updateMovementState();
        }


        private void updateMovementState()
        {
            if(lastMovementState != MovementState)
            {
                var msg = new MoveMessage(MovementState);
                SendMessage(msg);

                lastMovementState = MovementState;
            }
        }

        private void SendMessage(IOMessage ioMsg, NetDeliveryMethod deliveryMethod = NetDeliveryMethod.ReliableUnordered)
        {
            var msg = ioMsg.ToNetMessage(netClient);
            netClient.SendMessage(msg, deliveryMethod);
            Log.Info("Sent a {0}", ioMsg.Type);
        }

        private void onMessageReceived(object peer)
        {
            NetIncomingMessage msg;
            while ((msg = netClient.ReadMessage()) != null)
            {
                handleMessage(msg);
                netClient.Recycle(msg);
            }
        }

        private void handleMessage(NetIncomingMessage msg)
        {
            // handle incoming message
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                    string text = msg.ReadString();
                    Console.WriteLine("[DEBUG/ERR/WARN] {0}", text);
                    break;

                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                    if (status == NetConnectionStatus.Connected)
                        OnConnected();
                    else
                        OnDisconnected();

                    string reason = msg.ReadString();
                    Console.WriteLine(status.ToString() + ": " + reason);

                    break;


                case NetIncomingMessageType.Data:
                    handleDataMessage(msg);
                    break;

                default:
                    Console.WriteLine("Unhandled message type: {0} ({1} bytes)", msg.MessageType, msg.LengthBytes);
                    break;
            }
        }

        private void OnConnected()
        {
            //send handshake init
            HandshakeInitMessage ioMsg = new HandshakeInitMessage(PlayerName);
            SendMessage(ioMsg);
        }

        private void OnDisconnected()
        {
            //do shit when the link drops. (forcefully or not)
            Connected = false;

            Console.WriteLine("We got d/c :(");
        }


        private void handleDataMessage(NetIncomingMessage netMsg)
        {
            var ioMsg = netMsg.ToIOMessage();

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

                default:
                    throw new Exception("Unrecognized message type!");
            }
        }

        private void handleMapReply(MapReplyMessage msg)
        {
            ChunkReceived?.Invoke(msg.Chunk, msg.GetData());
        }

        private void handleStatusUpdate(PlayerStatusMessage m)
        {
            if (m.HeroId >= 0)
                MainHero = (IHero)ObjectFactory.GetOrCreate(ObjectType.Hero, m.HeroId);

            Console.WriteLine("Got a hero message!");
        }

        private void handleHandshake(HandshakeReplyMessage m)
        {
            Console.WriteLine("Got a handshake reply!: {0}", m.Success);
            Connected = true;
        }

        public IEnumerable<IGameObject> GetNearbyGameObjects()
        {
            //NYI
            return Enumerable.Empty<IGameObject>();
        }

        public void RequestChunk(MapChunkId chunk)
        {
            if(!Connected)
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
    }
}
