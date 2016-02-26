using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public abstract class NPeer
    {
        public const string AppIdentifier = "ShanoRpg";
        public const int NetworkPort = 6969;

        internal NetPeer peer;

        protected NPeer(NetPeer peer)
        {
            this.peer = peer;

            //start the client
            peer.Start();
        }

        /// <summary>
        /// Reads incoming messages. To be called continuously. 
        /// </summary>
        public virtual void Update(int msElapsed)
        {
            NetIncomingMessage msg;
            while ((msg = peer.ReadMessage()) != null)
            {
                HandleIncomingMessage(msg);
                peer.Recycle(msg);
            }
        }

        internal void HandleIncomingMessage(NetIncomingMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                    Log.Default.Debug(msg.ReadString());
                    break;
                case NetIncomingMessageType.WarningMessage:
                    Log.Default.Warning(msg.ReadString());
                    break;
                case NetIncomingMessageType.ErrorMessage:
                    Log.Default.Error(msg.ReadString());
                    break;

                // data messages
                case NetIncomingMessageType.Data:
                    HandleDataMessage(msg);
                    break;

                // client connect / disconnect
                case NetIncomingMessageType.StatusChanged:
                    var status = (NetConnectionStatus)msg.ReadByte();

                    if (status == NetConnectionStatus.Connected)
                        OnConnected(msg.SenderConnection);
                    else if (status == NetConnectionStatus.Disconnected)
                        OnDisconnected(msg.SenderConnection);

                    string reason = msg.ReadString();
                    Log.Default.Info(status.ToString() + ": " + reason);
                    break;

                default:
                    Log.Default.Warning("Unhandled message type: {0} ({1} bytes)", msg.MessageType, msg.LengthBytes);
                    break;
            }
        }

        internal abstract void HandleDataMessage(NetIncomingMessage msg);

        internal virtual void OnConnected(NetConnection conn) { }
        internal virtual void OnDisconnected(NetConnection conn) { }
    }
}
