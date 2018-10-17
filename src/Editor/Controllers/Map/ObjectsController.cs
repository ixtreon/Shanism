using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Editor.Actions;
using Shanism.Editor.Actions.Objects;
using Shanism.ScenarioLib;
using System.Numerics;

namespace Shanism.Editor.Controllers
{
    /// <summary>
    /// Controls the startup objects and entities on a map.
    /// </summary>
    class ObjectsController
    {

        readonly ActionList history;
        readonly ScenarioConfig scenario;

        public IEntity BrushPrototype { get; set; }

        public ObjectsController(ActionList history, ScenarioConfig scenario)
        {
            this.history = history;
            this.scenario = scenario;
        }

        public void Add(Vector2 pos)
        {
            if (BrushPrototype == null)
                return;

            var ctor = new ObjectConstructor
            {
                Location = pos,
                TypeName = BrushPrototype.GetType().FullName,

                //TODO:
                Model = null,
                Owner = null,
                Size = 1,
                Tint = Color.White,
            };
            history.Do(new ObjectBrushAction(scenario.Map, ctor));
        }
    }
}
