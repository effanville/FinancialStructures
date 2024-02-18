namespace FinancialStructures.ExpensesStructures.Models
{
    /// <summary>
    /// The category the expense is associated to.
    /// Would be good if this was user configurable in
    /// a settings file.
    /// </summary>
    public enum ExpenseCategory
    {
        /// <summary>
        /// The expense is from an unknown source (this acts as the default).
        /// </summary>
        Unknown,

        /// <summary>
        /// The expense is from council tax.
        /// </summary>
        CouncilTax,

        /// <summary>
        /// The expense is from tax on income.
        /// </summary>
        IncomeTax,

        /// <summary>
        /// The expense is from national Insurance.
        /// </summary>
        NationalInsurance,

        /// <summary>
        /// The expense is from paying into or out of a pension.
        /// </summary>
        Pension,

        /// <summary>
        /// The expense is pertaining to Water or sewage costs.
        /// </summary>
        WaterSewage,

        /// <summary>
        /// The expense is pertaining to electricity supply.
        /// </summary>
        Electricity,

        /// <summary>
        /// The expense is pertaining to gas supply.
        /// </summary>
        Gas,

        /// <summary>
        /// The expense is pertaining to broadband.
        /// </summary>
        Broadband,

        /// <summary>
        /// The expense is pertaining to a car.
        /// </summary>
        Car,

        /// <summary>
        /// The expense is pertaining to car insurance.
        /// </summary>
        CarInsurance,

        /// <summary>
        /// The expense is pertaining to buildings and contents insurance.
        /// </summary>
        HouseInsurance,

        /// <summary>
        /// The expense is pertaining to a mortgage.
        /// </summary>
        Mortgage,

        /// <summary>
        /// The expense is pertaining to normal food costs.
        /// </summary>
        Food,

        /// <summary>
        /// The expense is due to eating out at a restaurant.
        /// </summary>
        EatingOut,

        /// <summary>
        /// The expense is due to purchasing alcohol.
        /// </summary>
        Alcohol,

        /// <summary>
        /// The expense is pertaining to car fuel.
        /// </summary>
        CarFuel,

        /// <summary>
        /// The expense is due to buying clothing.
        /// </summary>
        Clothing,

        /// <summary>
        /// The expense is due to buying a phone or a phone contract.
        /// </summary>
        Phone,

        /// <summary>
        /// The expense is from the income.
        /// </summary>
        Salary,

        /// <summary>
        /// The expense is from having a lodger.
        /// </summary>
        Lodger,

        /// <summary>
        /// The expense is from renting (income or expenditure).
        /// </summary>
        Rent,

        /// <summary>
        /// The expense is from parents.
        /// </summary>
        Parents,

        /// <summary>
        /// The expense is to do with sport.
        /// </summary>
        Sport,

        /// <summary>
        /// The expense is for something fun.
        /// </summary>
        Fun,

        /// <summary>
        /// The expense is a miscellaneous extra, not covered by the above.
        /// </summary>
        Misc
    }
}
