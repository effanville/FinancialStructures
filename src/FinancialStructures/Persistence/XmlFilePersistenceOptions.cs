using System.IO.Abstractions;

namespace FinancialStructures.Persistence
{
    public class XmlFilePersistenceOptions : PersistenceOptions
    {
        public XmlFilePersistenceOptions(string filePath, IFileSystem fileSystem)
            : base(filePath, fileSystem)
        {
        }
    }
}