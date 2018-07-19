using System;
using System.Collections.Concurrent;

namespace SvgViewer.Utility
{
    public class StaWorkerQueue
    {
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        public void AddWork(Action work)
        {
            _actions.Enqueue(work);
            Start();
        }

        private bool _isRuning = false;
        public bool IsRuning => _isRuning;
        private void Start()
        {
            if (_isRuning is false)
            {
                _isRuning = true;
                RunWork();
            }
        }

        private void RunWork()
        {
            _actions.TryDequeue(out var action);

            if (action is null)
            {
                _isRuning = false;
                return;
            }

            TaskEx.RunOnSta(action)
                .ContinueWith(x => RunWork());
        }
    }
}
