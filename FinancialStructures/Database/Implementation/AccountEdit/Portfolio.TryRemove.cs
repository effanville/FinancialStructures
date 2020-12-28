using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryRemove(Account elementType, TwoName name, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            switch (elementType)
            {
                case (Account.Security):
                {
                    foreach (Security sec in Funds)
                    {
                        if (name.IsEqualTo(sec.Names))
                        {
                            _ = Funds.Remove(sec);
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Security {name} removed from the database.");
                            OnPortfolioChanged(Funds, new PortfolioEventArgs(elementType));
                            return true;
                        }
                    }

                    break;
                }
                case (Account.Currency):
                {
                    foreach (Currency currency in Currencies)
                    {
                        if (name.IsEqualTo(currency.Names))
                        {
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted currency {currency.Names.Name}");
                            _ = Currencies.Remove(currency);
                            OnPortfolioChanged(Currencies, new PortfolioEventArgs(elementType));
                            return true;
                        }
                    }

                    break;
                }
                case (Account.BankAccount):
                {
                    foreach (CashAccount acc in BankAccounts)
                    {
                        if (name.IsEqualTo(acc.Names))
                        {
                            _ = BankAccounts.Remove(acc);
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleting Bank Account: Deleted {name}.");
                            OnPortfolioChanged(BankAccounts, new PortfolioEventArgs(elementType));
                            return true;
                        }
                    }

                    break;
                }
                case (Account.Benchmark):
                {
                    foreach (Sector sector in BenchMarks)
                    {
                        if (name.IsEqualTo(sector.Names))
                        {
                            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted benchmark {sector.Names.Name}");
                            _ = BenchMarks.Remove(sector);
                            OnPortfolioChanged(BenchMarks, new PortfolioEventArgs(elementType));
                            return true;
                        }
                    }

                    break;
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
                    return false;
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"{elementType} - {name} could not be found in the database.");
            return false;
        }
    }
}
