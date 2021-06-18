using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns a dictionary with the distribution of values
        /// </summary>
        public static Dictionary<DateTime, int> EntryDistribution(this IPortfolio portfolio, Totals elementType = Totals.Security, TwoName company = null)
        {
            switch (elementType)
            {
                case Totals.All:
                {
                    return portfolio.EntryDistribution(Totals.Security);
                }
                case (Totals.Security):
                {
                    var total = new Dictionary<DateTime, int>();
                    foreach (var desired in portfolio.FundsThreadSafe)
                    {
                        if (desired.Any())
                        {
                            foreach (var value in desired.Shares.Values())
                            {
                                if (total.TryGetValue(value.Day, out _))
                                {
                                    total[value.Day] += 1;
                                }
                                else
                                {
                                    total.Add(value.Day, 1);
                                }
                            }

                            foreach (var priceValue in desired.UnitPrice.Values())
                            {
                                if (total.TryGetValue(priceValue.Day, out _))
                                {
                                    total[priceValue.Day] += 1;
                                }
                                else
                                {
                                    total.Add(priceValue.Day, 1);
                                }
                            }

                            foreach (var investmentValue in desired.Investments.Values())
                            {
                                if (total.TryGetValue(investmentValue.Day, out _))
                                {
                                    total[investmentValue.Day] += 1;
                                }
                                else
                                {
                                    total.Add(investmentValue.Day, 1);
                                }
                            }
                        }
                    }

                    return total;
                }
                case (Totals.SecurityCompany):
                {
                    var total = new Dictionary<DateTime, int>();

                    var securities = portfolio.CompanySecurities(company.Company);
                    if (securities.Count == 0)
                    {
                        return new Dictionary<DateTime, int>();
                    }

                    foreach (ISecurity desired in securities)
                    {
                        if (desired.Any())
                        {
                            foreach (var value in desired.Shares.Values())
                            {
                                if (total.TryGetValue(value.Day, out _))
                                {
                                    total[value.Day] += 1;
                                }
                                else
                                {
                                    total.Add(value.Day, 1);
                                }
                            }

                            foreach (var priceValue in desired.UnitPrice.Values())
                            {
                                if (total.TryGetValue(priceValue.Day, out _))
                                {
                                    total[priceValue.Day] += 1;
                                }
                                else
                                {
                                    total.Add(priceValue.Day, 1);
                                }
                            }

                            foreach (var investmentValue in desired.Investments.Values())
                            {
                                if (total.TryGetValue(investmentValue.Day, out _))
                                {
                                    total[investmentValue.Day] += 1;
                                }
                                else
                                {
                                    total.Add(investmentValue.Day, 1);
                                }
                            }
                        }
                    }

                    return total;
                }
                case Totals.BankAccount:
                {
                    var total = new Dictionary<DateTime, int>();
                    foreach (ICashAccount cashAccount in portfolio.BankAccountsThreadSafe)
                    {
                        foreach (var value in cashAccount.Values.Values())
                        {
                            if (total.TryGetValue(value.Day, out _))
                            {
                                total[value.Day] += 1;
                            }
                            else
                            {
                                total.Add(value.Day, 1);
                            }
                        }
                    }

                    return total;
                }
                default:
                {
                    return new Dictionary<DateTime, int>();
                }
            }
        }
    }
}
