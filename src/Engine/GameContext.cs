using Shanism.Common.Util;
using Shanism.Engine.Exceptions;
using Shanism.Engine.Systems;
using Shanism.ScenarioLib;
using System;

namespace Shanism.Engine
{
    static class GameContext
    {
        public static ShanoEngine Instance { get; private set; }

        /// <summary>
        /// Tries to set the game engine instance all game objects are part of.
        /// Fails if there is another engine registered already. 
        /// </summary>
        public static void SetGame(ShanoEngine game)
        {
            if (Instance != null)
                throw new SingleServerException();

            Instance = game ?? throw new ArgumentNullException(nameof(game));
        }

        public static void ResetGame()
        {
            if(Instance == null)
                throw new InvalidOperationException("No server is currently registered.");

            Instance = null;
        }
    }

    public class GameComponent
    {
        /// <summary>
        /// Gets the game this object is part of.
        /// Only initialized once the object is actually part of the game, 
        /// e.g. when an entity is added to the map, a buff is applied, or an ability is learned.
        /// </summary>
        ShanoEngine Game => GameContext.Instance;

        /// <summary>
        /// Gets the map that contains the terrain and units in this scenario. 
        /// </summary>
        public IGameMap Map => Game.Map;

        /// <summary>
        /// Gets the scenario this object is part of. 
        /// </summary>
        public Scenario Scenario => Game.Scenario;

        /// <summary>
        /// Gets the shared performance counter for this object's game.
        /// </summary>
        internal PerfCounter UnitSystemPerfCounter => Game.debugPerfUnits;

        /// <summary>
        /// Gets the shared script runner for this object's game.
        /// </summary>
        internal IScriptRunner Scripts => Game.Scripts;

        internal Allocator ObjectAllocator => Game.Allocator;
    }
}
