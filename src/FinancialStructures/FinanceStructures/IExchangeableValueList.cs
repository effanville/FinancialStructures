namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A <see cref="IValueList"/> implementation that also has the ability to exchange
    /// the values with a <see cref="ICurrency"/>.
    /// </summary>
    public interface IExchangeableValueList : IReadOnlyExchangeableValueList, IValueList
    {
    }
}
