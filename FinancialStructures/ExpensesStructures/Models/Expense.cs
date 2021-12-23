using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Common.Structure.Extensions;
using FinancialStructures.ExpensesStructures.Helpers;

namespace FinancialStructures.ExpensesStructures.Models
{
    /// <summary>
    /// Contains information about a specific expense,
    /// including when, how much, and what sort of expense.
    /// </summary>
    public sealed class Expense : IXmlSerializable
    {
        /// <summary>
        /// The date this expense refers to
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }

        /// <summary>
        /// The amount of this expense.
        /// Negative amounts are outgoings.
        /// Positive amounts are incomings.
        /// </summary>
        public double Amount
        {
            get;
            private set;
        }

        /// <summary>
        /// The <see cref="ExpenseCategory"/> this expense is pertaining to.
        /// </summary>
        public ExpenseCategory Category
        {
            get;
            private set;
        }

        /// <summary>
        /// Any ancillary notes pertaining to this expense.
        /// </summary>
        public string Notes
        {
            get;
            private set;
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        internal Expense(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            Date = date;
            Amount = amount;
            Notes = notes;
            Category = category;
        }

        /// <summary>
        /// Default constrcutor.
        /// </summary>
        public Expense()
        {
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        private const string XmlBaseElement = "Expense";

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(XmlBaseElement);
            string values = reader.ReadContentAsString();
            string[] valuesArray = values.Split('-');
            if (valuesArray.Length != 4)
            {
                throw new XmlException($"{nameof(Expense)} should have 4 parts but found only {valuesArray.Length} - input was {values}");
            }

            Date = DateTime.Parse(valuesArray[0]);
            if (Enum.TryParse<ExpenseCategory>(valuesArray[1], out var result))
            {
                Category = result;
            }
            else
            {
                throw new XmlException("It didnt work.");
            }
            Amount = double.Parse(valuesArray[2]);
            Notes = valuesArray[3];

            reader.ReadEndElement();
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(XmlBaseElement);
            writer.WriteString(ToString());
            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Date.ToUkDateString()}-{Category.ToString().PadRight(EnumHelper.LongestEntry<ExpenseCategory>())}-{Amount}-{Notes}";
        }
    }
}
