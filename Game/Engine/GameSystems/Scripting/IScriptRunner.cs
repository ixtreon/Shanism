using System;
using Shanism.Engine.Systems;

namespace Shanism.Engine.Scripting
{
    interface IScriptRunner
    {
        void Run(Action<CustomScript> act);
    }
}