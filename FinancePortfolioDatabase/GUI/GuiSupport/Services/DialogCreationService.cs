using System.Windows;

namespace GUISupport.Services
{
    /// <summary>
    /// Created dialog boxes in the UI. Note that this should live in the UI part, but is a service that can be used in
    /// the view model area of the code.
    /// </summary>
    public class DialogCreationService : IDialogCreationService
    {
        /// <summary>
        /// The default parent to use if non are selected.
        /// </summary>
        private readonly Window fDefaultParent;

        /// <summary>
        /// The standard constructor to use.
        /// </summary>
        public DialogCreationService(Window defaultParent)
        {
            fDefaultParent = defaultParent;
        }

        /// <summary>
        /// Shows a standard message box with the specified parameters.
        /// </summary>
        public MessageBoxResult ShowMessageBox(string text, string title, MessageBoxButton buttons, MessageBoxImage imageType)
        {
            return MessageBox.Show(fDefaultParent, text, title, buttons, imageType);
        }

        /// <summary>
        /// Shows a standard message box with the specified parameters with non-default owner.
        /// </summary>
        public MessageBoxResult ShowMessageBox(Window owner, string text, string title, MessageBoxButton buttons, MessageBoxImage imageType)
        {
            return MessageBox.Show(owner, text, title, buttons, imageType);
        }
    }
}
