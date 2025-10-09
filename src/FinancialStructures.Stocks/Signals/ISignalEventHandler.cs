namespace Effanville.FinancialStructures.Stocks.Signals;

public interface ISignalEventHandler<TSignal, TState, TSignalResponse>
    where TSignal : ISignal<TState>
    where TState : ISignalState
{
    void Publish();
}
