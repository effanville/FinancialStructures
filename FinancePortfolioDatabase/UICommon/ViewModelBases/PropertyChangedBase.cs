using System.ComponentModel;

namespace UICommon.ViewModelBases
{
    /// <summary>
    /// Base implementation of INotifyPropertyChanged interface
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event handler controlling property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// invokes the event for property changed.
        /// </summary>
        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
