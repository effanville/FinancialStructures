using System.IO.Abstractions;

namespace FinancialStructures.Stocks.Persistence
{
    public class XmlFilePersistenceOptions : PersistenceOptions
    {
        public string FilePath { get; }
        public IFileSystem FileSystem { get; }

        public XmlFilePersistenceOptions(string filePath, IFileSystem fileSystem)
        {
            FilePath = filePath;
            FileSystem = fileSystem;
        }
    }
}