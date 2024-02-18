using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures.Implementation
{
    internal class ExpenseHistory : IExpenseHistory, IXmlSerializable
    {
        /// <inheritdoc/>
        public IExpenseCollection PredictedExpenses
        {
            get;
        } = new ExpenseCollection();

        /// <inheritdoc/>
        public Dictionary<DateTime, IMonthlyExpenses> ActualExpensesByMonth
        {
            get;
        } = new Dictionary<DateTime, IMonthlyExpenses>();

        public ExpenseHistory()
        {
        }

        internal ExpenseHistory(List<Expense> predictedExpenses, Dictionary<DateTime, IMonthlyExpenses> actualExpensesByMonth)
        {
            PredictedExpenses = new ExpenseCollection(predictedExpenses);
            ActualExpensesByMonth = actualExpensesByMonth;
        }

        /// <inheritdoc/>
        public void AddPredictedExpense(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            var expense = new Expense(date, amount, category, notes);
            PredictedExpenses.Add(expense);
        }

        /// <inheritdoc/>
        public void AddExpense(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            var monthDate = new DateTime(date.Year, date.Month, 1);
            if (ActualExpensesByMonth.TryGetValue(monthDate, out var monthlyExpenses))
            {
                monthlyExpenses.Add(date, amount, category, notes);
            }
            else
            {
                var expense = new Expense(date, amount, category, notes);
                ActualExpensesByMonth.Add(monthDate, new MonthlyExpenses(monthDate, new List<Expense>() { expense }));
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        private const string XmlBaseElement = "ExpenseHistory";
        private const string XmlPredictedElement = "Predicted";
        private const string XmlHistoryElement = "HistoryByMonth";

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(XmlBaseElement);
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement(XmlPredictedElement);
            if (!isEmpty)
            {
                PredictedExpenses.ReadXml(reader);
                reader.ReadEndElement();
            }

            isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement(XmlHistoryElement);

            if (!isEmpty)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    var monthlyExpenses = new MonthlyExpenses();
                    monthlyExpenses.ReadXml(reader);
                    ActualExpensesByMonth.Add(monthlyExpenses.Month, monthlyExpenses);
                }

                reader.ReadEndElement();
            }


            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseElement);

            writer.WriteStartElement(XmlPredictedElement);
            PredictedExpenses.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement(XmlHistoryElement);

            foreach (var pair in ActualExpensesByMonth)
            {
                pair.Value.WriteXml(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public void WritePredictedExpenses(string filepath, out string error)
        {
            WritePredictedExpenses(new FileSystem(), filepath, out error);
        }

        /// <inheritdoc/>
        public void WritePredictedExpenses(IFileSystem fileSystem, string filepath, out string error)
        {
            PredictedExpenses.WriteExpenses(fileSystem, filepath, "Predicted Expenses", out error);
        }

        /// <inheritdoc/>
        public void WriteExpenses(string filepath, DateTime month, out string error)
        {
            WriteExpenses(new FileSystem(), filepath, month, out error);
        }
        /// <inheritdoc/>
        public void WriteExpenses(IFileSystem fileSystem, string filepath, DateTime month, out string error)
        {
            ActualExpensesByMonth[month].WriteExpenses(fileSystem, filepath, $"Month Expenses for {month.Month}-{month.Year}", out error);
        }
    }
}
