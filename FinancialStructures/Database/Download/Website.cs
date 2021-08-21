namespace FinancialStructures.Database.Download
{
    /// <summary>
    /// Details the sort of website.
    /// </summary>
    public enum Website
    {
        /// <summary>
        /// The website is a Morningstar website.
        /// </summary>
        Morningstar,

        /// <summary>
        /// The website is a Yahoo website.
        /// </summary>
        Yahoo,

        /// <summary>
        /// The website is a Google website.
        /// </summary>
        Google,

        /// <summary>
        /// The website is a Bloomberg website.
        /// </summary>
        Bloomberg,

        /// <summary>
        /// The website is a TrustNet website.
        /// </summary>
        TrustNet,

        /// <summary>
        /// The website is a Financal Times website.
        /// </summary>
        FT,

        /// <summary>
        /// The website is not of a form that can be interpreted.
        /// </summary>
        NotImplemented
    }
}
