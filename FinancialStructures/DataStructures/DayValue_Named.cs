using FinancialStructures.Mathematics;
using PortfolioStatsCreatorHelper;
using System;
using System.Collections.Generic;

namespace FinancialStructures.DataStructures
{
    public class DayValue_Named : DailyValuation
    {
        public override int CompareTo(object obj)
        {
            if (obj is DayValue_Named value)
            {
                if (Company == value.Company)
                {
                    return Name.CompareTo(value.Name);
                }

                return Company.CompareTo(value.Company);
            }

            return 0;
        }

        public string HTMLTableHeader(UserOptions options, List<string> names)
        {
            var properties = this.GetType().GetProperties();
            string htmlHeader = string.Empty;
            foreach (var property in properties)
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
            var properties = this.GetType().GetProperties();
            string htmlData = "<th scope=\"row\">";

            for (int i = 0; i < properties.Length; i++)
            {
                if (names.Contains(properties[i].Name))
                {
                    if (i != 0)
                    {
                        htmlData += "<td>";
                    }
                    if (double.TryParse(properties[i].GetValue(this).ToString(), out double value))
                    {
                        htmlData += MathSupport.Trunc(value);
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

        public override string ToString()
        {
            //both name and company cannot be null so this is all cases.
            if (string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(Name))
            {
                return Name + "-" + base.ToString();
            }
            if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Company))
            {
                return Company + "-" + base.ToString();
            }

            return Company + "-" + Name + "-" + base.ToString();
        }

        /// <summary>
        /// Added company of the Daily valuation
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Added name of the daily valuation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public DayValue_Named() : base()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DayValue_Named(string name, string company, DateTime day, double value) 
            : base(day, value)
        {
            Name = name;
            Company = company;
        }

        /// <summary>
        /// Constructor to create an instance from a base class instance.
        /// </summary>
        public DayValue_Named(string name, string company, DailyValuation toAddOnto) 
            : this(name, company, toAddOnto.Day, toAddOnto.Value)
        {
        }
    }
}
