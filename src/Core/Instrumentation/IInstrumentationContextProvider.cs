namespace Alphacloud.Common.Core.Instrumentation
{
    using JetBrains.Annotations;


    /// <summary>
    ///   Instrumentation Context provider.
    /// </summary>
    public interface IInstrumentationContextProvider
    {
        /// <summary>
        ///   Returns current instrumentation context.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        IInstrumentationContext GetInstrumentationContext();


        /// <summary>
        ///   Set current instrumentation context.
        /// </summary>
        /// <param name="instrumentationContext"></param>
        void SetInstrumentationContext([CanBeNull] IInstrumentationContext instrumentationContext);


        /// <summary>
        ///   Re-initialize context.
        /// </summary>
        void Reset();
    }

}