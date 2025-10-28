using CookMaster_Project.Managers;
using CookMaster_Project.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        public UserDetailsWindow(UserManagers userManager)
        {
            InitializeComponent();
            DataContext = new UserDetailsWindowViewModel(userManager);
        }

        // Sync PasswordBox values to ViewModel 
        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsWindowViewModel vm && sender is PasswordBox pb)
            {
                vm.NewPassword = pb.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserDetailsWindowViewModel vm && sender is PasswordBox pb)
            {
                vm.ConfirmPassword = pb.Password;
            }
        }
    }
}