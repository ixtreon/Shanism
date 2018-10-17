using Shanism.Common;
using Shanism.Engine.Models.Systems;
using Shanism.Engine.Players;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Shanism.Engine.Systems
{
    sealed class PlayerSystem : GameSystem
    {
        readonly ShanoEngine engine;

        readonly ConcurrentQueue<(IClientReceptor, bool)> pendingConnections = new ConcurrentQueue<(IClientReceptor, bool)>();

        readonly List<ShanoReceptor> receptors = new List<ShanoReceptor>();
        readonly Dictionary<string, ShanoReceptor> receptorLookup = new Dictionary<string, ShanoReceptor>();
        readonly List<IPlayer> players = new List<IPlayer>();

        public override string Name => "Players";

        public IReadOnlyList<IPlayer> Players => players;
        public IReadOnlyList<ShanoReceptor> Receptors => receptors;

        public PlayerSystem(ShanoEngine engine)
        {
            this.engine = engine;
        }


        internal override void Update(int msElapsed)
        {
            // update in-game players
            for(int i = 0; i < receptors.Count; i++)
                receptors[i].Update(msElapsed);
        }


        public bool TryConnect(IClientReceptor cl, bool isHost, out ShanoReceptor receptor)
        {
            if(contains(cl.Name))
            {
                receptor = null;
                return false;
            }

            receptor = new ShanoReceptor(engine, cl, isHost);
            add(receptor);
            return true;
        }

        public ShanoReceptor Disconnect(string name)
        {
            int id;
            if(!receptorLookup.TryGetValue(name, out var r) || (id = receptors.IndexOf(r)) < 0)
                throw new InvalidOperationException($"This receptor is not part of the server.");

            remove(id, name);

            return r;
        }

        public bool TryGetReceptor(string name, out ShanoReceptor receptor)
            => receptorLookup.TryGetValue(name, out receptor);

        public void SendSystemMessage(string message, IPlayer toPlayer)
        {
            if(!receptorLookup.TryGetValue(toPlayer.Name, out var r))
                throw new InvalidOperationException($"Player is not part of the server.");

            r.SendSystemMessage(message);
        }

        public void SendSystemMessage(string message)
        {
            foreach(var r in receptors)
                r.SendSystemMessage(message);
        }

        void add(ShanoReceptor r)
        {
            receptors.Add(r);
            receptorLookup.Add(r.Name, r);
            players.Add(r.Player);
        }

        void remove(int id, string name)
        {
            receptors.RemoveAtFast(id);
            players.RemoveAtFast(id);
            receptorLookup.Remove(name);
        }

        bool contains(string name) => receptorLookup.ContainsKey(name);
    }
}
