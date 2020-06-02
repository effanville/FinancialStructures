using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StructureCommon.FileAccess
{
    /// <summary>
    /// Class containing default routines to write to any file type in <see cref="ExportType"/>.
    /// </summary>
    public static class FileWritingSupport
    {
        /// <summary>
        /// Writes a paragraph to the file.
        /// </summary>
        /// <param name="writer">The writer to use</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="sentence">The sentence to export.</param>
        /// <param name="tag">The <see cref="HtmlTag"/> to use.</param>
        public static void WriteParagraph(this StreamWriter writer, ExportType exportType, string[] sentence, HtmlTag tag = HtmlTag.p)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    string toOutput = string.Join(",", sentence);
                    writer.WriteLine(toOutput);
                    break;
                }
                case (ExportType.Html):
                {
                    string toOutput = string.Join(" ", sentence);
                    writer.WriteLine($"<{tag}>{toOutput}</{tag}>");
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes a title line to the file.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="title">The title string to write.</param>
        /// <param name="tag">The specific <see cref="HtmlTag"/> to use in this title.</param>
        public static void WriteTitle(this StreamWriter writer, ExportType exportType, string title, HtmlTag tag = HtmlTag.h1)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    writer.WriteLine("");
                    writer.WriteLine(title);
                    writer.WriteLine("");
                    break;
                }
                case (ExportType.Html):
                {
                    writer.WriteLine($"<{tag}>{title}</{tag}>");
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes an enumerable to a table, where the columns are all the properties of the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to write out.</typeparam>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="exportType">The type of file to export to.</param>
        /// <param name="values">The values to write into the table.</param>
        public static void WriteTable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<T> values)
        {
            T forTypes = default(T);
            foreach (T value in values)
            {
                if (value != null)
                {
                    forTypes = value;
                    break;
                }
            }

            if (forTypes == null)
            {
                return;
            }

            writer.WriteTable(exportType, forTypes.GetType().GetProperties().Select(type => type.Name), values);
        }

        /// <summary>
        /// Writes a table to the file, given header values which are a subset of the property names of the type <typeparamref name="T"/>.
        /// <para> If the value in rowValues is null, then this writes a break line into the table.</para>
        /// </summary>
        /// <typeparam name="T">The object to write the properties out of.</typeparam>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="exportType">The type of file to write to.</param>
        /// <param name="headerValues">The values to write out.</param>
        /// <param name="rowValues">The values to place in successive rows.</param>
        public static void WriteTable<T>(this StreamWriter writer, ExportType exportType, IEnumerable<string> headerValues, IEnumerable<T> rowValues)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    WriteTableHeader(writer, exportType, headerValues);
                    foreach (T value in rowValues)
                    {
                        PropertyInfo[] row = value.GetType().GetProperties();
                        WriteTableRow(writer, exportType, row.Where(info => headerValues.Contains(info.Name)).Select(ro => ro.GetValue(value)?.ToString()));
                    }
                    break;
                }
                case (ExportType.Html):
                {
                    writer.WriteLine("<table>");
                    writer.WriteLine("<thead><tr>");
                    WriteTableHeader(writer, exportType, headerValues);
                    writer.WriteLine("</tr></thead>");
                    writer.WriteLine("<tbody>");

                    foreach (T value in rowValues)
                    {
                        if (value != null)
                        {
                            PropertyInfo[] row = value.GetType().GetProperties();
                            writer.WriteLine("<tr>");
                            WriteTableRow(writer, exportType, row.Where(info => headerValues.Contains(info.Name)).Select(ro => ro.GetValue(value)?.ToString()));
                            writer.WriteLine("</tr>");
                        }
                        if (value == null)
                        {
                            writer.WriteLine("<tr><td><br/></td></tr>");
                        }
                    }

                    writer.WriteLine("</tbody>");
                    writer.WriteLine("</table>");
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the header of a table
        /// </summary>
        /// <param name="writer">The StreamWriter to use</param>
        /// <param name="exportType">The type of file to export to</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        public static void WriteTableHeader(this StreamWriter writer, ExportType exportType, IEnumerable<string> valuesToWrite)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    string toWrite = string.Join(",", valuesToWrite);
                    writer.WriteLine(toWrite);
                    break;
                }
                case (ExportType.Html):
                {
                    string htmlHeader = "<th scope=\"col\">";
                    int i = 0;
                    foreach (string property in valuesToWrite)
                    {
                        if (i != 0)
                        {
                            htmlHeader += "<th>";
                        }

                        htmlHeader += property;
                        htmlHeader += "</th>";
                        i++;
                    }

                    writer.WriteLine(htmlHeader);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes a row of a table from an enumerable of string values.
        /// </summary>
        /// <param name="writer">The StreamWriter to use</param>
        /// <param name="exportType">The type of file to export to</param>
        /// <param name="valuesToWrite">The values to use for the header names.</param>
        public static void WriteTableRow(this StreamWriter writer, ExportType exportType, IEnumerable<string> valuesToWrite)
        {
            switch (exportType)
            {
                case (ExportType.Csv):
                {
                    string toWrite = string.Join(",", valuesToWrite);
                    writer.WriteLine(toWrite);
                    break;
                }
                case (ExportType.Html):
                {
                    string htmlData = "<th scope=\"row\">";
                    // string htmlData = string.Empty;
                    int i = 0;
                    foreach (string property in valuesToWrite)
                    {
                        bool isDouble = double.TryParse(property, out double value);
                        //htmlData += "<td>";
                        if (i != 0)
                        {
                            if (value < 0)
                            {
                                htmlData += "<td data-negative>";
                            }
                            else
                            {
                                htmlData += "<td>";
                            }
                        }
                        htmlData += property;
                        if (i == 0)
                        {
                            htmlData += "</th>";
                        }
                        else
                        {
                            htmlData += "</td>";
                        }
                        i++;
                        htmlData += "</td>";
                    }

                    writer.WriteLine(htmlData);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Creates a generic header with default styles for a html page.
        /// </summary>
        /// <param name="writer">The writer to write the page with</param>
        /// <param name="title">A title to give the page</param>
        /// <param name="useColours">Whether to use colour styling or not.</param>
        public static void CreateHTMLHeader(this StreamWriter writer, string title, bool useColours)
        {
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine($"<title>{title}</title>");
            writer.WriteLine("<style>");
            writer.WriteLine("html, h1, h2, h3, h4, h5, h6 {font-family: \"Arial\", cursive, sans-serif; }");
            writer.WriteLine("h1 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("h2 { font-family: \"Arial\", cursive, sans-serif; margin-top: 1.5em; }");
            writer.WriteLine("body{ font-family: \"Arial\", cursive, sans-serif; font-size: 10px }");
            writer.WriteLine("table { border-collapse: collapse;}");
            writer.WriteLine("table, th, td { border: 1px solid black; }");
            writer.WriteLine("caption { margin-bottom: 1.2em; font-family: \"Arial\", cursive, sans-serif; font-size:medium; }");
            writer.WriteLine("tr {text-align: center;}");

            if (useColours)
            {
                writer.WriteLine("tr:nth-child(even) {background-color: #f0f8ff;}");
                writer.WriteLine("th{ background-color: #ADD8E6; height: 1.5em; }");
                writer.WriteLine("[data-negative] { background-color: red;}");
            }
            else
            {
                writer.WriteLine("th{ height: 1.5em; }");
            }

            writer.WriteLine(" p { line-height: 1.5em; margin-bottom: 1.5em;}");
            writer.WriteLine("</style> ");

            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
        }

        /// <summary>
        /// Creates a generic footer for a html page.
        /// </summary>
        /// <param name="writer">The writer to write the page with</param>
        public static void CreateHTMLFooter(this StreamWriter writer)
        {
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }
    }
}
