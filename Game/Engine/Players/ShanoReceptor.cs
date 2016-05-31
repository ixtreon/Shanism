using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Message.Network;
using Shanism.Common.Message.Server;
using Shanism.Common.Util;
using Shanism.Engine.Objects;
using Shanism.Common.Message.Client;
using Shanism.Engine.Systems.Orders;
using Shanism.Common.Game;
using Shanism.Common.Objects;
using Shanism.Engine.Entities;

namespace Shanism.Engine.Players
{
    /// <summary>
    /// Represents a human player connected to the engine. 
    /// </summary>
    class ShanoReceptor : INetReceptor
    {
        /// <summary>
        /// The engine this player is part of. 
        /// </summary>
        ShanoEngine Engine { get; }

        /// <summary>
        /// Gets the client handle of this player. 
        /// </summary>
        IShanoClient InputDevice { get; }

        /// <summary>
        /// Gets the underlying in-game player represented by this receptor. 
        /// </summary>
        public Player Player { get; }


        Hero MainHero => Player.MainHero;

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name => InputDevice.Name;


        public ShanoReceptor(ShanoEngine engine, IShanoClient inputDevice)
        {
            Engine = engine;
            InputDevice = inputDevice;
            Player = new Player(this, inputDevice.Name);    //broken circuitry..

            Player.ObjectSeen += onPlayerObjectSeen;
            Player.ObjectUnseen += onPlayerObjectUnseen;
            Player.MainHeroChanged += onPlayerHeroChange;

            InputDevice.MessageSent += parseClientMessage;
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
            Engine.Scripts.Run(s => s.OnHeroSpawned(h));
        }

        void onPlayerObjectSeen(Entity obj)
        {
            SendMessage(new ObjectSeenMessage(obj));
        }
        #endregion

        #region IShanoClient listeners

        void parseClientMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.Action:
                    Player.MainHero?.TryCastAbility((ActionMessage)msg);
                    break;

                case MessageType.MoveUpdate:
                    updateHeroMovement((MoveMessage)msg);
                    break;

                case MessageType.MapRequest:
                    parseMapRequest((MapRequestMessage)msg);
                    break;
            }
        }

        void updateHeroMovement(MoveMessage msg)
        {
            if (MainHero != null)
            {
                var ms = msg.Direction;
                if (ms.IsMoving)
                    MainHero.SetOrder(new PlayerMoveOrder(ms));
                else
                    MainHero.ClearOrder();
            }
        }

        async void parseMapRequest(MapRequestMessage msg)
        {
            var chunk = msg.Chunk;
            var chunkData = new TerrainType[chunk.Span.Area];

            await Task.Run(() => Engine.GetTiles(this, ref chunkData, chunk));

            SendMessage(new MapDataMessage(chunk.Span, chunkData));
        }

        #endregion

        #region IReceptor Implementation
        public event Action<IOMessage> MessageSent;


        internal void SendMessage(IOMessage msg) => MessageSent(msg);


        public string GetPerfData()
        {
            return Engine.GetPerfData();
        }

        public void UpdateServer(int msElapsed)
        {
            Engine.Update(msElapsed);
        }

        public GameFrameMessage GetCurrentFrame()
        {
            var msg = Serialization.ShanoReader.PrepareGameFrame(Player.VisibleObjects);
            return msg;
        }

        public void SendHandshake(bool isSuccessful)
        {
            var scConfig = Engine.Scenario.Config;

            SendMessage(new HandshakeReplyMessage(isSuccessful, scConfig.GetBytes(), scConfig.ZipContent()));
        }
        #endregion
    }
}
