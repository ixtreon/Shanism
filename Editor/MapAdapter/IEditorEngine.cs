using Shanism.Common.Message;
using Shanism.Editor.ViewModels;
using Shanism.Engine;
using Shanism.ScenarioLib;
using System.Collections.Generic;

namespace Shanism.Editor.MapAdapter
{
    interface IEditorEngine
    {
        ScenarioViewModel ScenarioView { get; }

        IReadOnlyCollection<Entity> StartupObjects { get; }

        Entity CreateObject(ObjectConstructor oc);

        bool AddObject(Entity o);

        bool RemoveObject(Entity o);

        void SendMessage(IOMessage msg);
    }
}