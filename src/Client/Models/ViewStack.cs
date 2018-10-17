using Shanism.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Systems
{
    public interface IViewContainer
    {
        void Push(View view);
        View Pop();
        void ResetToMain();
        bool IsMainView { get; }

        [Obsolete("don't like this being here")]
        View Current { get; }
    }

    public class ViewStack : IViewContainer, ISystem
    {

        readonly IClient game;
        readonly List<View> views = new List<View>();

        public View Main => views.Count > 0 ? views[0] : null;

        public View Current => views.Count > 0 ? views[views.Count - 1] : null;

        public bool IsMainView => views.Count == 1;

        public ViewStack(IClient game, View mainView)
        {
            this.game = game;

            views.Add(mainView);
        }

        public void Update(int msElapsed)
            => Current?.DoUpdate(msElapsed);

        public void WriteDebugStats(List<string> stats)
            => Current?.WriteDebugStats(stats);

        public void SetMain(View mainView)
        {
            if(mainView == null)
                throw new ArgumentNullException(nameof(mainView));

            Current?.Hide();
            {
                views.Clear();
                views.Add(mainView);

                mainView.Init(this, game);
            }
            Current.Show();
        }

        /// <summary>
        /// Pushes a new view to the top of the stack.
        /// </summary>
        public void Push(View view)
        {
            Current?.Hide();
            {
                views.Add(view);
                view.Init(this, game);
            }
            Current.Show();
        }

        /// <summary>
        /// Pops the top-most view and displays whatever's underneath it.
        /// </summary>
        public View Pop()
        {
            var topView = Current;

            Current?.Hide();
            {
                views.RemoveAt(views.Count - 1);
            }
            Current?.Show();

            return topView;
        }

        /// <summary>
        /// Pops all child views and displays the main view.
        /// </summary>
        public void ResetToMain()
        {
            Current?.Hide();
            {
                views.RemoveRange(1, views.Count - 1);
            }
            Current.Show();
        }

        void initView(View v)
        {
            v.Init(this, game);
        }
    }
}
