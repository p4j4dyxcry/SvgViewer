using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvgViewer
{
    public static class TaskEx
    {
        public static Task RunOnSta<T>(Func<T> function)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(function());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        public static Task RunOnSta(Action action)
        {
            return RunOnSta(() =>
            {
                action();
                return true;
            });
        }
    }
}
