using System.IO.Abstractions;

namespace Effanville.FinancialStructures.Persistence
{
    public abstract class PersistenceOptions
    {
        public string FilePath { get; }
        public IFileSystem FileSystem { get; }
        public string Version { get; }

        protected PersistenceOptions(string filePath, IFileSystem fileSystem, string version)
        {
            FilePath = filePath;
            FileSystem = fileSystem;
            Version = version;
        }
    }
}
