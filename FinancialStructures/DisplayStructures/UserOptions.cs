using System.Collections.Generic;

namespace FinancialStructures.PortfolioStatsCreatorHelper
{
    public class VisibleName
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public VisibleName()
        { }

        public VisibleName(string name, bool vis)
        {
            Visible = vis;
            Name = name;
        }
    }
    public static class NameGetter
    {
        public static bool GetData(List<VisibleName> names, string nameToSearch)
        {
            foreach (var name in names)
            {
                if (name.Name == nameToSearch)
                {
                    return name.Visible;
                }
            }

            return false;
        }
    }

    public class UserOptions
    {
        public UserOptions()
        { }

        public UserOptions(List<string> securities, List<string> bankaccs, List<string> sectors, List<VisibleName> conditions)
        {
            SecurityDataToExport = securities;
            BankAccDataToExport = bankaccs;
            SectorDataToExport = sectors;
            DisplayValueFunds = NameGetter.GetData(conditions, nameof(DisplayValueFunds));
            Spacing = NameGetter.GetData(conditions, nameof(Spacing));
            Colours = NameGetter.GetData(conditions, nameof(Colours));

            ShowSecurites = NameGetter.GetData(conditions, nameof(ShowSecurites));
            ShowBankAccounts = NameGetter.GetData(conditions, nameof(ShowBankAccounts));
            ShowSectors = NameGetter.GetData(conditions, nameof(ShowSectors));
        }
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
