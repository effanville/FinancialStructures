using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.StatisticStructures;
using NUnit.Framework;
using StructureCommon.DataStructures;

namespace FinancialStructures.Tests.NamingStructuresTests
{
    [TestFixture]
    public class NameSortingTests
    {
        //Note currently dont test for name sorting order.
        [TestCase("2005/1/1", "5", "2004/1/1", "6", "2003/1/1", "7", "2002/1/1", "8", "2001/1/1", "9", "Value", SortDirection.Ascending, "2005/1/1", "5", "2004/1/1", "6", "2003/1/1", "7", "2002/1/1", "8", "2001/1/1", "9")]
        [TestCase("2005/1/1", "5", "2004/1/1", "6", "2003/1/1", "7", "2002/1/1", "8", "2001/1/1", "9", "Value", SortDirection.Descending, "2001/1/1", "9", "2002/1/1", "8", "2003/1/1", "7", "2004/1/1", "6", "2005/1/1", "5")]
        [TestCase("2005/1/1", "5", "2004/1/1", "6", "2003/1/1", "7", "2002/1/1", "8", "2001/1/1", "9", "Day", SortDirection.Descending, "2005/1/1", "5", "2004/1/1", "6", "2003/1/1", "7", "2002/1/1", "8", "2001/1/1", "9")]
        [TestCase("2005/1/1", "5", "2004/1/1", "6", "2003/1/1", "7", "2002/1/1", "8", "2001/1/1", "9", "Day", SortDirection.Ascending, "2001/1/1", "9", "2002/1/1", "8", "2003/1/1", "7", "2004/1/1", "6", "2005/1/1", "5")]
        public void SortingTests(DateTime date1, double value1, DateTime date2, double value2, DateTime date3, double value3, DateTime date4, double value4, DateTime date5, double value5, string field, SortDirection direction, DateTime sortedDate1, double sortedValue1, DateTime sortedDate2, double sortedValue2, DateTime sortedDate3, double sortedValue3, DateTime sortedDate4, double sortedValue4, DateTime sortedDate5, double sortedValue5)
        {
            var listToSort = new List<DayValue_Named>();
            listToSort.Add(new DayValue_Named("company", "name", new DailyValuation(date1, value1)));
            listToSort.Add(new DayValue_Named("company", "name", new DailyValuation(date2, value2)));
            listToSort.Add(new DayValue_Named("company", "name", new DailyValuation(date3, value3)));
            listToSort.Add(new DayValue_Named("company", "name", new DailyValuation(date4, value4)));
            listToSort.Add(new DayValue_Named("company", "name", new DailyValuation(date5, value5)));

            NameSorting.SortName(listToSort, field, direction);

            Assert.AreEqual(sortedDate1, listToSort[0].Day);
            Assert.AreEqual(sortedValue1, listToSort[0].Value);

            Assert.AreEqual(sortedDate2, listToSort[1].Day);
            Assert.AreEqual(sortedValue2, listToSort[1].Value);

            Assert.AreEqual(sortedDate3, listToSort[2].Day);
            Assert.AreEqual(sortedValue3, listToSort[2].Value);

            Assert.AreEqual(sortedDate4, listToSort[3].Day);
            Assert.AreEqual(sortedValue4, listToSort[3].Value);

            Assert.AreEqual(sortedDate5, listToSort[4].Day);
            Assert.AreEqual(sortedValue5, listToSort[4].Value);
        }
    }
}
