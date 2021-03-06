﻿using CommandLibrary;
using MessageDialogManagerLib;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetFrameworkSample.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly IMessageDialogManager _messageDialogManager;

        public MainWindowViewModel(IMessageDialogManager messageDialogManager)
        {
            _messageDialogManager = messageDialogManager;
            ShowFolderBrowserSingleCommand = new Command(ShowFolderBrowserSingleCommandExecute, ShowFolderBrowserSingleCommandCanExecute);
            ShowFileBrowserSingleCommand = new Command(ShowFileBrowserSingleCommandExecute, ShowFileBrowserSingleCommandCanExecute);
            ShowFolderBrowserMultipleCommand = new Command(ShowFolderBrowserMultipleCommandExecute, ShowFolderBrowserMultipleCommandCanExecute);
            ShowFileBrowserMultipleCommand = new Command(ShowFileBrowserMultipleCommandExecute, ShowFileBrowserMultipleCommandCanExecute);
            ShowInfoDialogCommand = new Command(ShowInfoDialogCommandExecute, ShowInfoDialogCommandCanExecute);
            ShowOkCancelDialogCommand = new Command(ShowOkCancelDialogCommandExecute, ShowOkCancelDialogCommandCanExecute);
            ShowProgressCommand = new Command(ShowProgressCommandExecute, ShowProgressCommandCanExecute);
            ShowCustomDialogCommand = new Command(ShowCustomDialogCommandExecute, ShowCustomDialogCommandCanExecute);
            ShowSaveFileDialogCommand = new Command(ShowSaveFileDialogCommandExecute, ShowSaveFileDialogCommandCanExecute);
        }

        public ICommand ShowFolderBrowserSingleCommand { get; private set; }

        public ICommand ShowFileBrowserSingleCommand { get; private set; }

        public ICommand ShowFolderBrowserMultipleCommand { get; private set; }

        public ICommand ShowFileBrowserMultipleCommand { get; private set; }

        public ICommand ShowInfoDialogCommand { get; private set; }

        public ICommand ShowOkCancelDialogCommand { get; private set; }

        public ICommand ShowProgressCommand { get; private set; }

        public ICommand ShowCustomDialogCommand { get; private set; }

        public ICommand ShowSaveFileDialogCommand { get; private set; }

        private bool ShowFolderBrowserSingleCommandCanExecute()
        {
            return true;
        }

        private async void ShowFolderBrowserSingleCommandExecute()
        {
            if (_messageDialogManager.ShowFolderBrowser("Select a folder", ""))
                await _messageDialogManager.ShowInfoDialogAsync("Folder Selected", _messageDialogManager.FolderPath);
            else
                await _messageDialogManager.ShowInfoDialogAsync("No folder has been selected", _messageDialogManager.FolderPath);
        }

        private bool ShowFileBrowserSingleCommandCanExecute()
        {
            return true;
        }

        private async void ShowFileBrowserSingleCommandExecute()
        {
            if (_messageDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*"))
                await _messageDialogManager.ShowInfoDialogAsync("File Selected", _messageDialogManager.FilePath);
            else
                await _messageDialogManager.ShowInfoDialogAsync("No file has been selected", _messageDialogManager.FilePath);
        }

        //---------------------------------------------------------------------------------------------
        private bool ShowFolderBrowserMultipleCommandCanExecute()
        {
            return true;
        }

        private async void ShowFolderBrowserMultipleCommandExecute()
        {
            if (_messageDialogManager.ShowFolderBrowser("Select a folder", "", true))
            {
                string selected = string.Empty;
                foreach (var folder in _messageDialogManager.FolderPaths)
                {
                    selected += $"{folder}\n";
                }
                await _messageDialogManager.ShowInfoDialogAsync("Folders Selected", selected);
            }
            else
                await _messageDialogManager.ShowInfoDialogAsync("No folder has been selected", _messageDialogManager.FolderPath);
        }

        private bool ShowFileBrowserMultipleCommandCanExecute()
        {
            return true;
        }

        private async void ShowFileBrowserMultipleCommandExecute()
        {
            if (_messageDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*", true))
            {
                string selected = string.Empty;
                foreach (var file in _messageDialogManager.FilePaths)
                {
                    selected += $"{file}\n";
                }
                await _messageDialogManager.ShowInfoDialogAsync("File Selected", selected);
            }
            else
                await _messageDialogManager.ShowInfoDialogAsync("No file has been selected", _messageDialogManager.FilePath);
        }
        //---------------------------------------------------------------------------------------------

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

        private bool ShowSaveFileDialogCommandCanExecute()
        {
            return true;
        }

        private async void ShowSaveFileDialogCommandExecute()
        {
            if (_messageDialogManager.ShowSaveFileDialog("Save a file", "", "fileName", ".txt", "Text documents (.txt)|*.txt"))
                await _messageDialogManager.ShowInfoDialogAsync("File Selected to Save", _messageDialogManager.FilePathToSave);
            else
                await _messageDialogManager.ShowInfoDialogAsync("No file has been selected to save", _messageDialogManager.FilePathToSave);
        }
    }
}
