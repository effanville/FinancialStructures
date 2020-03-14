using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.PortfolioAPI;
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
        public static List<object> ReadFromCsv(string filePath, PortfolioElementType type, Action<string, string, string> reportLogger)
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
                    case PortfolioElementType.Security:
                        return CreateSecurityData(valuationsToRead, reportLogger);
                    case PortfolioElementType.BankAccount:
                    case PortfolioElementType.Currency:
                    case PortfolioElementType.Sector:
                        return CreateAccountData(valuationsToRead, reportLogger);
                    default:
                        return new List<object>();
                }
            }
            catch (Exception ex)
            {
                reportLogger("Error", "Loading", ex.Message);
                return new List<object>();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private static List<object> CreateSecurityData(List<string[]> valuationsToRead, Action<string, string, string> reportLogger)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    reportLogger("Error", "Loading", "Line in Csv file has incomplete data.");
                    break;
                }

                var line = new DayDataView(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]), double.Parse(dayValuation[2]), double.Parse(dayValuation[3]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        private static List<object> CreateAccountData(List<string[]> valuationsToRead, Action<string, string, string> reportLogger)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 2)
                {
                    reportLogger("Error", "Loading", "Line in Csv file has incomplete data.");
                    break;
                }

                var line = new DayValue_ChangeLogged(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }
    }
}
