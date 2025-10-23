using CookMaster_Project.Managers;
using CookMaster_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {

        //Constructor that accepts UserManagers instance
        public RegisterWindow(UserManagers userManager)
        {
            InitializeComponent();
            DataContext = new RegisterWindowViewModel(userManager); 
        }


        //update the password in the PasswordBox (View)to the Password prop. in ViewModel
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
           
            if (DataContext is RegisterWindowViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }


        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterWindowViewModel viewModel)
            {
                viewModel.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
