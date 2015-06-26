using IO;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using IO.Objects;
using Lidgren.Network;
using Network.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    /// <summary>
    /// Lidgren.Network server
    /// </summary>
    public class LServer
    {
        const string AppIdentifier = "ShanoRpg";
        const int NetworkPort = 6969;

        /// <summary>
        /// A method that accepts a game client pending connection, and eventually returns a receptor instance for this guy. 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public delegate IGameReceptor ClientGeneratorCallback(IGameClient client);


        public event Action<NetGameClient> ClientConnected;

        public event Action<NetGameClient> ClientDisconnected;


        public ClientGeneratorCallback ClientConnectHandler { get; set; }

        readonly IGameEngine engine;

        readonly NetPeerConfiguration netConfig;
        readonly NetServer server;

        readonly Dictionary<NetConnection, NetGameClient> clients;

        public LServer(IGameEngine engine)
        {
            clients = new Dictionary<NetConnection, NetGameClient>();

            netConfig = new NetPeerConfiguration(AppIdentifier);
            netConfig.Port = NetworkPort;

            server = new NetServer(netConfig);
            server.Start();
            Log.Info("Server started!");
        }


        // starts listening to events from the server
        private void hookToEngine()
        {
            engine.AnyUnitEntersVisionRange += Engine_AnyUnitEntersVisionRange;
            engine.AnyUnitOrderChanged += Engine_AnyUnitOrderChanged;
        }

        private void Engine_AnyUnitOrderChanged(IUnit u, IO.Common.OrderType ord)
        {
            throw new NotImplementedException();
        }

        private void Engine_AnyUnitEntersVisionRange(IUnit origin, IUnit trigger)
        {
            throw new NotImplementedException();
        }

        public void Update(int msElapsed)
        {
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                handleMessage(msg);
                server.Recycle(msg);
            }

            //update clients
            foreach (var gc in clients.Values)
                gc.Update(msElapsed);
        }


        private void handleMessage(NetIncomingMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                    Log.Debug(msg.ReadString());
                    break;
                case NetIncomingMessageType.WarningMessage:
                    Log.Warning(msg.ReadString());
                    break;
                case NetIncomingMessageType.ErrorMessage:
                    Log.Error(msg.ReadString());
                    break;

                // data messages
                case NetIncomingMessageType.Data:
                    handleDataMessage(msg);
                    break;

                // client connect / disconnect
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();

                    if (status == NetConnectionStatus.Connected)
                    {
                        /// just connected
                        // do nothing, wait for a handshake init
                    }
                    else
                    {
                        /// just disconnected

                        //see if this was a tracked connection
                        var conn = msg.SenderConnection;
                        NetGameClient client;
                        if (!clients.TryGetValue(conn, out client))
                            return;

                        //TODO: do proper disconnect?
                        ClientDisconnected?.Invoke(client);
                    }

                    string reason = msg.ReadString();
                    Log.Info(status.ToString() + ": " + reason);
                    break;

                default:
                    Log.Error("Unhandled message type: {0} ({1} bytes)", msg.MessageType, msg.LengthBytes);
                    break;
            }
        }


        private void handleDataMessage(NetIncomingMessage netMsg)
        {
            Debug.Assert(netMsg.MessageType == NetIncomingMessageType.Data);


            //first check if it's a handshake and if so, ask the server whether to accept it
            var msg = netMsg.ToIOMessage();

            if(msg.Type == MessageType.HandshakeInit)
            {
                handleHandshake(netMsg.SenderConnection, (HandshakeInitMessage)msg);
                return;
            }

            //for all other message types, the client must already have an identity. 
            var client = clients.TryGet(netMsg.SenderConnection);
            if (client == null)
            {
                Log.Warning("Received map request from a non-client. ");
                return;
            }

            switch (msg.Type)
            {
                case MessageType.MapRequest:

                    client.HandleMapRequest((MapRequestMessage)msg);
                    break;

                case MessageType.MoveUpdate:
                    client.HandleMoveUpdate((MoveMessage)msg);
                    break;


            }
        }

        /// <summary>
        /// Parses an incoming <see cref="HandshakeInitMessage"></see>
        /// by handing it over to the server, using <see cref="ClientConnectHandler"/>. 
        /// 
        /// If the server successfully accepts the user it returns a <see cref="IGameReceptor"/> object
        /// which is added to the server's list of connected peers. Otherwise the connection is dropped.  
        /// </summary>
        /// <param name="peerConnection"></param>
        /// <param name="msg"></param>
        private void handleHandshake(NetConnection peerConnection, HandshakeInitMessage msg)
        {
            //get the peer data
            var client = new NetGameClient(server, peerConnection, msg.PlayerName);

            if (ClientConnectHandler == null)
            {
                Log.Warning("A client tried to join but no server was listening for it!");
                return;
            }

            //check if the server accepts it
            var receptor = ClientConnectHandler(client);
            var accepted = (receptor != null);  //TODO: make an actual check

            //if so, add to our list, too
            if (accepted)
            {
                client.Initialize(receptor);
                clients.Add(peerConnection, client);

                //and raise the event
                ClientConnected?.Invoke(client);
            }

            Log.Info("Got a handshake from {0}! Do we accept it? {1}", peerConnection.RemoteEndPoint.Address, accepted ? "yep" : "nope");


            //prepare and send the handshake reply
            var ioMsg = new HandshakeReplyMessage(accepted);
            client.SendMessage(ioMsg);

            //TODO: drop connection
            if (!accepted)
            {
            }

            //proceed by sending a player status message. 
            if(accepted)
            {
                client.SendPlayerStatusUpdate();
            }
        }

    }
}
