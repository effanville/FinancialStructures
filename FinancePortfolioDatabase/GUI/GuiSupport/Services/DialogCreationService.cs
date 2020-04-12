using System.Windows;

namespace GUISupport.Services
{
    public class DialogCreationService : IDialogCreationService
    {
        private readonly Window fDefaultParent;

        public DialogCreationService(Window defaultParent)
        {
            fDefaultParent = defaultParent;
        }

        public MessageBoxResult ShowMessageBox(string text, string title, MessageBoxButton buttons, MessageBoxImage imageType)
        {
            return MessageBox.Show(fDefaultParent, text, title, buttons, imageType);
        }

        public MessageBoxResult ShowMessageBox(Window owner, string text, string title, MessageBoxButton buttons, MessageBoxImage imageType)
        {
            return MessageBox.Show(owner, text, title, buttons, imageType);
        }
    }
}
