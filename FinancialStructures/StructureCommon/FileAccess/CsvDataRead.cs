using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.IO;

namespace StructureCommon.FileAccess
{

    /// <summary>
    /// Contains routines to extract data from csv files.
    /// </summary>
    public static class CsvReaderWriter
    {
        /// <summary>
        /// Reads data from a csv file.
        /// </summary>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static List<object> ReadFromCsv<T>(T dataGainer, string filePath, IReportLogger reportLogger = null) where T : ICSVAccess
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);

                string line = null;
                var valuationsToRead = new List<string[]>();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    valuationsToRead.Add(words);
                }

                return dataGainer.CreateDataFromCsv(valuationsToRead, reportLogger);
            }
            catch (Exception ex)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, ex.Message);
                return new List<object>();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Exports data to a file in csv format.
        /// </summary>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static void WriteToCSVFile<T>(T dataTypeToWrite, string filePath, IReportLogger reportLogger = null) where T : ICSVAccess
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath);
                dataTypeToWrite.WriteDataToCsv(writer, reportLogger);
            }
            catch (Exception ex)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
