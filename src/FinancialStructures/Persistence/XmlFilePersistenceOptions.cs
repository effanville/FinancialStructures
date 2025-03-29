using System.IO.Abstractions;

namespace Effanville.FinancialStructures.Persistence
{
    public class XmlFilePersistenceOptions : PersistenceOptions
    {
        public XmlFilePersistenceOptions(string filePath, IFileSystem fileSystem, string version)
            : base(filePath, fileSystem, version)
        {
        }
    }
}