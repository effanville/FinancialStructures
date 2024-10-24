﻿using System;

using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Tests.TestDatabaseConstructor
{
    public class SectorConstructor
    {
        public Sector item;

        public SectorConstructor(string company, string name, string currency = null, string url = null)
        {
            NameData names = new NameData(company, name, currency, url);
            item = new Sector(names);
        }

        public SectorConstructor WithData(DateTime date, decimal valueToAdd)
        {
            item.Values.SetData(date, valueToAdd);
            return this;
        }
    }
}
