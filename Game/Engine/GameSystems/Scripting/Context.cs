using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Engine.Scripting
{
    /// <summary>
    /// Taken from: 
    /// http://codereview.stackexchange.com/questions/31820/basic-single-threaded-implementation-of-synchronizationcontext
    /// </summary>
    /// <seealso cref="System.Threading.SynchronizationContext" />
    public class SingleThreadedSynchronizationContext : SynchronizationContext
    {

        readonly ConcurrentQueue<WorkItem> _workItems = new ConcurrentQueue<WorkItem> ();
        readonly Thread _executingThread;

        public SingleThreadedSynchronizationContext(Thread executingThread)
        {
            if (executingThread == null)
                throw new ArgumentNullException(nameof(executingThread));
            _executingThread = executingThread;
        }

        internal bool HasWorkItems => !_workItems.IsEmpty;


        WorkItem executeNextWorkItem()
        {
            WorkItem currentItem;
            if (_workItems.TryDequeue(out currentItem))
            {
                currentItem.Execute();
            }
            return currentItem;
        }

        public void ExecutePendingWorkItems()
        {
            //var oldContext = Current;
            //SetSynchronizationContext(this);

            while (HasWorkItems)
                executeNextWorkItem();

            //SetSynchronizationContext(oldContext);
        }

        public void Post(Action act) => Post(new SendOrPostCallback((_) => act()), null);

        public void Post(Action<object> act, object state) => Post(new SendOrPostCallback(act), state);

        public override void Post(SendOrPostCallback d, object state)
        {
            _workItems.Enqueue(new WorkItem(d, state, null));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if (Thread.CurrentThread == _executingThread)
            {
                var requestedWorkItem = new WorkItem (d, state, null);
                _workItems.Enqueue(requestedWorkItem);

                WorkItem executedWorkItem = null;
                while (HasWorkItems && executedWorkItem != requestedWorkItem)
                    executedWorkItem = executeNextWorkItem();
            }
            else
            {
                using (var reset = new ManualResetEventSlim())
                {
                    _workItems.Enqueue(new WorkItem(d, state, reset));
                    reset.Wait();
                }
            }
        }
    }
    sealed class WorkItem
    {
        readonly SendOrPostCallback _callback;
        readonly object _state;
        readonly ManualResetEventSlim _reset;

        public WorkItem(SendOrPostCallback callback, object state, ManualResetEventSlim reset)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            _callback = callback;
            _state = state;
            _reset = reset;
        }

        public void Execute()
        {
            _callback(_state);
            if (_reset != null)
            {
                _reset.Set();
            }
        }
    }
}
