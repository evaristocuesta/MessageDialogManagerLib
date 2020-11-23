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
            ShowFolderBrowserCommand = new Command(ShowFolderBrowserCommandExecute, ShowFolderBrowserCommandCanExecute);
            ShowFileBrowserCommand = new Command(ShowFileBrowserCommandExecute, ShowFileBrowserCommandCanExecute);
            ShowInfoDialogCommand = new Command(ShowInfoDialogCommandExecute, ShowInfoDialogCommandCanExecute);
            ShowOkCancelDialogCommand = new Command(ShowOkCancelDialogCommandExecute, ShowOkCancelDialogCommandCanExecute);
            ShowProgressCommand = new Command(ShowProgressCommandExecute, ShowProgressCommandCanExecute);
            ShowCustomDialogCommand = new Command(ShowCustomDialogCommandExecute, ShowCustomDialogCommandCanExecute);
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
        }

        private bool ShowFileBrowserCommandCanExecute()
        {
            return true;
        }

        private async void ShowFileBrowserCommandExecute()
        {
            if (_messageDialogManager.ShowFileBrowser("Select a file", "", "*.* | *.*"))
                await _messageDialogManager.ShowInfoDialogAsync("File Selected", _messageDialogManager.FilePath);
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
