using MahApps.Metro.Controls;
using MessageDialogManagerLib;
using NetFrameworkSample.ViewModel;

namespace NetFrameworkSample.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new MessageDialogManagerMahapps(App.Current));
        }
    }
}
