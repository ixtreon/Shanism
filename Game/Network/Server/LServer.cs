using IO;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using IO.Objects;
using IxLog;
using Lidgren.Network;
using Network.Objects.Serializers;
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
    public class LServer : LPeer
    {
        static LServer()
        {
            Log.Init("server");
            SerializerModules.Init();
        }


        public event Action<NetGameClient> ClientConnected;

        public event Action<NetGameClient> ClientDisconnected;


        readonly IEngine engine;

        internal NetServer Server {  get { return (NetServer)peer; } }

        readonly Dictionary<NetConnection, NetGameClient> clients = new Dictionary<NetConnection, NetGameClient>();



        public LServer(IEngine engine)
            : base(new NetServer(new NetPeerConfiguration(AppIdentifier) { Port = NetworkPort }))
        {
            this.engine = engine;
            //engine.AnyUnitOrderChanged += Engine_AnyUnitOrderChanged;

            Log.Default.Info("Server started!");
        }


        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            //update netclients
            foreach (var client in clients.Values)
                client.Update(msElapsed);
        }


        internal override void OnConnected(NetConnection conn)
        {
            // do nothing, wait for a handshake init
        }

        internal override void OnDisconnected(NetConnection conn)
        {
            //see if this was a tracked connection
            NetGameClient client;
            if (!clients.TryGetValue(conn, out client))
                return;

            //TODO: do some proper disconnect?
            ClientDisconnected?.Invoke(client);
        }


        internal override void HandleDataMessage(NetIncomingMessage incomingMessage)
        {
            //try
            {
                var msg = incomingMessage.ToIOMessage();


                //check if it's a handshake and if so, ask the server whether to accept it
                if (msg.Type == MessageType.HandshakeInit)
                {
                    handleHandshake(incomingMessage.SenderConnection, (HandshakeInitMessage)msg);
                    return;
                }

                //for all other message types, the client must already have an identity. 
                var client = clients.TryGet(incomingMessage.SenderConnection);
                if (client == null)
                {
                    Log.Default.Warning("Received a request from an unknown client. Ignoring it. ");
                    return;
                }

                client.HandleMessage(msg);
            }
            //catch (Exception e)
            //{
            //    Log.Default.Error("Unhandled exception while handling a data packet: {0}", e.Message);
            //}
        }

        void Engine_AnyUnitOrderChanged(IEnumerable<IPlayer> seenByPlayers, IO.Common.OrderType orderType)
        {

            //send a message to each of them
        }

        /// <summary>
        /// Parses an incoming <see cref="HandshakeInitMessage"></see>
        /// by handing it over to the server. 
        /// 
        /// If the server successfully accepts the user it returns a <see cref="IGameReceptor"/> object
        /// which is added to the server's list of connected peers. Otherwise the connection is dropped.  
        /// </summary>
        /// <param name="peerConnection"></param>
        /// <param name="msg"></param>
        void handleHandshake(NetConnection peerConnection, HandshakeInitMessage msg)
        {
            //get the peer data
            var client = new NetGameClient(Server, peerConnection, msg.PlayerName);


            //check if the server accepts it
            var receptor = engine.AcceptClient(client);
            var accepted = (receptor != null);  //TODO: make an actual check

            //if so, add to our list, too
            if (accepted)
            {
                // do our set-up
                client.Initialize(receptor);
                clients.Add(peerConnection, client);

                // inform the engine we'll be playing
                // (this step is necessary for single-player, check out LocalShano implementation)
                engine.StartPlaying(receptor);

                //and raise the client event
                ClientConnected?.Invoke(client);
            }

            Log.Default.Info("Got a handshake from {0}! Do we accept it? {1}", peerConnection.RemoteEndPoint.Address, accepted ? "yep" : "nope");

            //prepare and send the handshake reply
            receptor.SendHandshake(accepted);

            if (!accepted)
            {
                //TODO: drop connection
            }
        }


        NetGameClient fromPlayer(IPlayer pl)
        {
            return clients.Values.FirstOrDefault(cl => cl.GameReceptor == pl);
        }
    }
}
