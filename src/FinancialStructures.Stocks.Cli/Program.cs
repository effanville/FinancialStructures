using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Common.Console;
using Common.Console.Commands;
using Common.Structure.Extensions;
using Common.Structure.Reporting;

namespace FinancialStructures.Stocks.Cli
{
    internal static class Program
    {
        internal static int Main(string[] args)
        {
            var fileSystem = new FileSystem();

            IConsole console = new ConsoleInstance(WriteError, WriteLine);

            // Create the logger.
            IReportLogger logger = new LogReporter(ReportAction, saveInternally: true);
            return InternalMain(args, fileSystem, console, logger);

            // Create the Console to write output.
            void WriteLine(string text) => Console.WriteLine(text);

            void WriteError(string text)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-ddThh:mm:ss}]: {text}");
                Console.ForegroundColor = color;
            }

            void ReportAction(ReportSeverity severity, ReportType reportType, string location, string text)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = reportType == ReportType.Error
                    ? ConsoleColor.Red
                    : reportType == ReportType.Warning
                        ? ConsoleColor.Yellow
                        : color;
                console.WriteLine($"[{DateTime.Now:yyyy-MM-ddThh:mm:ss}]: [{reportType}]: {text}");

                Console.ForegroundColor = color;
            }
        }

        private static int InternalMain(string[] args, IFileSystem fileSystem, IConsole console, IReportLogger logger)
        {
            var globals = new ConsoleGlobals(fileSystem, console, logger);
            // Define the acceptable commands for this program.
            var validCommands = new List<ICommand>()
            {
                new CreateDatabaseCommand(fileSystem),
                new UpdateDatabaseCommand(fileSystem),
            };

            // Generate the context, validate the arguments and execute.
            int exitCode = ConsoleContext.SetAndExecute(args, globals, validCommands);
            string logPath = fileSystem.Path.Combine(fileSystem.Directory.GetCurrentDirectory(), $"{DateTime.Now.FileSuitableDateTimeValue()}-db-update.log");
            logger.WriteReportsToFile(logPath);
            return exitCode;
        }
    }
}