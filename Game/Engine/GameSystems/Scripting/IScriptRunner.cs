using System;
using Shanism.Engine.Systems;
using System.Threading;
using Shanism.Engine.Objects.Buffs;

namespace Shanism.Engine.Scripting
{
    interface IScriptRunner
    {
        void Run(Action<CustomScript> act);

        void Enqueue(Action act);
    }
}