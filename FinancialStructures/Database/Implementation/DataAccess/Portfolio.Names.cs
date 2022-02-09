using System.Collections.Generic;
using System.Linq;

using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public IReadOnlyList<string> Companies(Account elementType)
        {
            return NameDataForAccount(elementType).Select(NameData => NameData.Company).Distinct().ToList();
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Names(Account elementType)
        {
            return NameDataForAccount(elementType).Select(NameData => NameData.Name).ToList();
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Sectors(Account elementType)
        {
            List<string> sectors = NameDataForAccount(elementType).SelectMany(name => name.Sectors).Distinct().ToList();
            sectors.Sort();
            return sectors;
        }

        /// <inheritdoc/>
        public IReadOnlyList<NameData> NameDataForAccount(Account accountType)
        {
            var accounts = Accounts(accountType);
            return accounts.Select(acc => acc.Names.Copy()).ToList();
        }
    }
}
