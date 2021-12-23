using System.Windows.Media;

namespace FinancePortfolioDatabase.GUI.TemplatesAndStyles
{
    /// <summary>
    /// Stores the colours and brushes used in the program.
    /// </summary>
    public static class Colours
    {
        /// <summary>
        /// A brush to be used for background.
        /// </summary>
        public static Brush BackgroundBrush
        {
            get;
            set;
        } = new SolidColorBrush(Colors.AliceBlue);

        /// <summary>
        /// A brush for secondary background colours.
        /// </summary>
        public static Brush BackgroundSecondaryBrush
        {
            get;
            set;
        } = new SolidColorBrush(Colors.LightBlue);

        /// <summary>
        /// A brush with the colour for buttons.
        /// </summary>
        public static Brush DefaultButtonBrush
        {
            get;
            set;
        } = new SolidColorBrush(Colors.Lavender);

        /// <summary>
        /// A brush for any highlights.
        /// </summary>
        public static Brush HighlightBrush
        {
            get;
            set;
        } = new SolidColorBrush(Colors.DarkGray);

        /// <summary>
        /// A brush for any highlights.
        /// </summary>
        public static Brush ForegroundBrush
        {
            get;
            set;
        } = new SolidColorBrush(Colors.Black);
    }
}
