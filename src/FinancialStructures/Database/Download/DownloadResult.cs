using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Download
{
    internal class DownloadResult
    {
        public NameData Name    { get; set; }
        public bool     Success { get; set; }
        public decimal  Value   { get; set; }
        public override string ToString() => $"[DownloadResult. Name={Name}, Success={Success}, Value={Value}";
    }
}