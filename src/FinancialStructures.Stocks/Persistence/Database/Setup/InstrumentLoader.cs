using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

using Common.Structure.Reporting;
using Common.Structure.WebAccess;

using FinancialStructures.Stocks.Persistence.Models;

namespace FinancialStructures.Stocks.Persistence.Database.Setup
{
    internal static class InstrumentLoader
    {
        /// <summary>
        /// Get instruments from an index
        /// </summary>
        /// <param name="indexName">FTSE-100,FTSE-250,FTSE-350 etc</param>
        public static string[] GetIndexInstrumentsFromFTSE(string indexName)
        {
            string ftseWebsite = $"https://www.londonstockexchange.com/indices/{indexName}/constituents/table";
            IReportLogger logger = new LogReporter(null, true);
            // driver initialization varies across different drivers
            // but they all support parameter-less constructors

            var driver = WebDownloader.GetCachedInstance(forceNew: true);
            StringBuilder sb = new StringBuilder();
            int index = 1;
            while (index < 10)
            {
                string url = ftseWebsite + $"?page={index}";
                string text = null;
                int numberTries = 0;
                while (string.IsNullOrEmpty(text) && numberTries < 20)
                {
                    text = WebDownloader.GetElementText(driver, url, "ftse-index-table", 1000 + 1000 * numberTries, logger);
                    numberTries++;
                }
                if (text != null)
                {
                    if (text == "What's this?\r\n12345")
                    {
                        break;
                    }
                    text = text.Substring(text.IndexOf("Change %") + "Change %".Length);
                    text = text.Replace("12345", "").Trim('\r').Trim('\n');
                    _ = sb.Append(text)
                        .AppendLine();
                }

                index++;
            }

            return sb.ToString().Split("\r\n");
        }

        public static string[] GetInstrumentsFromFile(string stockFilePath, IFileSystem fileSystem, IReportLogger logger = null)
        {
            string[] fileContents = Array.Empty<string>();
            try
            {
                fileContents = fileSystem.File.ReadAllLines(stockFilePath);
            }
            catch (Exception ex)
            {
                logger?.Error("FileRead", $"Failed to read from file located at {stockFilePath}: {ex.Message}.");
            }

            if (fileContents.Length == 0)
            {
                logger?.Error("FileRead", "Nothing in file selected, but expected stock company, name, url data.");
                return null;
            }

            return fileContents;
        }

        public static int ConfigureInstruments(StockExchangeDbContext context, string stockFilePath, IFileSystem fileSystem, IReportLogger logger = null)
        {
            string[] fileContents = GetInstrumentsFromFile(stockFilePath, fileSystem, logger);
            int numberChanges = ConfigureInstruments(context, fileContents, out _, out _, out _, logger);
            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Configured StockExchange from file {stockFilePath}.");
            return numberChanges;
        }

        public static int ConfigureInstruments(
            StockExchangeDbContext context,
            string[] instrumentData,
            out List<string> newInstrumentData,
            out List<string> existingInstrumentData,
            out List<Instrument> removedInstruments,
            IReportLogger logger = null)
        {
            DataSource dataSource = context.DataSources.SingleOrDefault(ex => ex.Name == "Yahoo");
            var existingInstruments = context.Instruments;
            var addedInstruments = new List<Instrument>();
            existingInstrumentData = new List<string>();
            newInstrumentData = new List<string>();
            removedInstruments = new List<Instrument>(context.Instruments);
            foreach (string line in instrumentData)
            {
                string[] inputs = line.Split(',');
                if (inputs.Length == 7)
                {
                    Exchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == inputs[4]);
                    var inst = existingInstruments.FirstOrDefault(i => i.Name == inputs[2] && i.Ric == inputs[0]);
                    if (inst != null)
                    {
                        existingInstrumentData.Add(line);
                        _ = removedInstruments.Remove(inst);
                        continue;
                    }
                    newInstrumentData.Add(line);
                    addedInstruments.Add(new Instrument()
                    {
                        Ric = inputs[0],
                        Company = inputs[1],
                        Name = inputs[2],
                        Currency = inputs[3],
                        ExchangeId = exchange.Id,
                        Url = inputs[5],
                        Sectors = inputs[6]
                    });
                }
                else
                {
                    string[] tabSepInputs = line.Split("\t");
                    if (tabSepInputs.Length == 7)
                    {
                        string ric = $"{tabSepInputs[0].Trim('.')}.L";
                        string name = tabSepInputs[0].Replace(".L", "");
                        var inst = existingInstruments.FirstOrDefault(i => i.Name == name && i.Ric == ric);
                        if (inst != null)
                        {
                            existingInstrumentData.Add(line);
                            _ = removedInstruments.Remove(inst);
                            continue;
                        }
                        newInstrumentData.Add(line);
                        Exchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");
                        addedInstruments.Add(new Instrument()
                        {
                            Ric = ric,
                            Company = tabSepInputs[1],
                            Name = name,
                            Currency = tabSepInputs[2],
                            ExchangeId = exchange.Id,
                            Url = $"{dataSource.BaseUrl}{ric}",
                            Sectors = ""
                        });
                    }
                    else
                    {
                        string[] spaceSepInputs = line.Split(" ");
                        if (spaceSepInputs.Length > 7)
                        {
                            string ric = $"{spaceSepInputs[0].Trim('.').Replace('.', '-')}.L";
                            string currency = spaceSepInputs[^5];
                            string[] nameArray = spaceSepInputs.Skip(1).SkipLast(5).ToArray();
                            string name = string.Join(" ", nameArray);
                            var inst = existingInstruments.FirstOrDefault(i => i.Name == name && i.Ric == ric);
                            if (inst != null)
                            {
                                existingInstrumentData.Add(line);
                                _ = removedInstruments.Remove(inst);
                                continue;
                            }
                            newInstrumentData.Add(line);
                            Exchange exchange = context.Exchanges.Single(ex => ex.ExchangeIdentifier == "LSE");
                            addedInstruments.Add(new Instrument()
                            {
                                Ric = ric,
                                Company = spaceSepInputs[0],
                                Name = name,
                                Currency = spaceSepInputs[^5],
                                ExchangeId = exchange.Id,
                                Url = $"{dataSource.BaseUrl}{ric}",
                                Sectors = ""
                            });
                        }
                    }
                }
            }

            context.Instruments.AddRange(addedInstruments);
            int numberChanges = context.SaveChanges();

            logger?.Log(ReportType.Information, ReportLocation.AddingData.ToString(), $"Added {numberChanges} into database.");
            return numberChanges;
        }
    }
}