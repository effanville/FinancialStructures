namespace FinancialStructures.FinanceInterfaces
{
    public interface ICurrency : ISingleValueDataList
    {
        string BaseCurrency
        {
            get;
        }
        string QuoteCurrency
        {
            get;
        }
        ICurrency Inverted();
        new ICurrency Copy();
    }
}
