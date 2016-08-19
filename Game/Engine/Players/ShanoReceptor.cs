using Shanism.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.Message.Client;
using Shanism.Common.Game;
using Shanism.Engine.Entities;
using System.Collections.Generic;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Engine.Players
{
    /// <summary>
    /// Represents (the internal parts of) a client connected to the engine. 
    /// Handles all messages coming from the given client.  
    /// Tightly coupled with the <see cref="Player"/> class. 
    /// </summary>
    class ShanoReceptor : IReceptor
    {
        /// <summary>
        /// The engine this player is part of. 
        /// </summary>
        ShanoEngine Engine { get; }

        /// <summary>
        /// Gets the client handle of this player. 
        /// </summary>
        public IShanoClient Client { get; }

        public ConnectionState State { get; } = ConnectionState.Playing;

        /// <summary>
        /// Gets the underlying in-game player represented by this receptor. 
        /// </summary>
        public Player Player { get; }


        public uint Id => Player.Id;

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name => Client.Name;

        public IReadOnlyCollection<IEntity> VisibleEntities => (IReadOnlyCollection<IEntity>)Player.visibleEntities;

        Hero MainHero => Player.MainHero;


        public ShanoReceptor(ShanoEngine engine, IShanoClient client)
        {
            Engine = engine;

            Client = client;
            Client.MessageSent += parseClientMessage;

            Player = new Player(this, client.Name);
            Player.MainHeroChanged += onPlayerHeroChange;
        }


        #region Player listeners
        void onPlayerHeroChange(Hero h)
        {
            SendMessage(new PlayerStatusMessage(h.Id));

            Engine.Scripts.Run(s => s.OnPlayerMainHeroChanged(Player));
        }
        #endregion

        #region IShanoClient listeners

        async void parseClientMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.MapRequest:
                    await parseMapRequest((MapRequestMessage)msg);
                    break;

                case MessageType.ClientChat:
                    parseChat((Shanism.Common.Message.Client.ChatMessage)msg);
                    break;
            }
        }

        void parseChat(Shanism.Common.Message.Client.ChatMessage msg)
        {
            var text = msg.Message;

            var seenFromPlayers = new HashSet<Player>();
            foreach (var ourUnit in Player.controlledUnits)
                foreach (var seenFromUnit in ourUnit.visibleFromUnits)
                    if (seenFromUnit.Owner.IsHuman)
                        seenFromPlayers.Add(seenFromUnit.Owner);

            //send to nearby players (TODO)
            var outMsg = new Shanism.Common.Message.Server.ChatMessage(text, Player);
            //foreach (var pl in seenFromPlayers)
            //    pl.SendMessage(outMsg);
            SendMessage(outMsg);

            //finally send to scripts
            var ev = new Events.PlayerChatArgs(Player, text);
            Engine.Scripts.Run(s => s.OnPlayerChatMessage(ev));
        }


        async Task parseMapRequest(MapRequestMessage msg)
        {
            var chunk = msg.Chunk;
            var chunkData = new TerrainType[chunk.Span.Area];

            await Task.Run(() => Engine.Map.Terrain.Get(chunk.Span, ref chunkData));

            SendMessage(new MapDataMessage(chunk.Span, chunkData));
        }

        #endregion

        #region IReceptor Implementation
        public event Action<IOMessage> MessageSent;


        internal void SendMessage(IOMessage msg)
        {
            MessageSent?.Invoke(msg);
        }


        public string GetDebugString() => Engine.DebugString;

        public void Update(int msElapsed)
        {
            if (MainHero != null)
            {
                //movement
                if (Client.State.IsMoving)
                    MainHero.MovementState = new MovementState(Client.State.MoveAngle);
                else
                    MainHero.MovementState = MovementState.Stand;

                //actions
                MainHero.TryCastAbility(Client.State);
            }
        }

        /// <summary>
        /// Sends a chat message to the given player.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        internal void SendSystemMessage(string msg)
        {
            SendMessage(new Shanism.Common.Message.Server.ChatMessage(msg, null));
        }


        internal void SendHandshake(bool isSuccessful)
        {
            if (!isSuccessful)
            {
                SendMessage(HandshakeReplyMessage.Negative);
                return;
            }

            var scConfig = Engine.Scenario.Config;
            var msg = new HandshakeReplyMessage(Id, scConfig.SaveToBytes(), scConfig.ZipContent());

            SendMessage(msg);
        }
        #endregion
    }
}
