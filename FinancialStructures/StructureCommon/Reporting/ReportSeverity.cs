namespace StructureCommon.Reporting
{
    /// <summary>
    /// How serious the report is.
    /// </summary>
    public enum ReportSeverity
    {
        /// <summary>
        /// The most serious level, always to be returned.
        /// </summary>
        Critical,

        /// <summary>
        /// Middle seriousness.
        /// </summary>
        Useful,

        /// <summary>
        /// Low seriousness, only output if everything is to be recorded.
        /// </summary>
        Detailed
    }
}
