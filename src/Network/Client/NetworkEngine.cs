using Shanism.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Common.Messages;

namespace Shanism.Network.Client
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Emulates a local game engine to fool the game client. 
    /// </summary>
    public class NetworkEngine : NPeer, IEngine
    {

        //bool isConnected;
        //bool handshakeSent;
        NReceptor receptor;

        public string HostAddress { get; }

        /// <summary>
        /// Gets the state of the underlying server. 
        /// State is always one of Stopped, Playing, or LoadFailure.
        /// </summary>
        public ServerState State { get; private set; }

        /// <summary>
        /// Gets the state of the single client (receptor) represented by this instance.
        /// </summary>
        internal ClientState ClientState { get; private set; }

        public IClientReceptor GameClient { get; private set; }

        public IEnumerable<IPlayer> Players { get; }

        public NetClient NetClient => (NetClient)peer;

        public bool IsConnected => ClientState >= ClientState.Connected;

        public bool IsLocal => false;

        static NetworkEngine()
        {
            NetLog.Init("client");
        }


        public NetworkEngine(string hostAddress)
            : base(new NetClient(new NetPeerConfiguration(AppIdentifier)))
        {
            State = ServerState.Stopped;
            HostAddress = hostAddress;
        }


        public void Disconnect()
        {
            NetClient.Disconnect("!@#$ this game, srsly");
            OnDisconnected(null);
        }

        internal override void OnConnected(NetConnection conn)
        {
            NetLog.Default.Info("Connected");

            State = ServerState.Playing;
            receptor.TrySendHandshake();
        }

        internal override void OnDisconnected(NetConnection conn)
        {
            NetLog.Default.Info("Disconnected");

            State = ServerState.Stopped;
            receptor.Disconnect(DisconnectReason.TimeOut);
        }

        //got a message from the server
        internal override void ReadMessage(NetIncomingMessage msg)
        {
            receptor.ReadServerMessage(msg);
        }

        /// <summary>
        /// Sends a reliable message to the server.
        /// </summary>
        internal void SendMessageReliable(ClientMessage ioMsg)
        {
            if(State != ServerState.Playing)
                throw new InvalidOperationException($"Can't send messages unless playing.");

            NetClient.SendMessage(ioMsg.ToNetMessage(NetClient), NetDeliveryMethod.ReliableUnordered);
        }

        public IEngineReceptor Connect(IClientReceptor client)
        {
            if(receptor != null)
                receptor.Disconnect(DisconnectReason.Manual);

            GameClient = client;
            receptor = new NReceptor(this, client);

            Task.Run((Action)tryConnectAsync);

            return receptor;
        }

        void tryConnectAsync()
        {
            try
            {
                NetClient.Connect(HostAddress, DefaultPort);
            }
            catch
            {
                State = ServerState.LoadFailure;
            }
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);     // reads incoming messages

            receptor.Update(msElapsed);
        }

        public void OpenToNetwork()
        {
            NetLog.Default.Info("Unable to open a remote server to the network.");
        }

        public bool TryRestartScenario(out string errors)
        {
            errors = "Unable to restart a scenario over the network.";
            
            NetLog.Default.Info(errors);
            return false;
        }

        public bool Disconnect(string name)
        {
            NetLog.Default.Info("Unable to disconnect a player over the network.");
            return false;
        }
    }
}
