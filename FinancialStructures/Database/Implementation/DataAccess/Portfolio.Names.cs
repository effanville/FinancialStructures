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
            var companies = NameDataForAccount(elementType).Select(NameData => NameData.Company).Distinct().ToList();
            companies.Sort();
            return companies;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Names(Account elementType)
        {
            var names = NameDataForAccount(elementType).Select(NameData => NameData.Name).ToList();
            names.Sort();
            return names;
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
