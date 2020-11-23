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
        private System.Windows.Application _app;
        /// <summary>
        /// We have to use this private variable instead a local variable to show a custom dialog
        /// to avoid an error closing the dialog
        /// </summary>
        private CustomDialog _customView;
        private Dictionary<IDialogViewModel, CustomDialog> _customDialogs;
        private MetroWindow MetroWindow => (MetroWindow)_app.MainWindow;

        /// <summary>
        /// Gets the selected folder
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets the selected file
        /// </summary>
        public string FilePath { get; set; }

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
            bool res = false;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Title = title;
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
            bool res = false;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (Directory.Exists(initialPath))
                openFileDialog.InitialDirectory = initialPath;
            else
                openFileDialog.InitialDirectory = "";
            openFileDialog.Filter = filter;
            openFileDialog.Title = title;
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
                var dataTemplate = resource as DataTemplate;
                if (dataTemplate != null && dataTemplate.DataType.Equals(viewModel.GetType()))
                {
                    customView = dataTemplate.LoadContent() as CustomDialog;
                    break;
                }
            }
            return customView;
        }
    }
}
