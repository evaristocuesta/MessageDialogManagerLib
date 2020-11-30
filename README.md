# MessageDialogManagerLib
[![NuGet](https://img.shields.io/nuget/v/MessageDialogManagerLib)](https://nuget.org/packages/MessageDialogManagerLib/) [![.NET Core](https://github.com/evaristocuesta/MessageDialogManagerLib/workflows/.NET%20Core/badge.svg)](https://github.com/evaristocuesta/MessageDialogManagerLib/actions)

**MessageDialogManagerLib** is a library to easily use dialogs from ViewModels in WPF Mahapps.Metro applications. This library uses [Mahapps.Metro](https://mahapps.com/) to show info, progress, ok/cancel or custom dialog. To show an open file dialog, this library uses Microsoft.Win32.OpenFileDialog, to show a save file dialog uses Microsoft.Win32.SaveFileDialog and to show a folder browser, this library uses [FolderBrowserEx](https://github.com/evaristocuesta/FolderBrowserEx).

Supporting .NET Framework (4.5+) and .NET Core (3.0 and 3.1)

## Table of contents

  - [Introduction](#introduction)
  - [Getting Started](#getting-started)
  - [License](#license)
  - [Credits](#credits)

## Introduction

In both WPF .NET Framework and .NET Core applications we can use dialogs. The problem comes when you want to show a dialog from a ViewModel, especially when you want to test your ViewModel and dialogs block your tests. 

The aim of this project is to offer a library with an interface to easily inject in your ViewModel. Your ViewModel will be independent from dialogs implementations and will be testable without blocks.  

## Getting Started

To use this library, there are a few options:

  - Download this repository
  - Use the [MessageDialogManagerLib Nuget Package](https://nuget.org/packages/MessageDialogManagerLib/)

To use this library, the WPF application has to use [Mahapps.Metro](https://mahapps.com/).

The **MessageDialogManagerLib** uses the IMessageDialogManager interface.
```csharp
public interface IMessageDialogManager
{
    /// <summary>
    /// Shows a folder browser dialog
    /// </summary>
    /// <param name="title">Sets the title of the dialog</param>
    /// <param name="initialPath">Sets the initial path of the dialog</param>
    /// <returns>Returns if a folder has been selected</returns>
    bool ShowFolderBrowser(string title, string initialPath);

    /// <summary>
    /// Gets the selected folder
    /// </summary>
    string FolderPath { get; set; }

    /// <summary>
    /// Shows a file browser dialog
    /// </summary>
    /// <param name="title">Sets the title of the dialog</param>
    /// <param name="initialPath">Sets the initial path of the dialog</param>
    /// <param name="filter">Sets a filter to show only the files that meet the filter</param>
    /// <returns>Returns if a file has been selected</returns>
    bool ShowFileBrowser(string title, string initialPath, string filter);

    /// <summary>
    /// Gets the selected file
    /// </summary>
    string FilePath { get; set; }

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
```
To use **MessageDialogManagerLib** in an application, you can follow this example code. There are others examples in the directory Samples of the solution. 

```csharp
MessageDialogManagerMahapps messageDialogManager = new MessageDialogManagerMahapps(App.Current);
MessageDialogResult result = 
                await messageDialogManager.ShowOkCancelDialogAsync(
                    "Ok Cancel Dialog", 
                    "This is a Ok Cancel Dialog");
if (result == MessageDialogResult.OK)
    await messageDialogManager.ShowInfoDialogAsync("Result", "You select Ok");
else
    await messageDialogManager.ShowInfoDialogAsync("Result", "You select Cancel");
```

If you want to use the **MessageDialogManagerLib** library from a View Model, follow this example code. You can find the complete example in the directory Samples of the solution. 

```csharp
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;

namespace MessageDialogManagerLib
{
    public class MessageDialogManagerMahapps : IMessageDialogManager
    {
        private ProgressDialogController _controller;
        private readonly System.Windows.Application _app;
        /// <summary>
        /// We have to use this private variable instead a local variable to show a custom dialog
        /// to avoid an error closing the dialog
        /// </summary>
        private CustomDialog _customView;
        private readonly Dictionary<IDialogViewModel, CustomDialog> _customDialogs;
        private MetroWindow MetroWindow => (MetroWindow)_app.MainWindow;

        /// <summary>
        /// Gets the selected folder
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets the selected file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the file to save
        /// </summary>
        public string FilePathToSave { get; set; }

        public MessageDialogManagerMahapps(System.Windows.Application app)
        {
            _app = app;
            _customDialogs = new Dictionary<IDialogViewModel, CustomDialog>();
        }

        /// <summary>
        /// Shows a ok and cancel dialog
        /// </summary>
        /// <param name="text">Sets the text of the dialog</param>
        /// <param name="title">Sets the title of the dialog</param>
        /// MessageDialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, MessageDialogResult.Cancel.
        public async Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title)
        {
            var result =
              await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);

            return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative
              ? MessageDialogResult.OK
              : MessageDialogResult.Cancel;
        }

        /// <summary>
        /// Shows a info dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="message">Sets the message of the dialog</param>
        /// <returns></returns>
        public async Task ShowInfoDialogAsync(string title, string message)
        {
            await MetroWindow.ShowMessageAsync(title, message);
        }

        /// <summary>
        /// Shows a progress dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="message">Sets the message of the dialog</param>
        /// <returns></returns>
        public async Task ShowProgress(string title, string message)
        {
            _controller = await MetroWindow.ShowProgressAsync(title, message);
            _controller.Minimum = 0;
            _controller.Maximum = 100;
        }

        /// <summary>
        /// Updates the progress
        /// </summary>
        /// <param name="progress">Sets de progress</param>
        public void UpdateProgress(double progress)
        {
            if (_controller != null)
                _controller.SetProgress(progress);
        }

        /// <summary>
        /// Updates the message progress
        /// </summary>
        /// <param name="message">Sets the message progress</param>
        public void UpdateMessageProgress(string message)
        {
            if (_controller != null)
                _controller.SetMessage(message);
        }

        /// <summary>
        /// Closes the progress dialog
        /// </summary>
        /// <returns></returns>
        public async Task CloseProgress()
        {
            if (_controller != null)
            {
                await _controller.CloseAsync();
                _controller = null;
            }
        }

        /// <summary>
        /// Shows a folder browser dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="initialPath">Sets the initial path of the dialog</param>
        /// <returns>Returns if a folder has been selected</returns>
        public bool ShowFolderBrowser(string title, string initialDirectory)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Title = title
            };
            bool res;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderPath = folderBrowserDialog.SelectedFolder;
                res = true;
            }
            else
            {
                FolderPath = string.Empty;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Shows a file browser dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="initialPath">Sets the initial path of the dialog</param>
        /// <param name="filter">Sets a filter to show only the files that meet the filter</param>
        /// <returns>Returns if a file has been selected</returns>
        public bool ShowFileBrowser(string title, string initialPath, string filter)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (Directory.Exists(initialPath))
                openFileDialog.InitialDirectory = initialPath;
            else
                openFileDialog.InitialDirectory = "";
            openFileDialog.Filter = filter;
            openFileDialog.Title = title;
            bool res;
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                res = true;
            }
            else
            {
                FilePath = string.Empty;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// Shows a save file dialog
        /// </summary>
        /// <param name="title">Sets the title of the dialog</param>
        /// <param name="fileName">Sets the file's name</param>
        /// <param name="defaultExt">Sets the default file's extension</param>
        /// <param name="filter">Sets the filter</param>
        /// <returns>Returns if a file has been saved</returns>
        public bool ShowSaveFileDialog(string title, string initialPath, string fileName, string defaultExt, string filter)
        {
            bool res = false;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = title,
                FileName = fileName,
                DefaultExt = defaultExt,
                Filter = filter
            };
            if (Directory.Exists(initialPath))
                saveFileDialog.InitialDirectory = initialPath;
            else
                saveFileDialog.InitialDirectory = "";

            if (saveFileDialog.ShowDialog() == true)
            {
                res = true;
                FilePathToSave = saveFileDialog.FileName;
            }
            else
            {
                FilePathToSave = "";
            }


            return res;
        }

        /// <summary>
        /// Shows a custom dialog
        /// </summary>
        /// <param name="viewModel">Sets the viewmodel attached to the custom dialog</param>
        /// <returns></returns>
        public async Task ShowDialogAsync(IDialogViewModel viewModel)
        {
            if (_customDialogs.ContainsKey(viewModel))
            {
                _customView = _customDialogs[viewModel];
            }
            else
            {
                _customView = SearchCustomDialog(viewModel);
                if (_customView != null)
                {
                    _customDialogs.Add(viewModel, _customView);
                }
            }
            if (_customView != null)
            {
                if (viewModel.CloseCommand == null)
                    viewModel.CloseCommand = new Command(async () => await MetroWindow.HideMetroDialogAsync(_customView), null);
                _customView.DataContext = viewModel;
                await MetroWindow.ShowMetroDialogAsync(_customView);
            }
        }

        private CustomDialog SearchCustomDialog(IDialogViewModel viewModel)
        {
            CustomDialog customView = null;
            foreach (var resource in _app.Resources.Values)
            {
                if (resource is DataTemplate dataTemplate && dataTemplate.DataType.Equals(viewModel.GetType()))
                {
                    customView = dataTemplate.LoadContent() as CustomDialog;
                    break;
                }
            }
            return customView;
        }
    }
}
```

## License

Copyright © 2020 Evaristo Cuesta 

**MessageDialogManagerLib** is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/evaristocuesta/MessageDialogManagerLib/blob/master/LICENSE).

## Credits

This project uses [Mahapps.Metro](https://mahapps.com/) and [FolderBrowserEx](https://github.com/evaristocuesta/FolderBrowserEx).

