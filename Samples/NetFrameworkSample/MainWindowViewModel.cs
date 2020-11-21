using CommandLibrary;
using MessageDialogManagerLib;
using System.Windows.Input;

namespace NetFrameworkSample
{
    public class MainWindowViewModel
    {
        private IMessageDialogManager _messageDialogManager;

        public MainWindowViewModel(IMessageDialogManager messageDialogManager)
        {
            _messageDialogManager = messageDialogManager;
            ShowFolderBrowserCommand = new Command(ShowFolderBrowserCommandExecute, ShowFolderBrowserCommandCanExecute);
            ShowFileBrowserCommand = new Command(ShowFileBrowserCommandExecute, ShowFileBrowserCommandCanExecute);
        }

        public ICommand ShowFolderBrowserCommand { get; private set; }

        public ICommand ShowFileBrowserCommand { get; private set; }

        private bool ShowFolderBrowserCommandCanExecute()
        {
            return true;
        }

        private void ShowFolderBrowserCommandExecute()
        {
            if (_messageDialogManager.ShowFolderBrowser("Select a folder", ""))
                _messageDialogManager.ShowInfoDialogAsync("Folder Selected", _messageDialogManager.FolderPath);
        }

        private bool ShowFileBrowserCommandCanExecute()
        {
            return true;
        }

        private void ShowFileBrowserCommandExecute()
        {
            if (_messageDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*"))
                _messageDialogManager.ShowInfoDialogAsync("File Selected", _messageDialogManager.FilePath);
        }
    }
}
