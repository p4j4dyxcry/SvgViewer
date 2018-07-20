using System;

namespace SvgViewer.Utility
{
    public class StaWorkerManager
    {
        private readonly StaWorkerQueue[] _workerQueue;
        public StaWorkerManager(int workerCount)
        {
            _workerQueue = new StaWorkerQueue[workerCount <= 0 ? 1 : workerCount];
            for(var i = 0 ; i < workerCount ; ++i)
                _workerQueue[i] = new StaWorkerQueue();
        }

        public void AddJob(Action job)
        {
            GetLessWorkerQueue().AddJob(job);
        }

        private StaWorkerQueue GetLessWorkerQueue()
        {
            return _workerQueue.FindMin(x => x.JobsCount);
        }
    }
}
