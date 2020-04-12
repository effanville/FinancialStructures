namespace GUISupport.Services
{
    public interface IFileInteractionService
    {
        FileInteractionResult SaveFile(string defaultExt, string fileName, string initialDirectory = null, string filter = null);

        FileInteractionResult OpenFile(string defaultExt, string initialDirectory = null, string filter = null);
    }
}
