using Microsoft.Win32;
using System.Windows;

namespace GUISupport.Services
{
    public class FileInteractionService : IFileInteractionService
    {
        private readonly Window fDefaultParent;

        public FileInteractionService(Window defaultParent)
        {
            fDefaultParent = defaultParent;
        }

        public FileInteractionResult SaveFile(string defaultExt, string fileName, string initialDirectory = null, string filter = null)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = defaultExt, FileName = fileName };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                saving.InitialDirectory = initialDirectory;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                saving.Filter = filter;
            }
            bool? saved = saving.ShowDialog(fDefaultParent);
            if (saved != null && (bool)saved)
            {
                return new FileInteractionResult(saved, saving.FileName);
            }

            return new FileInteractionResult(saved, null);
        }

        public FileInteractionResult OpenFile(string defaultExt, string initialDirectory = null, string filter = null)
        {
            OpenFileDialog openFile = new OpenFileDialog() { DefaultExt = defaultExt };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                openFile.InitialDirectory = initialDirectory;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                openFile.Filter = filter;
            }

            bool? showed = openFile.ShowDialog(fDefaultParent);
            if (showed != null && (bool)showed)
            {
                return new FileInteractionResult(showed, openFile.FileName);
            }

            return new FileInteractionResult(showed, null);
        }
    }
}
