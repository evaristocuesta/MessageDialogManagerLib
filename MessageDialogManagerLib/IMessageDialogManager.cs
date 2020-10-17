using System.Threading.Tasks;

namespace MessageDialogManagerLib
{
    public interface IMessageDialogManager
    {
        bool ShowFolderBrowser(string title, string initialPath);
        string FolderPath { get; set; }
        bool ShowFileBrowser(string title, string initialPath, string filter);
        string FilePath { get; set; }
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string title, string message);
        Task ShowProgress(string title, string message);
        void UpdateProgress(double progress);
        void UpdateMessageProgress(string message);
        Task CloseProgress();
        Task ShowDialogAsync(IDialogViewModel viewModel);
    }
}
