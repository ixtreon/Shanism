using Engine.Objects;
using IO.Objects;
using ScenarioLib;
using ShanoEditor.ViewModels;
using System.Collections.Generic;

namespace ShanoEditor.MapAdapter
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