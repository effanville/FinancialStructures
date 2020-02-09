
namespace FinancialStructures.ReportingStructures
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
        AddingData,
        EditingData,
        Parsing
    }
}
