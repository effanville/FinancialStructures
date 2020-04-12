﻿namespace GUISupport.Services
{
    /// <summary>
    /// The result of an interaction with a file system.
    /// </summary>
    public class FileInteractionResult
    {
        /// <summary>
        /// Whether the interaction was a success or not.
        /// </summary>
        public bool? Success
        {
            get;
            set;
        }

        /// <summary>
        /// The path obtained from the interaction.
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        public FileInteractionResult(bool? success, string filePath)
        {
            Success = success;
            FilePath = filePath;
        }

        public FileInteractionResult()
        {
        }
    }
}
