using Shanism.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Message.Network;
using Shanism.Common.Message.Server;
using Shanism.Common.Message.Client;
using Shanism.Engine.Systems.Orders;
using Shanism.Common.Game;
using Shanism.Engine.Entities;
using Shanism.Engine.Serialization;

namespace Shanism.Engine.Players
{
    /// <summary>
    /// Represents a client connected to the engine. 
    /// Handles all messages coming from the given client.  
    /// </summary>
    class ShanoReceptor : INetReceptor
    {
        static EngineSerializer serializer = new EngineSerializer();

        /// <summary>
        /// The engine this player is part of. 
        /// </summary>
        ShanoEngine Engine { get; }

        /// <summary>
        /// Gets the client handle of this player. 
        /// </summary>
        public IShanoClient Client { get; }

        /// <summary>
        /// Gets the underlying in-game player represented by this receptor. 
        /// </summary>
        public Player Player { get; }


        Hero MainHero => Player.MainHero;

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name => Client.Name;


        public ShanoReceptor(ShanoEngine engine, IShanoClient client)
        {
            Engine = engine;
            Client = client;
            Player = new Player(this, client.Name);    //broken circuitry..

            Player.ObjectSeen += onPlayerObjectSeen;
            Player.ObjectUnseen += onPlayerObjectUnseen;
            Player.MainHeroChanged += onPlayerHeroChange;

            Client.MessageSent += parseClientMessage;
        }

        #region Player listeners
        void onPlayerObjectUnseen(Entity obj)
        {
            SendMessage(new ObjectUnseenMessage(obj.Id, obj.IsDestroyed));
        }

        void onPlayerHeroChange(Hero h)
        {
            SendMessage(new PlayerStatusMessage(h.Id));

            //TODO: change to OnPlayerHeroChanged
            Engine.Scripts.Run(s => s.OnPlayerMainHeroChanged(Player));
        }

        void onPlayerObjectSeen(Entity obj)
        {
            SendMessage(new ObjectSeenMessage(obj));
        }
        #endregion

        #region IShanoClient listeners

        async void parseClientMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.Action:
                    Player.MainHero?.TryCastAbility((ActionMessage)msg);
                    break;

                case MessageType.MoveUpdate:
                    parseMoveMessage((MoveMessage)msg);
                    break;

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
            var pls = Player.controlledUnits
                .SelectMany(u => u.visibleFromUnits)
                .Select(u => u.Owner)
                .Where(pl => pl.IsHuman)
                .Distinct()
                .ToList();

            var ev = new Events.PlayerChatArgs(Player, text);

            Engine.Scripts.Run(s => s.OnPlayerChatMessage(ev));

            if (ev.Propagate)
            {
                var outMsg = new Shanism.Common.Message.Server.ChatMessage(text, Player);
                foreach (var pl in pls)
                    SendMessage(outMsg);
            }
        }

        void parseMoveMessage(MoveMessage msg)
        {
            if (MainHero != null)
            {
                if (msg.IsMoving)
                    MainHero.SetOrder(new PlayerMoveOrder(msg.AngleRad));
                else
                    MainHero.ClearOrder();
            }
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


        internal void SendMessage(IOMessage msg) => MessageSent(msg);


        public string GetPerfData() => Engine.PerformanceData;

        public void UpdateServer(int msElapsed) => Engine.Update(msElapsed);


        public GameFrameMessage GetCurrentFrame()
        {
            var msg = serializer.PrepareGameFrame(Player, Player.VisibleObjects);
            return msg;
        }

        public void SendHandshake(bool isSuccessful)
        {
            if (!isSuccessful)
            {
                SendMessage(HandshakeReplyMessage.Negative);
                return;
            }

            var scConfig = Engine.Scenario.Config;
            var msg = new HandshakeReplyMessage(true, scConfig.SaveToBytes(), scConfig.ZipContent());

            SendMessage(msg);
        }
        #endregion
    }
}
