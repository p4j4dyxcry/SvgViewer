using System;

namespace SvgViewer.Utility
{
    public class StaWorkerManager
    {
        private readonly IWorkerQueue[] _workerQueue;
        public StaWorkerManager(int workerCount , Action killed = null)
        {
            _workerQueue = new IWorkerQueue[workerCount <= 0 ? 1 : workerCount];
            for (var i = 0; i < workerCount; ++i)
            {
                //_workerQueue[i] = new StaWorkerQueue();
                _workerQueue[i] = new BackgroundStaWorkerThread(TimeSpan.FromSeconds(10));
                _workerQueue[i].Killed += killed;
            }
        }

        public void AddJob(Action job)
        {
            GetLessWorkerQueue().AddJob(job);
        }

        private IWorkerQueue GetLessWorkerQueue()
        {
            return _workerQueue.FindMin(x => x.JobsCount);
        }
    }
}
