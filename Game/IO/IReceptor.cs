using IO.Common;
using IO.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// A game receptor as exposed by the engine to the world. 
    /// 
    /// Event based for simplicity.  
    /// </summary>
    public interface IReceptor
    {
        event Action<HandshakeReplyMessage> HandshakeReplied;

        event Action<PlayerStatusMessage> MainHeroChanged;
        
        event Action<MapReplyMessage> MapChunkReceived;

        //event Action<IUnit, IPlayer> UnitOwnerChanged;

        event Action<IGameObject> ObjectUnseen;

        event Action<IGameObject> ObjectSeen;

        event Action<IUnit> AnyUnitAction;

        /// <summary>
        /// Causes the underlying game server to update. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last invocation of this method. </param>
        void UpdateServer(int msElapsed);
    }
}
