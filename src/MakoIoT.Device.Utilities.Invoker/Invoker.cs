using System;

namespace MakoIoT.Device.Utilities.Invoker
{
    /// <summary>
    /// Provides resilience transient fault handling capabilities.
    /// </summary>
    public static class Invoker
    {
        /// <summary>
        /// Delegate invoked when exception occurs during invocation.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="attempt">Invocation attempt number</param>
        /// <returns>[true] if re-try should be continued; [false] to exit.</returns>
        public delegate bool ExceptionDelegate(Exception ex, int attempt);

        /// <summary>
        /// Tries invoking action number of times.
        /// </summary>
        /// <param name="action">The action to invoke</param>
        /// <param name="maxAttempts">Maximum attempts</param>
        /// <param name="exceptionDelegate">Action to invoke when exception occurs</param>
        public static void Retry(Action action, int maxAttempts, ExceptionDelegate exceptionDelegate = null)
        {
            int attempt = 0;
            while (attempt < maxAttempts)
            {
                attempt++;
                try
                {
                    action();
                    break;
                }
                catch (Exception e)
                {
                    bool exit = attempt == maxAttempts;
                    if (exceptionDelegate != null)
                        exit |= !exceptionDelegate(e, attempt);

                    if (exit)
                        // ReSharper disable once PossibleIntendedRethrow
                        throw e;
                }
            }
        }
    }
}
