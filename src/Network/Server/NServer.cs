using Shanism.Common;
using Shanism.Common.Messages;
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
            NetLog.Init("server");
        }


        readonly IEngine engine;

        readonly Dictionary<NetConnection, NetworkClient> clients = new Dictionary<NetConnection, NetworkClient>();


        internal NetServer NetServer => (NetServer)peer;

        uint CurrentFrame;

        public NServer(IEngine engine)
            : base(new NetServer(new NetPeerConfiguration(AppIdentifier) { Port = DefaultPort }))
        {
            this.engine = engine;
            NetLog.Default.Info("Server started!");
        }


        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            CurrentFrame++;

            //update all netclients
            foreach (var client in clients.Values)
                client.Update(msElapsed, CurrentFrame);
        }

        internal override void OnConnected(NetConnection conn)
        {
            // do nothing, wait for a handshake init
        }

        internal override void OnDisconnected(NetConnection conn)
        {
            if(clients.TryGetValue(conn, out var client))
            {
                clients.Remove(conn);
                engine.Disconnect(client.Name);

                Console.WriteLine($"Client {conn} has disconnected!");
            }
        }


        internal override void ReadMessage(NetIncomingMessage msg)
        {
            // if client is registered, just pass to the assigned receptor.
            if (clients.TryGetValue(msg.SenderConnection, out var client))
            {
                client.ReadServerMessage(msg);
                return;
            }

            // otherwise only let through protobuf msgs...
            var msgType = (NetMessageHeader)msg.ReadByte();
            if(msgType != NetMessageHeader.ProtoMessage)
            {
                NetLog.Default.Warning($"Bad message header from {msg.SenderEndPoint.Address} (unknown).");
                return;
            }

            // ...that are handshakes
            var ioMsg = msg.ProtoDeserialize<ClientMessage>();
            if(ioMsg == null || ioMsg.Type != ClientMessageType.HandshakeInit)
            {
                NetLog.Default.Warning($"Bad message from {msg.SenderEndPoint.Address} (unknown).");
                return;
            }


            // create a client object and see if server accepts it
            var request = (HandshakeInit)ioMsg;
            var peerConnection = msg.SenderConnection;
            client = new NetworkClient(NetServer, peerConnection, request.PlayerName);

            //if so, add to our list, too
            var receptor = engine.Connect(client);
            if (receptor != null)
            {
                // inform the client & add to the connected list
                client.Initialize(receptor);
                clients.Add(peerConnection, client);

                // inform the engine we'll be playing (necessary for single-player)
                receptor.StartPlaying();

                NetLog.Default.Info($"Player {request.PlayerName} ({peerConnection.RemoteEndPoint.Address}) joined the game.");
            }
            else
            {
                //TODO: drop connection
                NetLog.Default.Info($"Player {request.PlayerName} ({peerConnection.RemoteEndPoint.Address}) was rejected.");
            }
        }


    }
}
