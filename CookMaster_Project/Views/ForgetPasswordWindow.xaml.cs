using CookMaster_Project.Managers;
using CookMaster_Project.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CookMaster_Project.Views
{
    public partial class ForgetPasswordWindow : Window
    {
        public ForgetPasswordWindow(UserManagers userManagers)
        {
            InitializeComponent();
            DataContext = new ForgetPasswordWindowViewModel(userManagers);
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgetPasswordWindowViewModel vm)
            {
                vm.NewPassword = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgetPasswordWindowViewModel vm)
            {
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}

