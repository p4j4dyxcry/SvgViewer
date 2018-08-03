using System;

namespace SvgViewer.Utility
{
    interface IWorkerQueue
    {
        event Action Killed;

        int JobsCount { get; }
        void AddJob(Action action);
    }
}
