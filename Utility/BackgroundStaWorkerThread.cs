using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SvgViewer.Utility
{
    public class BackgroundStaWorkerThread : IWorkerQueue
    {
        private readonly object _lock = new object();
        private readonly ConcurrentQueue<Action> _jobs = new ConcurrentQueue<Action>();
        private bool _isRuning;
        public TimeSpan Timeout { get; }
        public event Action Killed;

        private bool IsRuning
        {
            get => _isRuning;
            set
            {
                lock (_lock)
                {
                    _isRuning = value;
                }
            }
        }
        

        public int JobsCount => _jobs.Count;
        public BackgroundStaWorkerThread(TimeSpan timeout)
        {
            Timeout = timeout;
        }

        private void RunInternal()
        {
            if (_isRuning)
                return;
            var thread = new Thread(() =>
            {
                IsRuning = true;
                var date = DateTime.Now;

                while (IsRuning)
                {
                    lock (_lock)
                    {
                        _jobs.TryDequeue(out var action);
                        action?.Invoke();
                    }

                    if (DateTime.Now - date > Timeout && JobsCount == 0)
                    {
                        IsRuning = false;
                    }
                }
                Killed?.Invoke();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();        
        }

        public void AddJob(Action action)
        {
            lock (_lock)
            {
                _jobs.Enqueue(action);
                RunInternal();
            }
        }

        public void Kill()
        {
            IsRuning = false;
        }
    }
}
