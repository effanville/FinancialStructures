
namespace FinancialStructures.Reporting
{
    public enum ReportType
    {
        Error = 0,
        Warning = 1,
        Report = 2
    }

    public enum Location
    {
        Unknown,
        Downloading,
        Saving,
        Loading,
        AddingData,
        EditingData,
        DeletingData,
        Parsing,
        StatisticsPage,
        DatabaseAccess,
        Help
    }

    public enum Severity
    {
        Critical,
        Useful,
        Detailed
    }
}
