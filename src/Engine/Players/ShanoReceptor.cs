using Shanism.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Shanism.Common.Messages;
using Shanism.Engine.Entities;
using System.Collections.Generic;
using Shanism.Common.Entities;
using Shanism.Engine.Objects.Orders;

namespace Shanism.Engine.Players
{

    enum ReceptorState
    {
        Connecting, Playing, Disconnected
    }

    /// <summary>
    /// A player connected to a shano engine.
    /// Handles all messages coming from the given client.  
    /// Tightly coupled with the <see cref="Shanism.Engine.Player"/> class. 
    /// </summary>
    class ShanoReceptor : GameComponent, IEngineReceptor
    {
        /// <summary>
        /// Gets the engine this receptor is part of. 
        /// </summary>
        ShanoEngine Engine { get; }

        /// <summary>
        /// Gets the in-game player behind this receptor. 
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the client instance behind this receptor. 
        /// </summary>
        public IClientReceptor Client { get; }


        public ReceptorState State { get; private set; }

        public bool IsHost { get; }



        internal event Action<ShanoReceptor> OnDisconnect;

        public uint PlayerId => Player.Id;

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name => Client.Name;

        public IReadOnlyCollection<IEntity> VisibleEntities => Player.visibleEntities;

        Hero MainHero => Player.MainHero;


        public ShanoReceptor(ShanoEngine engine, IClientReceptor client, bool isHost)
        {
            Engine = engine;
            Client = client;
            IsHost = isHost;

            Player = new Player(this, client.Name);
            Player.MainHeroChanged += onPlayerHeroChange;
        }


        void onPlayerHeroChange(Hero h)
        {
            SendMessage(new PlayerStatus(h.Id));

            Scripts.Run(s => s.OnPlayerMainHeroChanged(Player));
        }

        #region IShanoClient listeners

        public void HandleMessage(ClientMessage msg)
        {
            switch(msg.Type)
            {
                case ClientMessageType.MapRequest:
                    handleMapRequest((MapRequest)msg);
                    break;

                case ClientMessageType.Chat:
                    parseChat((ClientChat)msg);
                    break;

                default:
                    throw new Exception();
            }
        }

        void parseChat(ClientChat msg)
        {
            var text = msg.Message;

            var seenFromPlayers = new HashSet<Player>();
            foreach(var ourUnit in Player.controlledUnits)
                foreach(var seenFromUnit in ourUnit.visibleFromUnits)
                    if(seenFromUnit.Owner.IsHuman)
                        seenFromPlayers.Add(seenFromUnit.Owner);

            //send to nearby players (TODO)
            var outMsg = new ServerChat(Player, text);
            //foreach (var pl in seenFromPlayers)
            //    pl.SendMessage(outMsg);

            SendMessage(outMsg);

            //finally send to scripts
            var ev = new Events.PlayerChatArgs(Player, text);
            Scripts.Run(s => s.OnPlayerChatMessage(ev));
        }


        void handleMapRequest(MapRequest msg)
        {
            var chunk = msg.Chunk;
            var chunkData = new TerrainType[chunk.Bounds.Area];

            Map.Terrain.Get(chunk.Bounds, ref chunkData);

            SendMessage(new MapData(chunk.Bounds, chunkData));
        }

        #endregion

        #region IReceptor Implementation

        /// <summary>
        /// Sends a message to the client.
        /// </summary>
        internal void SendMessage(ServerMessage msg)
            => Client.HandleMessage(msg);

        public void StartPlaying()
        {
            if(State != ReceptorState.Connecting)
                return;

            SendHandshake(true);
            State = ReceptorState.Playing;

            Scripts.Run(s => s.OnPlayerJoined(Player));
        }

        /// <summary>
        /// Disconnects the client
        /// </summary>
        public void Disconnect()
        {
            if(State != ReceptorState.Playing)
                return;

            Engine.Disconnect(Name);

            State = ReceptorState.Disconnected;
            OnDisconnect?.Invoke(this);
        }

        /// <summary>
        /// Gets some kind of debug info.
        /// </summary>
        public string GetDebugString()
            => Engine.DebugString;

        public void Update(int msElapsed)
        {
            if(State != ReceptorState.Playing)
                return;

            if(MainHero == null)
                return;

            if(!(MainHero.DefaultOrder is MoveDirection move))
                MainHero.DefaultOrder = move = new MoveDirection(MainHero);

            // movement & actions
            var curState = Client.PlayerState;
            move.Angle = curState.IsMoving ? curState.MoveAngle : float.NaN;
            MainHero.TryCastAbility(curState);
        }

        /// <summary>
        /// Sends a chat message to the client.
        /// </summary>
        internal void SendSystemMessage(string msg)
            => SendMessage(new ServerChat(null, msg));

        /// <summary>
        /// Sends a handshake to the client.
        /// </summary>
        /// <param name="isSuccessful"></param>
        internal void SendHandshake(bool isSuccessful)
        {
            if(!isSuccessful)
            {
                SendMessage(HandshakeReply.Negative);
                return;
            }

            var scConfig = Scenario.Config;
            var msg = new HandshakeReply(PlayerId, scConfig.SaveToBytes(), scConfig.ZipContent());

            SendMessage(msg);
        }
        #endregion
    }
}
