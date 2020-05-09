using System;

namespace UICommon.ViewModelBases
{
    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    public abstract class ViewModelBase<T> : PropertyChangedBase where T : class
    {
        /// <summary>
        /// The data to be used in this view model.
        /// </summary>
        public T DataStore
        {
            get;
            set;
        }

        /// <summary>
        /// Any string to use to display in a header or a title of a UI element.
        /// </summary>
        public virtual string Header
        {
            get;
        }

        /// <summary>
        /// Whether this element should be able to be closed.
        /// </summary>
        public virtual bool Closable
        {
            get
            {
                return false;
            }
        }

        public virtual Action<object> LoadSelectedTab
        {
            get;
            set;
        }

        public ViewModelBase(string header)
        {
            Header = header;
        }

        public ViewModelBase(string header, Action<object> loadTab)
        {
            Header = header;
            LoadSelectedTab = loadTab;
        }

        public virtual void UpdateData(T dataToDisplay, Action<object> removeTab)
        {
            DataStore = null;
            DataStore = dataToDisplay;
        }

        /// <summary>
        /// Mechanism to update the data 
        /// </summary>
        public virtual void UpdateData(T dataToDisplay)
        {
            DataStore = null;
            DataStore = dataToDisplay;
        }
    }
}
