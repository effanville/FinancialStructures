using System;
using System.Collections.Generic;
using FinancialStructures.NamingStructures;
using FinancialStructures.StatsMakers;
using StructureCommon.DataStructures;
using StructureCommon.Extensions;

namespace FinancialStructures.DataStructures
{
    public class DayValue_Named : DailyValuation
    {
        public override int CompareTo(object obj)
        {
            if (obj is DayValue_Named value)
            {
                return Names.CompareTo(value.Names);
            }

            return 0;
        }

        public string HTMLTableHeader(UserOptions options, List<string> names)
        {
            System.Reflection.PropertyInfo[] properties = GetType().GetProperties();
            string htmlHeader = string.Empty;
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (names.Contains(property.Name))
                {
                    htmlHeader += "<th scope=\"col\">";
                    htmlHeader += property.Name;
                    htmlHeader += "</th>";
                }
            }

            return htmlHeader;
        }

        public string HTMLTableData(UserOptions options, List<string> names)
        {
            System.Reflection.PropertyInfo[] properties = GetType().GetProperties();
            string htmlData = "<th scope=\"row\">";

            for (int i = 0; i < properties.Length; i++)
            {
                if (names.Contains(properties[i].Name))
                {
                    bool isDouble = double.TryParse(properties[i].GetValue(this).ToString(), out double value);
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
                    if (isDouble)
                    {

                        htmlData += value.TruncateToString();
                    }
                    else
                    {
                        htmlData += properties[i].GetValue(this);
                    }

                    htmlData += "</td>";
                }
            }
            htmlData += "</th>";
            return htmlData;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Names.ToString() + "-" + base.ToString();
        }

        /// <summary>
        /// Names associated to the values.
        /// </summary>
        public TwoName Names
        {
            get; set;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public DayValue_Named() : base()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DayValue_Named(string company, string name, DateTime day, double value)
            : base(day, value)
        {
            Names = new TwoName(company, name);
        }

        /// <summary>
        /// Constructor to create an instance from a base class instance.
        /// </summary>
        public DayValue_Named(string company, string name, DailyValuation toAddOnto)
            : this(company, name, toAddOnto.Day, toAddOnto.Value)
        {
        }
    }
}
