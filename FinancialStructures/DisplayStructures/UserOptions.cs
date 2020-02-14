using System.Collections.Generic;

namespace PortfolioStatsCreatorHelper
{
    public class UserOptions
    {
        public List<string> SecurityDataToExport { get; set; }

        public List<string> BankAccDataToExport { get; set; }

        public List<string> SectorDataToExport { get; set; }
        public bool DisplayValueFunds { get; set; }
        public bool Spacing { get; set; }

        public bool Colours { get; set; }

        public bool ShowSecurites { get; set; }
        public bool ShowBankAccounts { get; set; }

        public bool ShowSectors { get; set; }
    }
}
