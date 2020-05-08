using System.Collections;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace GUISupport
{
    internal class BindableChart : Chart
    {
        public IEnumerable SeriesSource
        {
            get
            {
                return (IEnumerable)GetValue(SeriesSourceProperty);
            }
            set
            {
                SetValue(SeriesSourceProperty, value);
            }
        }

        public static readonly DependencyProperty SeriesSourceProperty = DependencyProperty.Register(
            name: "SeriesSource",
            propertyType: typeof(IEnumerable),
            ownerType: typeof(BindableChart),
            typeMetadata: new PropertyMetadata(
                defaultValue: default(IEnumerable),
                propertyChangedCallback: new PropertyChangedCallback(OnSeriesSourceChanged)
            )
        );

        private static void OnSeriesSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IEnumerable newValue = (IEnumerable)e.NewValue;
            BindableChart source = (BindableChart)d;

            source.Series.Clear();
            if (newValue != null)
            {
                foreach (LineSeries item in newValue)
                {
                    source.Series.Add(item);
                }
            }
        }
    }
}
