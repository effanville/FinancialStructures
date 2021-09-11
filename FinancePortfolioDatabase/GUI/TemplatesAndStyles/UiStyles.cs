using System.Windows.Media;

namespace FinancePortfolioDatabase.GUI.TemplatesAndStyles
{
    /// <summary>
    /// Contains styling info for the UI.
    /// </summary>
    public sealed class UiStyles
    {
        /// <summary>
        /// A brush for the primary background colour.
        /// </summary>
        public Brush BackgroundBrush
        {
            get;
            set;
        } = Colours.BackgroundBrush;

        /// <summary>
        /// A brush for secondary colours.
        /// </summary>
        public Brush BackgroundSecondaryBrush
        {
            get;
            set;
        } = Colours.BackgroundSecondaryBrush;

        /// <summary>
        /// A brush for any highlights.
        /// </summary>
        public static Brush HighlightBrush
        {
            get;
            set;
        } = Colours.HighlightBrush;

        /// <summary>
        /// A brush to be used on buttons.
        /// </summary>
        public Brush DefaultButtonBrush
        {
            get;
            set;
        } = Colours.DefaultButtonBrush;

        /// <summary>
        /// The colour for foreground objects, eg text.
        /// </summary>
        public Brush ForegroundBrush
        {
            get;
            set;
        } = Colours.ForegroundBrush;

        /// <summary>
        /// The font family for all font display.
        /// </summary>
        public FontFamily FontFamily
        {
            get;
            set;
        }

        /// <summary>
        /// A small font size to use.
        /// </summary>
        public int SmallFontSize
        {
            get;
            set;
        } = 11;

        /// <summary>
        /// An intermediary font size.
        /// </summary>
        public int MediumFontSize
        {
            get;
            set;
        } = 12;

        /// <summary>
        /// The size of font to use in main headers.
        /// </summary>
        public int LargeFontSize
        {
            get;
            set;
        } = 14;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UiStyles()
        {
            FontFamily = new FontFamily("Century Gothic");
        }
    }
}
