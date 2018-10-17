using Shanism.Client.UI;
using Shanism.Client.Views;
using Shanism.Common;
using Shanism.Engine;
using System;
using System.Threading.Tasks;

namespace Client.Game.Models
{
    class EngineStarter
    {
        readonly View root;
        readonly Action<IEngine> handler;

        bool isLoading;

        public EngineStarter(View root, Action<IEngine> handler)
        {
            this.root = root;
            this.handler = handler;
        }

        ShanoEngine Load(string path)
        {
            var engine = new ShanoEngine(System.Reflection.Assembly.Load);
            // await loading (non-blocking)
            if (!engine.TryLoadScenario(path, out var errors))
                throw new Exception(errors);

            return engine;
        }

        public void TryLoadWithUI(string path)
        {
            if (isLoading) throw new InvalidOperationException("A scenario is already loading...");
            isLoading = true;

            var task = Task.Run(() => Load(path));
            var box = new LoadingBox<ShanoEngine>(task, FinishLoading);

            box.Hidden += (o, e) => CancelLoading();

            root.Add(box);
        }

        void CancelLoading()
        {
            isLoading = false;
        }

        void FinishLoading(Task<ShanoEngine> task)
        {
            if (!isLoading) return;
            isLoading = false;

            if (task.Status == TaskStatus.RanToCompletion)
                handler(task.Result);
            else
                root.ShowMessageBox($"Compilation failed", task.Exception.Message);
        }
    }
}
