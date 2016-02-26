using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Message;
using IO.Message.Network;
using IO.Message.Server;
using IO.Util;
using Engine.Objects;
using IO.Message.Client;
using Engine.Systems.Orders;
using IO.Common;
using IO.Objects;
using Engine.Objects.Entities;

namespace Engine.Players
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

            InputDevice.ActionActivated += inputDevice_ActionActivated;
            InputDevice.MapRequested += inputDevice_MapRequested;
            InputDevice.MovementStateChanged += inputDevice_MovementStateChanged;
            InputDevice.HandshakeInit += inputDevice_HandshakeInit;

        }

        #region Player listeners
        void onPlayerObjectUnseen(Entity obj)
        {
            SendMessage(new ObjectUnseenMessage(obj.Id));
        }

        void onPlayerHeroChange(Hero h)
        {
            SendMessage(new PlayerStatusMessage(h.Id));

            //TODO: change to OnPlayerHeroChanged
            Engine.Scenario.RunScripts(s => s.OnHeroSpawned(h));
        }

        void onPlayerObjectSeen(Entity obj)
        {
            SendMessage(new ObjectSeenMessage(obj));
        }
        #endregion

        #region IShanoClient listeners
        void inputDevice_ActionActivated(ActionMessage obj)
        {
            if (MainHero == null)
                return;
            Player.MainHero.TryCastAbility(obj);
        }

        void inputDevice_HandshakeInit()
        {
            //run scripts?!?!?!?!?!?
        }

        void inputDevice_MovementStateChanged(MoveMessage msg)
        {
            if (MainHero == null)
                return;

            var newState = msg.Direction;
            if (newState.IsMoving)
                MainHero.SetOrder(new PlayerMoveOrder(msg.Direction));
            else
                MainHero.ClearOrder();
        }

        async void inputDevice_MapRequested(MapRequestMessage msg)
        {
            var chunk = msg.Chunk;
            var chunkData = new TerrainType[chunk.Span.Area];
            await Task.Run(() =>
            {
                Engine.GetTiles(this, ref chunkData, chunk);
            });

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
