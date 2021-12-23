namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Static extension methods for <see cref="NameData"/>.
    /// </summary>
    public static class NamesExtensions
    {
        /// <summary>
        /// Static extension method to convert a <see cref="NameData"/> into a <see cref="TwoName"/>
        /// </summary>
        public static TwoName ToTwoName(this NameData names)
        {
            return new TwoName(names.Company, names.Name);
        }
    }
}
