using System;

namespace SvgViewer.Utility
{
    public class StaWorkerManager
    {
        private readonly StaWorkerQueue[] _workerQueue;
        public int WorkerCount { get; }
        public StaWorkerManager(int workerCount)
        {
            WorkerCount = workerCount <= 0 ? 1 : workerCount;
            _workerQueue = new StaWorkerQueue[WorkerCount];
        }

        private long _workerId ;
        public void AddWork(Action action)
        {
            _workerId = System.Threading.Interlocked.Increment(ref _workerId);

            GetWorkerQueue(_workerId).AddWork(action);
        }

        private StaWorkerQueue GetWorkerQueue(long id)
        {
            var tagetWorkerIndex = _workerId % WorkerCount;
            return _workerQueue[tagetWorkerIndex] ?? (_workerQueue[tagetWorkerIndex] = new StaWorkerQueue());
        }
    }
}
