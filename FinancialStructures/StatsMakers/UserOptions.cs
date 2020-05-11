using System.Collections.Generic;

namespace FinancialStructures.StatsMakers
{
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
        public List<string> SecurityDataToExport { get; set; } = new List<string>();

        public List<string> BankAccDataToExport { get; set; } = new List<string>();

        public List<string> SectorDataToExport { get; set; } = new List<string>();
        public bool DisplayValueFunds { get; set; } = false;
        public bool Spacing { get; set; } = false;

        public bool Colours { get; set; } = false;

        public bool ShowSecurites { get; set; } = true;
        public bool ShowBankAccounts { get; set; } = true;

        public bool ShowSectors { get; set; } = true;
    }
}
