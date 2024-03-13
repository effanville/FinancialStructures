using System.IO.Abstractions;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class SqlitePersistenceOptions : PersistenceOptions
    {
        public bool InMemory { get; }

        public SqlitePersistenceOptions(string filePath, IFileSystem fileSystem)           
            : this(inMemory: false, filePath, fileSystem)
        {
        }
        
        public SqlitePersistenceOptions(bool inMemory, string filePath, IFileSystem fileSystem)           
            : base(filePath, fileSystem)
        {
            InMemory = inMemory;
        }
    }
}