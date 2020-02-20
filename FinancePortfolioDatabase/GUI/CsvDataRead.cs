using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;
using System.IO;

namespace GUISupport
{
    public enum ElementType
    {
        Security,
        Sector,
        BankAccount,
        CurrencyExchange
    }

    public static class CsvDataRead
    {
        public static List<object> ReadFromCsv(string filePath, ElementType type, ErrorReports reports)
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
                    case ElementType.Security:
                        return CreateSecurityData(valuationsToRead, reports);
                    case ElementType.BankAccount:
                    case ElementType.CurrencyExchange:
                    case ElementType.Sector:
                        return CreateAccountData(valuationsToRead, reports);
                    default:
                        return new List<object>();
                }
            }
            catch (Exception ex)
            {
                reports.AddError(ex.Message, Location.Loading);
                return new List<object>();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

        }

        public static List<object> CreateSecurityData(List<string[]> valuationsToRead, ErrorReports reports)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 4)
                {
                    reports.AddError("Line in Csv file has incomplete data.", Location.Loading);
                    break;
                }

                var line = new DayDataView(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]), double.Parse(dayValuation[2]), double.Parse(dayValuation[3]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }

        public static List<object> CreateAccountData(List<string[]> valuationsToRead, ErrorReports reports)
        {
            var dailyValuations = new List<object>();
            foreach (var dayValuation in valuationsToRead)
            {
                if (dayValuation.Length != 2)
                {
                    reports.AddError("Line in Csv file has incomplete data.", Location.Loading);
                    break;
                }

                var line = new DayValue_ChangeLogged(DateTime.Parse(dayValuation[0]), double.Parse(dayValuation[1]));
                dailyValuations.Add(line);
            }

            return dailyValuations;
        }
    }
}
