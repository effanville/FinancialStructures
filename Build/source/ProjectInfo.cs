namespace Build
{
    public class ProjectInfo
    {
        public string FolderName
        {
            get; set;
        }

        public ProjectInfo(string folderName, string projectName)
        {
            FolderName = folderName;
            ProjectName = projectName;
        }

        public string ProjectName
        {
            get; set;
        }

    }
}
