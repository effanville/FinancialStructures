using System.IO.Abstractions;

namespace FinancialStructures.Persistence
{
    public class SqlitePersistenceOptions : PersistenceOptions
    {
        public bool InMemory { get; }
        public string FilePath { get; }
        public IFileSystem FileSystem { get; }

        public SqlitePersistenceOptions(bool inMemory, string filePath, IFileSystem fileSystem)
        {
            FilePath = filePath;
            FileSystem = fileSystem;
            InMemory = inMemory;
        }
    }
}