using System;
using Shanism.Engine.Systems;
using System.Threading;
using Shanism.Engine.Objects.Buffs;

namespace Shanism.Engine.Scripting
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