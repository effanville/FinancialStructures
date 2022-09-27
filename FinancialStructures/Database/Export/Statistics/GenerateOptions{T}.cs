using System;
using System.Collections.Generic;

namespace FinancialStructures.Database.Export.Statistics
{
    /// <summary>
    /// Contains data for generation of values, where the values are specified
    /// from an <see cref="Enum"/>.
    /// </summary>
    /// <typeparam name="T">An enumerable listing all possible values.</typeparam>
    public class GenerateOptions<T> where T : Enum
    {
        /// <summary>
        /// Should this table be displayed.
        /// </summary>
        public bool ShouldGenerate
        {
            get;
        }

        /// <summary>
        /// What fields to display in the table.
        /// </summary>
        public IReadOnlyList<T> GenerateFields
        {
            get;
        } = new List<T>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenerateOptions()
        {
            ShouldGenerate = true;
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="shouldGenerate">Whether this should be displayed.</param>
        /// <param name="display">What fields to display.</param>
        public GenerateOptions(bool shouldGenerate, IReadOnlyList<T> display)
        {
            ShouldGenerate = shouldGenerate;
            GenerateFields = display;
        }
    }
}
