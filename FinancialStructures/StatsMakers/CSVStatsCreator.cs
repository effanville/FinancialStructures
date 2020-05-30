using System;
using System.Collections.Generic;
using System.IO;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.StatisticStructures;

namespace FinancialStructures.StatsMakers
{
    public static class CSVStatsCreator
    {
        private static void WriteSpacing(StreamWriter writer, bool spacing)
        {
            if (spacing)
            {
                writer.WriteLine("");
            }
        }
        private static void WriteSectorAnalysis(StreamWriter writer, IPortfolio portfolio, System.Reflection.PropertyInfo[] info, UserOptions options, int maxNameLength, int maxCompanyLength, int maxNumLength)
        {
            writer.WriteLine("Analysis By Sector");

            WriteHeader(writer, info, options.SecurityDataToExport, maxNameLength, maxCompanyLength, maxNumLength);

            List<string> sectorNames = portfolio.GetSecuritiesSectors();
            foreach (string sectorName in sectorNames)
            {
                List<SecurityStatistics> valuesToWrite = new List<SecurityStatistics>
                {
                    portfolio.GenerateSectorFundsStatistics(sectorName),
                    portfolio.GenerateBenchMarkStatistics(sectorName)
                };
                int linesWritten = 0;
                foreach (SecurityStatistics value in valuesToWrite)
                {
                    if ((options.DisplayValueFunds && value.LatestVal > 0) || !options.DisplayValueFunds)
                    {
                        string line = string.Empty;
                        foreach (System.Reflection.PropertyInfo props in info)
                        {
                            if (options.SecurityDataToExport.Contains(props.Name))
                            {
                                if (Double.TryParse(props.GetValue(value).ToString(), out double result))
                                {
                                    line += result.ToString() + ",";
                                }
                                else
                                {
                                    if (props.Name == "Name")
                                    {
                                        line += props.GetValue(value).ToString() + ",";
                                    }
                                    else
                                    {
                                        line += props.GetValue(value).ToString() + ",";
                                    }
                                }
                            }
                        }
                        linesWritten++;
                        writer.WriteLine(line);
                    }
                }

                if (linesWritten > 0)
                {
                    WriteSpacing(writer, options.Spacing);
                }
            }
        }

        public static bool CreateCSVPageCustom(IPortfolio portfolio, string filepath, UserOptions options)
        {
            int maxNameLength = Math.Min(25, portfolio.LongestName() + 2);
            int maxCompanyLength = Math.Min(25, portfolio.LongestCompany() + 2);
            int maxNumLength = 10;
            int length = maxNameLength + maxCompanyLength + 8 * maxNumLength;
            StreamWriter htmlWriter = new StreamWriter(filepath);

            htmlWriter.WriteLine("Funds Data");

            SecurityStatistics totals = portfolio.GeneratePortfolioStatistics();
            System.Reflection.PropertyInfo[] properties = totals.GetType().GetProperties();

            WriteHeader(htmlWriter, properties, options.SecurityDataToExport, maxNameLength, maxCompanyLength, maxNumLength);

            List<string> companies = portfolio.Companies(AccountType.Security);
            companies.Sort();
            foreach (string compName in companies)
            {
                List<SecurityStatistics> securities = portfolio.GenerateCompanyFundsStatistics(compName);
                int linesWritten = 0;
                foreach (SecurityStatistics sec in securities)
                {
                    if ((options.DisplayValueFunds && sec.LatestVal > 0) || !options.DisplayValueFunds)
                    {
                        string line = string.Empty;
                        foreach (System.Reflection.PropertyInfo props in properties)
                        {
                            if (options.SecurityDataToExport.Contains(props.Name))
                            {
                                if (Double.TryParse(props.GetValue(sec).ToString(), out double result))
                                {
                                    line += result.ToString() + ",";
                                }
                                else
                                {
                                    if (props.Name == "Name")
                                    {
                                        line += props.GetValue(sec).ToString() + ",";
                                    }
                                    else
                                    {
                                        line += props.GetValue(sec).ToString() + ",";
                                    }
                                }
                            }
                        }

                        htmlWriter.WriteLine(line);
                        linesWritten += 1;
                    }
                }

                if (linesWritten > 0)
                {
                    WriteSpacing(htmlWriter, options.Spacing);
                }
            }

            if ((options.DisplayValueFunds && totals.LatestVal > 0) || !options.DisplayValueFunds)
            {
                htmlWriter.WriteLine("");
                string fundTotalLine = string.Empty;
                foreach (System.Reflection.PropertyInfo props in properties)
                {
                    if (options.SecurityDataToExport.Contains(props.Name))
                    {
                        if (Double.TryParse(props.GetValue(totals).ToString(), out double result))
                        {
                            fundTotalLine += result.ToString() + ",";
                        }
                        else
                        {
                            if (props.Name == "Name")
                            {
                                fundTotalLine += props.GetValue(totals).ToString() + ",";
                            }
                            else
                            {
                                fundTotalLine += props.GetValue(totals).ToString() + ",";
                            }
                        }
                    }
                }

                htmlWriter.WriteLine(fundTotalLine);
                WriteSpacing(htmlWriter, options.Spacing);
            }

            DayValue_Named bankTotals = new DayValue_Named(string.Empty, "Totals,", DateTime.Today, portfolio.TotalValue(AccountType.BankAccount));
            System.Reflection.PropertyInfo[] bankProperties = bankTotals.GetType().GetProperties();

            WriteHeader(htmlWriter, bankProperties, options.BankAccDataToExport, maxNameLength, maxCompanyLength, maxNumLength);

            List<string> BankCompanies = portfolio.Companies(AccountType.BankAccount);
            BankCompanies.Sort();
            foreach (string compName in BankCompanies)
            {
                List<DayValue_Named> bankAccounts = portfolio.GenerateCompanyBankAccountStatistics(compName, options.DisplayValueFunds);
                int linesWritten = 0;
                foreach (DayValue_Named acc in bankAccounts)
                {
                    if ((options.DisplayValueFunds && acc.Value > 0) || !options.DisplayValueFunds)
                    {
                        string line = string.Empty;
                        foreach (System.Reflection.PropertyInfo prop in bankProperties)
                        {
                            if (options.BankAccDataToExport.Contains(prop.Name))
                            {
                                if (Double.TryParse(prop.GetValue(acc).ToString(), out double result))
                                {
                                    line += result.ToString() + ",";
                                }
                                else
                                {
                                    if (prop.PropertyType == typeof(string))
                                    {
                                        if (prop.Name == "Name")
                                        {
                                            line += prop.GetValue(acc).ToString() + ",";
                                        }
                                        else
                                        {
                                            line += prop.GetValue(acc).ToString() + ",";
                                        }
                                    }
                                }
                            }
                        }
                        linesWritten++;
                        htmlWriter.WriteLine(line);
                    }
                }

                if (linesWritten > 0)
                {
                    WriteSpacing(htmlWriter, options.Spacing);
                }
            }


            if ((options.DisplayValueFunds && bankTotals.Value > 0) || !options.DisplayValueFunds)
            {
                string totalAccountsLine = string.Empty;
                foreach (System.Reflection.PropertyInfo prop in bankProperties)
                {
                    if (options.BankAccDataToExport.Contains(prop.Name))
                    {
                        if (Double.TryParse(prop.GetValue(bankTotals).ToString(), out double result))
                        {
                            totalAccountsLine += result.ToString() + ",";
                        }
                        else
                        {
                            if (prop.PropertyType == typeof(string))
                            {
                                if (prop.Name == "Name")
                                {
                                    totalAccountsLine += prop.GetValue(bankTotals).ToString() + ",";
                                }
                                else
                                {
                                    totalAccountsLine += prop.GetValue(bankTotals).ToString() + ",";
                                }
                            }
                        }
                    }
                }

                htmlWriter.WriteLine(totalAccountsLine);
                WriteSpacing(htmlWriter, options.Spacing);
            }

            WriteSpacing(htmlWriter, options.Spacing);

            DayValue_Named portfolioTotals = new DayValue_Named("Portfolio", "Total", DateTime.Today, portfolio.Value(DateTime.Today));
            System.Reflection.PropertyInfo[] portfolioProperties = bankTotals.GetType().GetProperties();

            string totalLine = string.Empty;
            foreach (System.Reflection.PropertyInfo prop in portfolioProperties)
            {
                if (Double.TryParse(prop.GetValue(portfolioTotals).ToString(), out double result))
                {
                    totalLine += result.ToString() + ",";
                }
                else
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        if (prop.Name == "Name")
                        {
                            totalLine += prop.GetValue(portfolioTotals).ToString() + ",";
                        }
                        else
                        {
                            totalLine += prop.GetValue(portfolioTotals).ToString() + ",";
                        }
                    }
                }
            }

            htmlWriter.WriteLine(totalLine);

            WriteSpacing(htmlWriter, options.Spacing);

            WriteSectorAnalysis(htmlWriter, portfolio, properties, options, maxNameLength, maxCompanyLength, maxNumLength);
            htmlWriter.Close();
            return true;
        }

        private static void WriteHeader(StreamWriter writer, System.Reflection.PropertyInfo[] info, List<string> names, int maxNameLength, int maxCompanyLength, int maxNumLength)
        {
            string header = string.Empty;

            foreach (System.Reflection.PropertyInfo props in info)
            {
                if (names.Contains(props.Name))
                {
                    if (props.PropertyType == typeof(string))
                    {
                        if (props.Name == "Name")
                        {
                            header += props.Name + ",";
                        }
                        else
                        {
                            header += props.Name + ",";
                        }

                    }

                    if (props.PropertyType == typeof(double))
                    {
                        header += props.Name + ",";
                    }
                }
            }

            writer.WriteLine(header);
            writer.WriteLine("");
        }
    }
}
