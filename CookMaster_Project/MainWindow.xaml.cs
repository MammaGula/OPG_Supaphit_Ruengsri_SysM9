using CookMaster_Project.Managers;
using CookMaster_Project.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookMaster_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserManagers userManager = new UserManagers(); // Create an instance of UserManagers
            DataContext = new MainWindowViewModel(userManager); // Pass it to the ViewModel constructor

        }


        // Update ViewModel's Password property when PasswordBox content changes
        private void Pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.Password = Pwd.Password;
        }
    }
}