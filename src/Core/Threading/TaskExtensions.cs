namespace Alphacloud.Common.Core.Threading
{
    #region using

    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    #endregion

    [PublicAPI]
    public static class TaskExtensions
    {
        /// <summary>
        ///   Wait for task completion ignoring OperationCancelled exceptions.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static bool SafeWait(this Task task, TimeSpan timeout)
        {
            try
            {
                return task.Wait(timeout);
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => e is OperationCanceledException);
                return true;
            }
        }
    }
}