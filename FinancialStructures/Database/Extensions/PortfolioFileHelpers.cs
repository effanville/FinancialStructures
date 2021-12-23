using System.IO.Abstractions;

namespace FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Helper class for getting directory and file information.
    /// </summary>
    public static class PortfolioFileHelpers
    {
        /// <summary>
        /// The directory where the database is stored.
        /// </summary>
        public static string Directory(this IPortfolio portfolio, IFileSystem fileSystem)
        {
            return string.IsNullOrEmpty(portfolio.FilePath) ? string.Empty : fileSystem.Path.GetDirectoryName(portfolio.FilePath);
        }

        /// <summary>
        /// The file name of the database location (considered the name).
        /// </summary>
        public static string DatabaseName(this IPortfolio portfolio, IFileSystem fileSystem)
        {
            return fileSystem.Path.GetFileNameWithoutExtension(portfolio.FilePath);
        }

        /// <summary>
        /// The file name of the database location (considered the name).
        /// </summary>
        public static string DatabaseName(this IPortfolio portfolio)
        {
            return portfolio.DatabaseName(new FileSystem());
        }
    }
}
