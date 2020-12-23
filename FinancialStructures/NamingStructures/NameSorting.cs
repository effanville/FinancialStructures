using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.Statistics;

namespace FinancialStructures.NamingStructures
{
    public static class NameSorting
    {
        public static void SortName(this List<DayValue_Named> nameList, string fieldToSortWith, SortDirection direction)
        {
            if (direction == SortDirection.Descending)
            {
                if (fieldToSortWith == "Value")
                {
                    nameList.Sort((a, b) => b.Value.CompareTo(a.Value));
                }
                else if (fieldToSortWith == "Day")
                {
                    nameList.Sort((a, b) => b.Day.CompareTo(a.Day));
                }
                else
                {
                    nameList.Sort((a, b) => b.Names.CompareTo(a.Names));
                }
            }
            else
            {
                if (fieldToSortWith == "Value")
                {
                    nameList.Sort((a, b) => a.Value.CompareTo(b.Value));
                }
                else if (fieldToSortWith == "Day")
                {
                    nameList.Sort((a, b) => a.Day.CompareTo(b.Day));
                }
                else
                {
                    nameList.Sort((a, b) => a.Names.CompareTo(b.Names));
                }
            }
        }
    }
}
