using Shanism.Editor.Controllers;
using System;
using System.Collections.Generic;

namespace Shanism.Editor.Actions
{
    /// <summary>
    /// A list of the user actions done so far.
    /// </summary>
    class ActionList
    {
        readonly int maxEntries;
        readonly List<ActionBase> actions = new List<ActionBase>();

        readonly EditorGameState client;

        int head = 0;

        public event Action<ActionBase> ActionDone;
        public event Action<ActionBase> ActionUndone;

        public ActionList(EditorGameState client, int maxEntries = 256)
        {
            this.client = client;
            this.maxEntries = maxEntries;
        }

        public void Do(ActionBase act)
        {
            act.Initialize(client);

            act.Apply();

            // remove everything after head
            var toRemove = actions.Count - head;
            if(toRemove > 0)
                actions.RemoveRange(head, toRemove);

            actions.Add(act);
            head = actions.Count;

            ActionDone?.Invoke(act);

            // cleanup? might be a bad idea..
            if(actions.Count >= 2 * maxEntries)
                actions.RemoveRange(0, actions.Count - maxEntries);
        }

        public bool CanUndo(int nSteps = 1)
            => nSteps > 0 && head >= nSteps;

        public bool CanRedo(int nSteps = 1)
            => nSteps > 0 && (actions.Count - head) >= nSteps;

        public ActionBase Previous => head > 1 ? actions[head - 2] : null;
        public ActionBase Current => head > 0 ? actions[head - 1] : null;

        public ActionBase Next => head < actions.Count ? actions[head] : null;

        public bool Undo() => Undo(1);
        public bool Redo() => Redo(1);

        public bool Undo(int nSteps)
        {
            if(!CanUndo(nSteps))
                return false;

            for(int i = 0; i < nSteps; i++)
            {
                head--;
                actions[head].Revert();

                ActionUndone?.Invoke(actions[head]);
            }

            return true;
        }

        public bool Redo(int nSteps)
        {
            if(!CanRedo(nSteps))
                return false;

            for(int i = 0; i < nSteps; i++)
            {
                actions[head].Apply();
                head++;

                ActionDone?.Invoke(actions[head - 1]);
            }

            return true;
        }
    }
}
