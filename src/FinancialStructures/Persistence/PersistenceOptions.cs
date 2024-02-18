using System.IO.Abstractions;

namespace FinancialStructures.Persistence
{
    public abstract class PersistenceOptions
    {
        public string FilePath { get; }
        public IFileSystem FileSystem { get; }
        
        protected PersistenceOptions(string filePath, IFileSystem fileSystem)
        {
            FilePath = filePath;
            FileSystem = fileSystem;
        }
    }
}