namespace FinancialStructures.NamingStructures
{
    public static class NamesExtensions
    {
        public static TwoName ToTwoName(this NameData names)
        {
            return new TwoName(names.Company, names.Name);
        }
    }
}
