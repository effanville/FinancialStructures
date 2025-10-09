namespace Effanville.FinancialStructures.Stocks.Signals;

public interface ISignalState
{
    ISignalConfig Config { get; }
    bool IsValid { get; }
    string RecorderLine { get; }
    string LogLine { get; }
    bool ShouldPublish();
}
