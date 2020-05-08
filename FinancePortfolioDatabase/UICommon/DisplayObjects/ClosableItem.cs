using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UICommon.DisplayObjects
{
    class ClosableTab : TabItem
    {
        /// <summary>
        /// Property - Set the Title of the Tab
        ///</summary>
        public string Title
        {
            set
            {
                ((ClosableHeader)this.Header).label_TabTitle.Content = value;
            }
        }

        // Constructor
        public ClosableTab()
        {
            // Create an instance of the usercontrol
            var closableTabHeader = new ClosableHeader();
            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;
            // Attach to the CloseableHeader events
            closableTabHeader.button_close.Click += new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
        }

        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((ClosableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
            }
        }

        // Button Close Click - Remove the Tab - (or raise
        // an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
            ((TabControl)this.Parent).Items.Remove(this);
        }
        // Label SizeChanged - When the Size of the Label changes
        // (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((ClosableHeader)this.Header).button_close.Margin = new Thickness(
               ((ClosableHeader)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }
    }
}
