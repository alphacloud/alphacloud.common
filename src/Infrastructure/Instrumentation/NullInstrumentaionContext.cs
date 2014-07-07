namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using Core.Instrumentation;


    class NullInstrumentaionContext : IInstrumentationContext
    {
        #region IInstrumentationContext Members

        /// <summary>
        ///   Get number of call by type.
        /// </summary>
        /// <param name="callType">Type filter. If <c>null</c>, total number of calls will be returned.</param>
        /// <returns>Number of calls.</returns>
        public int GetCallCount(string callType = null)
        {
            return 0;
        }


        /// <summary>
        ///   Calculate total duration of calls.
        /// </summary>
        /// <param name="callType">Type filter. If <c>null</c>, all types will be included.</param>
        /// <returns></returns>
        public TimeSpan GetCallDuration(string callType = null)
        {
            return TimeSpan.Zero;
        }


        /// <summary>
        ///   Find duplicated call by type.
        /// </summary>
        /// <param name="callType">Type filter.</param>
        /// <param name="threshold">Minimum number of duplicates.</param>
        /// <returns>List of duplicated calls.</returns>
        public CallSummary[] GetDuplicatedCalls(string callType, int threshold = 2)
        {
            return new CallSummary[0];
        }


        /// <summary>
        ///   Add call information.
        /// </summary>
        /// <param name="callType">Type.</param>
        /// <param name="info">Information</param>
        /// <param name="duration">Duration.</param>
        public void AddCall(string callType, string info, TimeSpan duration)
        {}


        /// <summary>
        ///   Return all captured call types.
        /// </summary>
        /// <returns>Captured call types.</returns>
        public string[] GetCallTypes()
        {
            return new string[0];
        }

        #endregion
    }
}