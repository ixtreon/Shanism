using Shanism.Common;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Common.Message.Client;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.Message.Network;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Network.Client
{
    /// <summary>
    /// The code for a network client using Lidgren.Network. 
    /// 
    /// Emulates a local game server to fool the game client. 
    /// </summary>
    public class NClient : NPeer, IShanoEngine
    {
        //
        bool isConnected;
        bool handshakeSent;
        NReceptor receptor;

        public IShanoClient GameClient { get; private set; }


        NetClient NetClient => (NetClient)peer;

        public bool IsConnected => isConnected;
        bool hasClient => GameClient != null;

        static NClient()
        {
            Log.Init("client");
        }


        public NClient(string hostAddress)
            : base(new NetClient(new NetPeerConfiguration(AppIdentifier)))
        {
            //initiate a connection
            var conn = NetClient.Connect(hostAddress, NetworkPort);
        }



        internal override void OnConnected(NetConnection conn)
        {
            isConnected = true;
            trySendHandshake();
        }

        internal override void OnDisconnected(NetConnection conn)
        {
            isConnected = false;

            Log.Default.Info("Disconnected");
            //TODO
        }

        //got a message from the server
        internal override void HandleDataMessage(NetIncomingMessage msg)
        {
            //parse it as an IOMessage
            var ioMsg = msg.ToIOMessage();
            if (ioMsg == null)
            {
                Log.Default.Warning($"Received a faulty message of length {msg.LengthBytes}.");
                return;
            }

            //if valid, pass it to the receptor
            receptor.HandleMessage(ioMsg);
        }

        internal void SendMessageReliable(IOMessage ioMsg)
        {
            SendMessage(ioMsg, NetDeliveryMethod.ReliableUnordered);
        }

        internal void SendMessage(IOMessage ioMsg,
            NetDeliveryMethod deliveryMethod)
        {
            if (!handshakeSent)
            {
                Log.Default.Debug("Unable to send a message to the server: no handshake was sent.");
                return;
            }

            var msg = ioMsg.ToNetMessage(NetClient);
            var result = NetClient.SendMessage(msg, deliveryMethod);

            if (result == NetSendResult.FailedNotConnected)
            {
                //TODO: initiate shutdown sequence
                Log.Default.Info($"Connection dropped. ");
                return;
            }

            Log.Default.Debug($"Sent a {ioMsg.Type}. ");
        }


        public override void Update(int msElapsed)
        {
            //read incoming messages 
            base.Update(msElapsed);

            receptor.Update(msElapsed);
        }

        //send a handshake init if both the server and client are set
        bool trySendHandshake()
        {
            if (!isConnected || !hasClient || handshakeSent)
                return false;

            handshakeSent = true;
            var ioMsg = new HandshakeInitMessage(receptor.Name);
            SendMessageReliable(ioMsg);
            return true;
        }

        #region Server implementation
        public IReceptor AcceptClient(IShanoClient client)
        {
            if (receptor != null)
                throw new InvalidOperationException("There is already a client assigned to this instance.");

            GameClient = client;
            GameClient.MessageSent += SendMessageReliable;
            receptor = new NReceptor(this, client);
            return receptor;
        }

        public void StartPlaying(IReceptor rec)
        {
            //send a handshake if we are already connected
            //otherwise do so when we connect
            trySendHandshake();
        }

        public void OpenToNetwork()
        {
            Log.Default.Info("Unable to open a remote server to the network.");
        }

        public void RestartScenario()
        {
            Log.Default.Info("Unable to restart a scenario over the network.");
        }
        #endregion
    }
}
