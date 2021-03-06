﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageDialogManagerLib
{
    public interface IMessageDialogManager
    {
        /// <summary>
        /// Shows a folder browser dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="initialPath">Sets the initial path of the dialog</param>
        /// <param name="allowMultiSelect">Allow multiple folders selection</param>
        /// <returns>Returns if a folder has been selected</returns>
        bool ShowFolderBrowser(string title, string initialPath, bool allowMultiSelect = false);

        /// <summary>
        /// Gets the selected folder
        /// </summary>
        string FolderPath { get; }

        /// <summary>
        /// Gets selected folders when AllowMultiSelect is true.
        /// </summary>
        List<string> FolderPaths { get; }

        /// <summary>
        /// Shows a file browser dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="initialPath">Sets the initial path of the dialog</param>
        /// <param name="filter">Sets a filter to show only the files that meet the filter</param>
        /// <param name="allowMultiSelect">Allow multiple files selection</param>
        /// <returns>Returns if a file has been selected</returns>
        bool ShowFileBrowser(string title, string initialPath, string filter, bool allowMultiSelect = false);

        /// <summary>
        /// Gets the selected file
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets the selected files when AllowMultiSelect is true
        /// </summary>
        List<string> FilePaths { get; }

        /// <summary>
        /// Shows a save file dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="initialPath">Sets the initial path of the dialog</param>
        /// <param name="fileName">Sets the file's name</param>
        /// <param name="defaultExt">Sets the default file's extension</param>
        /// <param name="filter">Sets the filter</param>
        /// <returns>Returns if a file has been saved</returns>
        bool ShowSaveFileDialog(string title, string initialPath, string fileName, string defaultExt, string filter);

        /// <summary>
        /// Gets the file to save
        /// </summary>
        string FilePathToSave { get; set; }

        /// <summary>
        /// Shows a ok and cancel dialog
        /// </summary>
        /// <param name="text">Sets the text of the dialog</param>
        /// <param name="title">Sets the title of the dialog</param>
        /// MessageDialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, MessageDialogResult.Cancel.
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);

        /// <summary>
        /// Shows a info dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="message">Sets the message of the dialog</param>
        /// <returns></returns>
        Task ShowInfoDialogAsync(string title, string message);

        /// <summary>
        /// Shows a progress dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="message">Sets the message of the dialog</param>
        /// <returns></returns>
        Task ShowProgress(string title, string message);

        /// <summary>
        /// Updates the progress
        /// </summary>
        /// <param name="progress">Sets de progress</param>
        void UpdateProgress(double progress);

        /// <summary>
        /// Updates the message progress
        /// </summary>
        /// <param name="message">Sets the message progress</param>
        void UpdateMessageProgress(string message);

        /// <summary>
        /// Closes the progress dialog
        /// </summary>
        /// <returns></returns>
        Task CloseProgress();

        /// <summary>
        /// Shows a custom dialog
        /// </summary>
        /// <param name="viewModel">Sets the viewmodel attached to the custom dialog</param>
        /// <returns></returns>
        Task ShowDialogAsync(IDialogViewModel viewModel);
    }
}
