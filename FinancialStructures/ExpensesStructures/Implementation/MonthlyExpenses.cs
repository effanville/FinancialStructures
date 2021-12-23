using System;
using System.Collections.Generic;
using System.Xml;
using Common.Structure.Extensions;
using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures.Implementation
{
    /// <summary>
    /// Expenses associated to a specific month.
    /// </summary>
    internal class MonthlyExpenses : ExpenseCollection, IMonthlyExpenses
    {
        /// <inheritdoc/>
        public DateTime Month
        {
            get;
            private set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        internal MonthlyExpenses(DateTime month, List<Expense> expenses)
            : base(expenses)
        {
            Month = month;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public MonthlyExpenses()
        {
        }

        private const string XmlBaseElement = "MonthlyExpenses";

        /// <inheritdoc/>
        public override void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(XmlBaseElement);
            string monthString = reader.ReadElementContentAsString();
            DateTime month = DateTime.Parse(monthString);
            Month = month;

            base.ReadXml(reader);
        }

        /// <inheritdoc/>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseElement);

            writer.WriteElementString("Month", Month.ToUkDateString());
            base.WriteXml(writer);

            writer.WriteEndElement();
        }
    }
}
