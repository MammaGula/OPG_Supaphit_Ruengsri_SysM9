using CookMaster_Project.Managers;
using CookMaster_Project.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {

        private readonly RegisterWindowViewModel _vm;


        //// In case of opening a window without sending UserManagers (will be pulled from App.Resources)
        //// want to share a shared status, such as a list of users (_users), LoggedIn user in UserManagers.
        //public RegisterWindow()
        //{
        //    InitializeComponent();
        //    var userManager = (UserManagers)Application.Current.Resources["UserManagers"];
        //    _vm = new RegisterWindowViewModel(userManager);
        //    DataContext = _vm;

        //}


        // In case of opening a window by passing in UserManagers 
        //If you want RegisterWindow to run in a separate context from the main application, or for testing purposes.
        public RegisterWindow(UserManagers userManager)
        {
            InitializeComponent();
            _vm = new RegisterWindowViewModel(userManager);
            DataContext = _vm;
        }


        //// Pattern matching: Check both sender and DataContext first,
        //// if sender is not PasswordBox the condition is not met and no exception is thrown.
        //private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (sender is PasswordBox pb && DataContext is RegisterWindowViewModel vm)
        //    {
        //        vm.Password = pb.Password;
        //    }
        //}



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
            if (sender is PasswordBox pb && DataContext is RegisterWindowViewModel vm)
            {
                vm.ConfirmPassword = pb.Password;
            }
        }
    }
}
