using Shanism.Editor.Controllers;
using Shanism.Editor.Game;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Editor.Actions
{
    abstract class ActionBase
    {

        public EditorGameState Game { get; private set; }

        public string Description { get; protected set; }

        public void Initialize(EditorGameState game)
        {
            Game = game;
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public abstract void Apply();

        public abstract void Revert();
    }


    class CompoundAction : ActionBase
    {
        readonly List<ActionBase> actions;

        public CompoundAction() { }

        public CompoundAction(IEnumerable<ActionBase> actions)
        {
            this.actions = actions.ToList();
        }

        public void Add(ActionBase item) => actions.Add(item);
        public void AddRange(IEnumerable<ActionBase> items) => actions.AddRange(items);

        public override void Apply()
        {
            foreach(var act in actions)
                act.Apply();
        }

        public override void Revert()
        {
            foreach(var act in actions)
                act.Revert();
        }
    }
}
