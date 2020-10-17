using System.Windows.Input;

namespace MessageDialogManagerLib
{
    public interface IDialogViewModel
    {
        ICommand CloseCommand { get; set; }
    }
}
