using System;
using Effanville.FinancialStructures.Stocks.Implementation;

namespace Effanville.FinancialStructures.Stocks.Signals;

public interface ISignal<TState> where TState : ISignalState
{
    void OnCandleFinished(DateTime now, ICandle candle, TState state);
    void OnPeriodicTimer(DateTime now, TState state);
}
