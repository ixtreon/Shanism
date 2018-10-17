using Shanism.Client.UI.Containers;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A UI control which loads a scenario.
    /// </summary>
    class LoadingBox<T> : Window
    {
        readonly Func<bool> hasLoaded;
        readonly Action<Task<T>> handler;

        readonly Task<T> task;

        public bool HasLoaded { get; private set; }

        public LoadingBox(Task<T> task, Action<Task<T>> handler)
        {
            this.task = task;

            hasLoaded = () => task.GetAwaiter().IsCompleted;
            this.handler = handler;

            Size = new Vector2(0.5f, 0.3f);
            Add(new Label
            {
                Bounds = ClientBounds,
                ParentAnchor = AnchorMode.All,

                TextAlign = AnchorPoint.Center,
                Text = "Compiling...",
            });

            IsVisible = true;
        }

        public override void Update(int msElapsed)
        {
            if(task.GetAwaiter().IsCompleted)
            {
                handler(task);

                Parent.Remove(this);
                return;
            }

            base.Update(msElapsed);
        }
    }
}
