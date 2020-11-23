using MessageDialogManagerLib;
using System.Windows.Input;

namespace NetFrameworkSample.ViewModel
{
    public class AboutViewModel : IDialogViewModel

    {
        public AboutViewModel()

        {

        }

        public ICommand CloseCommand { get; set; }

    }
}
