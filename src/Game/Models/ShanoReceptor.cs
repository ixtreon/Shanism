using Shanism.Common;
using Shanism.Common.Messages;
using System;

namespace Shanism.Client.Game
{
    /// <summary>
    /// Connects to a <see cref="IEngine"/> and works with the provided <see cref="IEngineReceptor"/>.
    /// Interfaces with the client internals.
    /// </summary>
    public class ShanoReceptor : IClientReceptor
    {

        readonly IEngine server;

        public IEngineReceptor Receptor { get; private set; }

        /// <summary>
        /// Gets the name of the local player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the player state as reported to the server.
        /// </summary>
        public PlayerState PlayerState { get; } = new PlayerState();

        /// <summary>
        /// Gets the game client state indicating the connection state.
        /// </summary>
        public ClientState State { get; private set; }

        /// <summary>
        /// Executed whenever a message is sent by the server.
        /// </summary>
        public event Action<ServerMessage> MessageReceived;


        public ShanoReceptor(string playerName, IEngine server)
        {
            Name = playerName;
            this.server = server ?? throw new ArgumentNullException(nameof(server));

            Reconnect();
        }

        void Reconnect()
        {
            State = ClientState.Connecting;

            // connect to the server; for SP, indicate we're the host
            if (server is Engine.ShanoEngine engine)
                Receptor = engine.Connect(this, true);
            else
                Receptor = server.Connect(this);

            if (Receptor == null)
                throw new Exception("Server rejected our connection.");

            State = ClientState.Connected;
        }

        public void StartPlaying()
        {
            if (State != ClientState.Connected)
                throw new InvalidOperationException($"Can't start playing unless in the `{nameof(ClientState.Connected)}` state.");

            Receptor.StartPlaying();
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        public void SendMessage(ClientMessage msg)
            => Receptor.HandleMessage(msg);

        /// <summary>
        /// Gets the debug string of the server.
        /// </summary>
        public string GetDebugString()
            => Receptor?.GetDebugString();

        public void Update(int msElapsed)
        {
            server.Update(msElapsed);
        }

        public void HandleMessage(ServerMessage baseMsg)
        {
            if (baseMsg == null)
                throw new Exception();

            switch (baseMsg.Type)
            {
                case ServerMessageType.HandshakeReply:
                    State = ((HandshakeReply)baseMsg).Success ? ClientState.Playing : ClientState.Rejected;
                    break;

                case ServerMessageType.Disconnected:
                    State = ClientState.Disconnected;
                    break;
            }

            MessageReceived?.Invoke(baseMsg);
        }


        public void RestartScenario()
        {
            if (server == null)
                throw new InvalidOperationException("No server to restart...");

            if (!server.TryRestartScenario(out var errors))
                throw new Exception($"Server could not restart the scenario: {errors}");

            Disconnect();
            Reconnect();
        }

        /// <summary>
        /// Instructs 
        /// </summary>
        public void Disconnect()
        {
            if (Receptor != null)
            {
                Receptor.Disconnect();
                Receptor = null;
            }

            State = ClientState.Disconnected;
        }
    }
}
