using Effanville.Common.Structure.Reporting;

namespace FinancialStructures.Persistence
{
    public interface IPersistence<T> where T : class
    {
        /// <summary>
        /// Loads the <see cref="T"/> from the file specified.
        /// </summary>
        T Load(PersistenceOptions options, IReportLogger reportLogger = null);

        /// <summary>
        /// Loads the <see cref="T"/> from the file specified and fills out parameter <paramref name="portfolio"/>
        /// </summary>
        bool Load(T portfolio, PersistenceOptions options, IReportLogger reportLogger = null);
        
        /// <summary>
        /// Saves the <see cref="T"/> to the file specified.
        /// </summary>
        bool Save(T portfolio, PersistenceOptions options, IReportLogger reportLogger = null);
    }
}