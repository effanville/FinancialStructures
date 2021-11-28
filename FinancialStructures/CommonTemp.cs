using System;
using System.IO;
using System.IO.Abstractions;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Structure.FileAccess
{
    internal class CommonTemp
    {
        public static void WriteToXmlFile<T>(IFileSystem fileSystem, string filePath, T objectToWrite, out string error, bool append = false) where T : new()
        {
            error = null;
            try
            {
                var xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true
                };
                using (Stream stream = fileSystem.FileStream.Create(filePath, append ? FileMode.Append : FileMode.Create))
                using (XmlWriter writer = XmlWriter.Create(new StreamWriter(stream), xmlWriterSettings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, objectToWrite);
                }
            }
            catch (Exception ex)
            {
                error = $"{ex.Message}-InnerException {ex.InnerException}";
                return;
            }
        }
    }
}
