namespace Effanville.FinancialStructures.NamingStructures;

/// <summary>
/// Static extension methods for <see cref="NameData"/>.
/// </summary>
public static class NamesExtensions
{
    /// <summary>
    /// Takes a copy of a <see cref="TwoName"/> and produces a new <see cref="TwoName"/>
    /// </summary>
    public static TwoName ToTwoName(this TwoName names) => new TwoName(names.Company, names.Name);

    public static NameData ToNameData(this TwoName names) => new NameData(names.Company, names.Name);
}
