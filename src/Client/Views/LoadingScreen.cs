using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using System;
using System.Numerics;
using System.Threading.Tasks;


namespace Shanism.Client.Views
{
    public class LoadingScreen : View
    {
        public bool AsyncLoad { get; set; } = false;  // debugging

        const float ColorFlipDuration = 100;
        const float GrayMax = 24;
        const float GrayRate = 1.05f;

        static readonly Color BackColorA = new Color(24, 24, 24);
        static readonly Color BackColorB = Color.Black;

        Task loadingTask;

        public bool HasFinished { get; private set; }
        public bool WasSuccessful { get; private set; }
        public Exception Exception { get; private set; }

        readonly Action mainTask;
        readonly Action successTask;
        readonly Action errorTask;

        readonly CounterF colorTimer = new CounterF(ColorFlipDuration * 2);

        Color backColor;

        public LoadingScreen(Action mainTask, Action successTask = null, Action errorTask = null)
        {
            this.errorTask = errorTask;
            this.successTask = successTask;
            this.mainTask = mainTask;
        }

        public override void Update(int msElapsed)
        {
            if (!AsyncLoad)
            {
                mainTask();
                setFinished(null);
                return;
            }

            switch (loadingTask?.Status)
            {
                case null:
                    loadingTask = new Task(mainTask);
                    loadingTask.Start();
                    break;

                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                    loadingTask.Start();
                    break;

                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:

                    // loading: update back color
                    colorTimer.Tick(msElapsed);

                    var v = (colorTimer.Value / ColorFlipDuration - 1);     // should take abs
                    var vsq = v * v;                                        // but is always squared
                    backColor = Color.Lerp(BackColorA, BackColorB, vsq);

                    break;

                case TaskStatus.RanToCompletion:
                    setFinished(null);
                    break;

                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    setFinished(loadingTask.Exception);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void setFinished(Exception e)
        {
            if (HasFinished)
                return;

            HasFinished = true;
            Exception = e;
            WasSuccessful = e == null;

            if (WasSuccessful)
                successTask?.Invoke();
            else
                errorTask?.Invoke();
        }


        protected override void OnDraw(CanvasStarter canvas)
        {
            using (var c = canvas.BeginScreen(SamplerState.PointClamp, SpriteSortMode.Deferred))
            {
                c.Clear(backColor);
                c.FillRectangle(Vector2.Zero, new Vector2(50), Color.Blue);
            }
        }
    }
}
