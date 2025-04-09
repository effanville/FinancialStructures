using System.IO.Abstractions;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class SqlitePersistenceOptions : PersistenceOptions
    {
        public bool InMemory { get; }

        public SqlitePersistenceOptions(string filePath, IFileSystem fileSystem, string version)
            : this(inMemory: false, filePath, fileSystem, version)
        {
        }

        public SqlitePersistenceOptions(bool inMemory, string filePath, IFileSystem fileSystem, string version)
            : base(filePath, fileSystem, version)
            => InMemory = inMemory;
    }
}