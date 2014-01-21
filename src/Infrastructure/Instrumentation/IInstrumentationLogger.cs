namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    public interface IInstrumentationLogger
    {
        void DatabaseCallCompleted(object sender, InstrumentationEventArgs eventArgs);
        void ServiceCallCompleted(object sender, InstrumentationEventArgs eventArgs);
        void OperationCompleted(object sender, OperationCompletedEventArgs eventArgs);
    }
}