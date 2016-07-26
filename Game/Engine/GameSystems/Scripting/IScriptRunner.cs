using System;
using Shanism.Engine.Systems;
using System.Threading;

namespace Shanism.Engine.Scripting
{
    interface IScriptRunner
    {
        SynchronizationContext Context { get; }

        void Run(Action<CustomScript> act);

    }
}