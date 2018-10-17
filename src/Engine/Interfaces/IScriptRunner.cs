using Shanism.Engine.Systems.Scripts;
using System;

namespace Shanism.Engine.Systems
{
    interface IScriptRunner
    {
        /// <summary>
        /// Executes the specified script action for all loaded scripts.
        /// </summary>
        void Run(Action<CustomScript> act);

        //TODO: is this method really needed anymore?
        void Enqueue(Action act);
    }
}