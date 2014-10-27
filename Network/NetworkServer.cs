using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Commands;
using IO.Common;

namespace Network
{
    /// <summary>
    /// A client willing to play online should create one of these and pretend it's a real server. 
    /// </summary>
    class NetworkServer : IServer
    {
        public readonly int Port;
        public readonly Socket TcpSocket;

        public IHero LocalHero
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private MovementState movementState;

        public MovementState MovementState
        {
            get { return movementState; }

            set
            {
                //TODO: send a command to the server

                movementState = value;
            }
        }

        public NetworkServer(string hostAddress, int port)
        {
            var ipHostInfo = Dns.GetHostEntry(hostAddress);
            var ipAddress = ipHostInfo.AddressList.First();

            var remoteEp = new IPEndPoint(ipAddress, port);

            TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            TcpSocket.BeginConnect(remoteEp, onConnect, null);
        }

        private void onConnect(IAsyncResult ar)
        {
            TcpSocket.EndConnect(ar);

            Console.WriteLine("Connected Successfully to the server!");

            //TODO: start reading from the server
        }
        public void RegisterAction(ActionArgs args)
        {
            //TODO: send a command to the server
            throw new NotImplementedException();
        }

        public void GetNearbyTiles(ref MapTile[,] tiles, out double x, out double y)
        {
            //TODO: should be sent and updated by the server in the background
            throw new NotImplementedException();
        }

        public IEnumerable<IUnit> GetUnits()
        {
            //TODO: should be sent and updated by the server in the background
            throw new NotImplementedException();
        }

        public IEnumerable<IGameObject> GetGameObjects()
        {
            throw new NotImplementedException();
        }
    }
}
