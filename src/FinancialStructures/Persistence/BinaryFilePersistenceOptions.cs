using System.IO.Abstractions;

namespace Effanville.FinancialStructures.Persistence
{
    public sealed class BinaryFilePersistenceOptions : PersistenceOptions
    {
        public BinaryFilePersistenceOptions(string filePath, IFileSystem fileSystem)            
            : base(filePath, fileSystem)
        {
        }
    }
}