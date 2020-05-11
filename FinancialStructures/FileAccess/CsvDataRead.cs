using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinancialStructures.DataReader
{
    /// <summary>
    /// Contains routines to extract data from csv files.
    /// </summary>
    public static class CsvDataRead
    {
        /// <summary>
        /// Reads data from a csv file.
        /// </summary>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="type">The type of data to import.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        /// <returns></returns>
        public static List<object> ReadFromCsv(string filePath, AccountType type, IReportLogger reportLogger = null)
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

                switch (type)
                {
                    case AccountType.Security:
                        return CreateSecurityData(valuationsToRead, reportLogger);
                    case AccountType.BankAccount:
                    case AccountType.Currency:
                    case AccountType.Sector:
                        return CreateAccountData(valuationsToRead, reportLogger);
                    default:
                        return new List<object>();
                }
            }
            catch (Exception ex)
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, ex.Message);
                return new List<object>();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private static List<object> CreateSecurityData(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                var line = new SecurityDayData(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]), double.Parse(dayValuation[2]), double.Parse(dayValuation[3]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        private static List<object> CreateAccountData(List<string[]> valuationsToRead, IReportLogger reportLogger = null)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 2)
                {
                    reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Line in Csv file has incomplete data.");
                    break;
                }

                var line = new DayValue_ChangeLogged(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        /// <summary>
        /// Exports data to a file in csv format.
        /// </summary>
        /// <param name="filePath">The path of file to read from.</param>
        /// <param name="type">The type of data to import.</param>
        /// <param name="accountToExport">The account of which the data is exported.</param>
        /// <param name="reportLogger">Reporting Callback.</param>
        public static void WriteToCSVFile(string filePath, AccountType type, object accountToExport, IReportLogger reportLogger = null)
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath);

                switch (type)
                {
                    case AccountType.Security:
                        WriteSecurityData(writer, (ISecurity)accountToExport, reportLogger);
                        break;
                    case AccountType.BankAccount:
                    case AccountType.Currency:
                    case AccountType.Sector:
                        WriteAccountData(writer, (ISingleValueDataList)accountToExport, reportLogger);
                        break;
                    default:
                        reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, "Account Type wasn't a known type.");
                        break;
                }
            }
            catch (Exception ex)
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Loading, ex.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private static void WriteSecurityData(TextWriter writer, ISecurity accountToExport, IReportLogger reportLogger)
        {
            foreach (SecurityDayData value in accountToExport.GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }

        private static void WriteAccountData(TextWriter writer, ISingleValueDataList accountToExport, IReportLogger reportLogger)
        {
            foreach (DayValue_ChangeLogged value in accountToExport.GetDataForDisplay())
            {
                writer.WriteLine(value.ToString());
            }
        }
    }
}
