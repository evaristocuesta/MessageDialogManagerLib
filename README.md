# MessageDialogManagerLib
[![NuGet](https://img.shields.io/nuget/v/MessageDialogManagerLib)](https://nuget.org/packages/MessageDialogManagerLib/) [![.NET Core](https://github.com/evaristocuesta/MessageDialogManagerLib/workflows/.NET%20Core/badge.svg)](https://github.com/evaristocuesta/MessageDialogManagerLib/actions)

**MessageDialogManagerLib** is a library to easily use dialogs from ViewModels in WPF Mahapps.Metro applications. This library uses [Mahapps.Metro](https://mahapps.com/) to show info, progress, ok/cancel or custom dialog. To show a file browser, this library uses Microsoft.Win32.OpenFileDialog and to show a folder browser, this library uses [FolderBrowserEx](https://github.com/evaristocuesta/FolderBrowserEx)

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
using CommandLibrary;
using MessageDialogManagerLib;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetFrameworkSample.ViewModel
{
    public class MainWindowViewModel
    {
        private IMessageDialogManager _messageDialogManager;

        public MainWindowViewModel(IMessageDialogManager messageDialogManager)
        {
            _messageDialogManager = messageDialogManager;
            ShowFolderBrowserCommand = 
                new Command(ShowFolderBrowserCommandExecute, ShowFolderBrowserCommandCanExecute);

            ShowFileBrowserCommand = 
                new Command(ShowFileBrowserCommandExecute, ShowFileBrowserCommandCanExecute);

            ShowInfoDialogCommand = 
                new Command(ShowInfoDialogCommandExecute, ShowInfoDialogCommandCanExecute);

            ShowOkCancelDialogCommand = 
                new Command(ShowOkCancelDialogCommandExecute, ShowOkCancelDialogCommandCanExecute);

            ShowProgressCommand = 
                new Command(ShowProgressCommandExecute, ShowProgressCommandCanExecute);

            ShowCustomDialogCommand = 
                new Command(ShowCustomDialogCommandExecute, ShowCustomDialogCommandCanExecute);
        }

        public ICommand ShowFolderBrowserCommand { get; private set; }

        public ICommand ShowFileBrowserCommand { get; private set; }

        public ICommand ShowInfoDialogCommand { get; private set; }

        public ICommand ShowOkCancelDialogCommand { get; private set; }

        public ICommand ShowProgressCommand { get; private set; }

        public ICommand ShowCustomDialogCommand { get; private set; }

        private bool ShowFolderBrowserCommandCanExecute()
        {
            return true;
        }

        private async void ShowFolderBrowserCommandExecute()
        {
            if (_messageDialogManager.ShowFolderBrowser("Select a folder", ""))
                await _messageDialogManager.ShowInfoDialogAsync("Folder Selected", _messageDialogManager.FolderPath);
            else
                await _messageDialogManager.ShowInfoDialogAsync("No folder has been selected", _messageDialogManager.FolderPath);
        }

        private bool ShowFileBrowserCommandCanExecute()
        {
            return true;
        }

        private async void ShowFileBrowserCommandExecute()
        {
            if (_messageDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*"))
                await _messageDialogManager.ShowInfoDialogAsync("File Selected", _messageDialogManager.FilePath);
            else
                await _messageDialogManager.ShowInfoDialogAsync("No file has been selected", _messageDialogManager.FilePath);
        }

        private async void ShowInfoDialogCommandExecute()
        {
            await _messageDialogManager.ShowInfoDialogAsync("Info dialog", "This is a info dialog example");
        }

        private bool ShowInfoDialogCommandCanExecute()
        {
            return true;
        }

        private async void ShowOkCancelDialogCommandExecute()
        {
            MessageDialogResult result = 
                await _messageDialogManager.ShowOkCancelDialogAsync(
                    "Ok Cancel Dialog", 
                    "This is a Ok Cancel Dialog");
            if (result == MessageDialogResult.OK)
                await _messageDialogManager.ShowInfoDialogAsync("Result", "You select Ok");
            else
                await _messageDialogManager.ShowInfoDialogAsync("Result", "You select Cancel");
        }

        private bool ShowOkCancelDialogCommandCanExecute()
        {
            return true;
        }

        private bool ShowProgressCommandCanExecute()
        {
            return true;
        }

        private async void ShowProgressCommandExecute()
        {
            await _messageDialogManager.ShowProgress("Progress", "This is a progress dialog");
            await DoSomeWorkAsync();
            await _messageDialogManager.CloseProgress();
        }

        private async Task DoSomeWorkAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                _messageDialogManager.UpdateProgress(i * 10);
                _messageDialogManager.UpdateMessageProgress($"Step {i} done");
            }
        }

        private bool ShowCustomDialogCommandCanExecute()
        {
            return true;
        }

        private async void ShowCustomDialogCommandExecute()
        {
            await _messageDialogManager.ShowDialogAsync(new AboutViewModel());
        }
    }
}

```

## License

Copyright © 2020 Evaristo Cuesta 

**MessageDialogManagerLib** is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/evaristocuesta/MessageDialogManagerLib/blob/master/LICENSE).

## Credits

This project uses [Mahapps.Metro](https://mahapps.com/) and [FolderBrowserEx](https://github.com/evaristocuesta/FolderBrowserEx).

