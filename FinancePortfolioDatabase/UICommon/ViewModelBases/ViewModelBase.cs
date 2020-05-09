using System;

namespace UICommon.ViewModelBases
{
    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    public abstract class ViewModelBase<T> : PropertyChangedBase where T : class
    {
        /// <summary>
        /// Any string to use to display in a header or a title of a UI element.
        /// </summary>
        public virtual string Header { get; }

        /// <summary>
        /// Whether this element should be able to be closed.
        /// </summary>
        public virtual bool Closable { get { return false; } }

        public virtual Action<object> LoadSelectedTab { get; set; }

        public ViewModelBase(string header)
        {
            Header = header;
        }

        public ViewModelBase(string header, Action<object> loadTab)
        {
            Header = header;
            LoadSelectedTab = loadTab;
        }

        public virtual void UpdateData(T portfolio, Action<object> removeTab)
        {
        }

        public abstract void UpdateData(T portfolio);
    }
}
