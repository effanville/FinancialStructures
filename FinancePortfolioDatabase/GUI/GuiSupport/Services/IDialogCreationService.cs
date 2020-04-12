using System.Windows;

namespace GUISupport.Services
{
    public interface IDialogCreationService
    {
        MessageBoxResult ShowMessageBox(string text, string title, MessageBoxButton buttons, MessageBoxImage imageType);

        MessageBoxResult ShowMessageBox(Window owner, string text, string title, MessageBoxButton buttons, MessageBoxImage imageType);
    }
}
