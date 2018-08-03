using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SvgViewer.Utility
{
    public class StaWorkerQueue : IWorkerQueue
    {
        private readonly ConcurrentQueue<Action> _jobs = new ConcurrentQueue<Action>();
        public int JobsCount => _jobs.Count;

        public event Action Killed;

        public void AddJob(Action job)
        {
            Task.Run(() =>
            {
                var isFirstJob = _jobs.IsEmpty;
                _jobs.Enqueue(() =>
                {
                    job();
                    Killed?.Invoke();
                });
                if(isFirstJob is true)
                    RunWork();
            });
        }

        private void RunWork()
        {
            _jobs.TryDequeue(out var action);

            if (action is null)
                return;

            TaskEx.RunOnSta(action)
                .ContinueWith(x => RunWork());
        }       
    }
}
