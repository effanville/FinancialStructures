using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Effanville.Common.Console.DependencyInjection;
using Effanville.Common.Structure.WebAccess;
using Effanville.FinancialStructures.Stocks.HistoricalRepository;
using Effanville.FinancialStructures.Stocks.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Effanville.FinancialStructures.Stocks.Cli;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddScoped<WebDownloader>()
            .AddScoped<HistoricalMarketsBuilder>()
            .AddScoped<IHistoricalMarketsPersistence, SqliteHistoricalMarketsPersistence>();
        IHost host = builder.SetupConsole(
                args,
                new List<Type>()
                {
                        typeof(CreateDatabaseCommand),
                        typeof(UpdateDatabaseCommand)
                })
            .Build();
        await host.RunAsync();
    }
}
