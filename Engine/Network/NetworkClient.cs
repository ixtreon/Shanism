using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Commands;
using IO.Common;
using IO.Network;
using ProtoBuf;

namespace Engine.Network
{
    /// <summary>
    /// NetworkManagers create these when a player connects so they can be added to an engine. 
    /// Objects of this type read data as it is received from the remote client and 
    /// raise the appropriate events for the engine to consume. 
    /// </summary>
    public class NetworkClient : IClient, IDisposable
    {
        const int MAX_MESSAGE_SIZE = 1024;
        const int HeaderSize = 2;

        private readonly IPAddress Address;

        public readonly Socket TcpSocket;
        public readonly MessageSocket TcpStream;

        public NetworkClient(Socket sock)
        {
            this.TcpSocket = sock;
            this.TcpStream = new MessageSocket(sock, MAX_MESSAGE_SIZE);

            Address = (sock.RemoteEndPoint as IPEndPoint).Address;
            Console.WriteLine("'{0}' connected!", Address.ToString());

            TcpStream.MessageReceived += TcpStream_MessageReceived;
        }

        private void TcpStream_MessageReceived(byte[] msgBuffer)
        {
            //deserialization is the tricky part
            try
            {
                using (var ms = new MemoryStream(msgBuffer, 0, msgBuffer.Length))
                {
                    var cmd = Serializer.Deserialize<CommandArgs>(ms);
                    switch (cmd.CommandType)
                    {
                        case CommandType.MovementUpdate:
                            this.state = ((MovementArgs)cmd).MoveState;
                            break;
                        case CommandType.Ability:
                            if (OnSpecialAction != null)
                                OnSpecialAction((ActionArgs)cmd);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private MovementState state = new MovementState();

        //Interface members
        public MovementState MovementState
        {
            get { return state; }
        }

        public event Action<ActionArgs> OnSpecialAction;

        public void Dispose()
        {
            TcpSocket.Dispose();
        }
    }
}
