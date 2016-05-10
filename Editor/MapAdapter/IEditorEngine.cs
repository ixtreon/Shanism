using Shanism.Engine.Objects;
using Shanism.Common.Objects;
using Shanism.ScenarioLib;
using Shanism.Editor.ViewModels;
using System.Collections.Generic;

namespace Shanism.Editor.MapAdapter
{
    interface IEditorEngine
    {
        IEnumerable<Entity> StartupObjects { get; }

        ScenarioViewModel ScenarioView { get; }

        Entity CreateObject(ObjectConstructor oc);

        bool AddObject(Entity o);

        bool RemoveObject(Entity o);
    }
}