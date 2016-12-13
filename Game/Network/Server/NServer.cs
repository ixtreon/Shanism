using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Server;
using Shanism.Common.StubObjects;
using Shanism.Common.Serialization;
using IxLog;
using Lidgren.Network;
using Shanism.Network.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network
{
    /// <summary>
    /// Lidgren.Network server
    /// </summary>
    public class NServer : NPeer
    {
        static NServer()
        {
            Log.Init("server");
        }


        readonly IShanoEngine engine;

        readonly Dictionary<NetConnection, NServerClient> clients = new Dictionary<NetConnection, NServerClient>();


        internal NetServer NetServer { get { return (NetServer)peer; } }


        public NServer(IShanoEngine engine)
            : base(new NetServer(new NetPeerConfiguration(AppIdentifier) { Port = NetworkPort }))
        {
            this.engine = engine;
            Log.Default.Info("Server started!");
        }


        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            //update all netclients
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
            NServerClient client;
            if (!clients.TryGetValue(conn, out client))
                return;

            //TODO: do some proper disconnect?
            clients.Remove(conn);
        }


        internal override void ReadMessage(NetIncomingMessage msg)
        {
            var isProtoMessage = msg.ReadByte() == 0;

            if(isProtoMessage)
            {
                var ioMsg = msg.ToIOMessage();

                if(ioMsg == null)
                {
                    Log.Default.Warning($"Unable to read incoming message of length {msg.LengthBytes}. ");
                    return;
                }

                //check if it's a handshake and if so, ask the server whether to accept it
                if(ioMsg.Type == MessageType.HandshakeInit)
                {
                    handleHandshake(msg.SenderConnection, (HandshakeInitMessage)ioMsg);
                    return;
                }

                //for all other message types, the client must already have an identity. 
                var client = clients.TryGet(msg.SenderConnection);
                if(client == null)
                {
                    Log.Default.Warning("Received a request from an unknown client. Ignoring it. ");
                    return;
                }

                client.handleProtoMessage(ioMsg);
            }
            else
            {
                //received a game frame..
            }
        }


        /// <summary>
        /// Parses an incoming <see cref="HandshakeInitMessage"></see> by handing it over to the server. 
        /// <para/>
        /// If the server successfully accepts the user it returns a <see cref="IReceptor"/> object
        /// which is added to the server's list of connected peers. Otherwise the connection is dropped.  
        /// </summary>
        void handleHandshake(NetConnection peerConnection, HandshakeInitMessage msg)
        {
            //get the peer data
            var client = new NServerClient(NetServer, peerConnection, msg.PlayerName);


            //check if the server accepts it
            var receptor = engine.AcceptClient(client);
            var accepted = (receptor != null);  //TODO: make an actual check
            Log.Default.Info($"Player {msg.PlayerName} ({peerConnection.RemoteEndPoint.Address}) wants to join. Accepted? {accepted}");

            //if so, add to our list, too
            if (accepted)
            {
                // do netgameclient / clients set-up
                client.Initialize(receptor);
                clients.Add(peerConnection, client);

                // inform the engine we'll be playing
                // (this step is necessary for single-player, check out LocalShano implementation)
                engine.StartPlaying(receptor);
            }

            if (!accepted)
            {
                //TODO: drop connection
            }
        }


    }
}
