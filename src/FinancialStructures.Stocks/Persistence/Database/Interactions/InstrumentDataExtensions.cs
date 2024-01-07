using System.Linq;

using FinancialStructures.Stocks.Persistence.Models;

using Microsoft.EntityFrameworkCore;

namespace FinancialStructures.Stocks.Persistence.Database.Interactions
{
    public static class InstrumentDataExtensions
    {
        public static InstrumentData GetLatest(this DbSet<InstrumentData> dbSet, int instrumentId)
            => dbSet.AsEnumerable().Where(a => a.InstrumentId == instrumentId).MaxBy(b => b.SnapshotTime);
    }
}