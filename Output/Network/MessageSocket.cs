using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IO.Network
{
    /// <summary>
    /// Wraps a Socket object, listening for messages of varying length prepended by their length
    /// of (currently) 2 bytes and raises the <see cref="MessageReceived"/> event to signal it. 
    /// </summary>
    public class MessageSocket
    {
        //2 for int16   (0; 32k)
        //4 for int32   (0; 2b)
        const int HEADER_SIZE = 2;

        public readonly Socket Socket;

        private readonly byte[] buffer;
        private readonly int bufferSize;
        private List<byte> messageBuffer = new List<byte>();

        public event Action<byte[]> MessageReceived;
        public struct SocketState
        {
            public Action<SocketState> Callback;
            public int DataSize;
            public int DataRead;
        }

        public MessageSocket(Socket sock, int bufferSize = 1024)
        {
            Socket = sock;
            this.bufferSize = bufferSize;
            buffer = new byte[this.bufferSize];

            Socket.BeginReceive(buffer, 0, this.bufferSize, SocketFlags.None, this.socketReceive, null);
        }

        private void socketReceive(IAsyncResult ar)
        {
            try
            {
                // append the currently read bytes
                var bytesRead = Socket.EndReceive(ar);
                messageBuffer.AddRange(buffer.Take(bytesRead));

                // repeat until we have the header
                if (messageBuffer.Count >= HEADER_SIZE)
                {
                    int msgLength = BitConverter.ToInt16(messageBuffer.Take(HEADER_SIZE).ToArray(), 0);

                    // repeat until we have the whole message
                    if (messageBuffer.Count >= msgLength + HEADER_SIZE)
                    {
                        //get the exact message. 
                        var msg = messageBuffer.Skip(HEADER_SIZE).Take(msgLength).ToArray();

                        // raise the event. 
                        if (MessageReceived != null)
                            MessageReceived(msg);

                        // cleanup the message buffer. 
                        messageBuffer.RemoveRange(0, msgLength + HEADER_SIZE);
                    }
                }

                Socket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, socketReceive, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading stream {0}:\n{1}", Socket.ToString(), e.ToString());
            }
        }
    }
}
