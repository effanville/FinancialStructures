using System.Windows.Media;

namespace Colouring
{
    /// <summary>
    /// Stores the colours and brushes used in the program.
    /// </summary>
    public static class Colours
    {
        /// <summary>
        /// The colour used for the background
        /// </summary>
        public static Color BackgroundMainColour
        {
            get;
            set;
        } = Colors.AliceBlue;

        /// <summary>
        /// A bruch with the same colour as <see cref="BackgroundMainColour"/>.
        /// </summary>
        public static Brush BackgroundBrush
        {
            get;
            set;
        } = new SolidColorBrush(BackgroundMainColour);

        /// <summary>
        /// A secondary background colour.
        /// </summary>
        public static Color BackgroundSecondColour
        {
            get;
            set;
        } = Colors.LightBlue;

        /// <summary>
        /// A brush with colour of <see cref="BackgroundSecondColour"/>
        /// </summary>
        public static Brush BackgroundSecondaryBrush
        {
            get;
            set;
        } = new SolidColorBrush(BackgroundSecondColour);

        /// <summary>
        /// The colour used on buttons.
        /// </summary>
        public static Color ButtonColour
        {
            get;
            set;
        } = Colors.Lavender;

        /// <summary>
        /// A brush with the same colour as <see cref="ButtonColour"/>.
        /// </summary>
        public static Brush DefaultButtonBrush
        {
            get;
            set;
        } = new SolidColorBrush(ButtonColour);
    }
}
