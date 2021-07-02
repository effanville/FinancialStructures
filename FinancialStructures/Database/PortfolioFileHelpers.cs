using System.IO.Abstractions;

namespace FinancialStructures.Database
{
    public static class PortfolioFileHelpers
    {
        /// <summary>
        /// The directory where the database is stored.
        /// </summary>
        public static string Directory(this IPortfolio portfolio, IFileSystem fileSystem)
        {
            return string.IsNullOrEmpty(portfolio.FilePath) ? string.Empty : fileSystem.Path.GetDirectoryName(portfolio.FilePath);
        }

        /// <inheritdoc/>
        public static string DatabaseName(this IPortfolio portfolio, IFileSystem fileSystem)
        {
            return fileSystem.Path.GetFileNameWithoutExtension(portfolio.FilePath);
        }
    }
}
