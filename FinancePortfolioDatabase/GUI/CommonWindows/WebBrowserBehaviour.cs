using System;
using System.Windows;
using System.Windows.Controls;

namespace WebBrowserWindow
{
    /// <summary>
    /// Class to assist with web browser to set a binding source.
    /// </summary>
    public static class WebBrowserBehaviours
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(object), typeof(WebBrowserBehaviours), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static object GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, object value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!(o is WebBrowser browser))
            {
                return;
            }

            Uri uri = null;

            if (e.NewValue is string)
            {
                string uriString = e.NewValue as string;
                uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            }
            else if (e.NewValue is Uri)
            {
                uri = e.NewValue as Uri;
            }

            browser.Source = uri;
        }
    }
}
