using CookMaster_Project.Managers;// Import UserManagers
using CookMaster_Project.MVVM;// Import RelayCommand
using CookMaster_Project.Views;// Import RegisterWindow
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace CookMaster_Project.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Fields
        private readonly UserManagers _userManager;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _error = string.Empty;


        // Properties
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string Error { get => _error; set { _error = value; OnPropertyChanged(); } }


        // Commands that the View can bind to
        public ICommand LoginCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand OpenRegisterCommand { get; }


        // Constructor
        public MainWindowViewModel(UserManagers userManager)
        {
            _userManager = userManager;
            LoginCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            OpenRegisterCommand = new RelayCommand(execute => OpenRegisterWindow());
            ForgotPasswordCommand = new RelayCommand(execute => ForgotPassword());
        }


        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);



        private void Login()
        {
            if (!_userManager.Login(Username, Password, out string msg))
            {
                MessageBox.Show(msg, "Login failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Generate and "send" simulated code
            var code = _userManager.GenerateTwoFactorCode(Username);
            MessageBox.Show($"Simulation: Your verification code is {code}.", "Two-Factor Code", MessageBoxButton.OK, MessageBoxImage.Information);

            // Show 2FA dialog
            var twoFactorWindow = new TwoFactorWindow(_userManager, Username)
            {
                Owner = Application.Current.MainWindow
            };
            var result = twoFactorWindow.ShowDialog();
            if (result != true)
            {
                MessageBox.Show("Two-factor verification failed or canceled.", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(msg, "Login completed", MessageBoxButton.OK);

            // Create and show the RecipeListWindow
            RecipeListWindow recipeListWindow = new RecipeListWindow();

            // Set it to a new MainWindow to reduce the problem of owner pointing to a closed window.
            Application.Current.MainWindow = recipeListWindow;

            recipeListWindow.Show();

            // Close old MainWindow 
            var oldMain = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MainWindow);
            oldMain?.Close();
        }




        // Open the ForgotPassword Window as dialog
        //Create instance of ForgetPasswordWindow by passing _userManager to constructor
        //Make the ForgetPasswordWindow window the main window by using Application.Current.MainWindow
        //so that new windows that are opened are in the context of the main window.
        private void ForgotPassword()
        {

            CookMaster_Project.Views.ForgetPasswordWindow forgetPasswordWindow = new CookMaster_Project.Views.ForgetPasswordWindow(_userManager)
            {
                Owner = Application.Current.MainWindow
            };

            forgetPasswordWindow.ShowDialog();
        }



        // Open the Registration Window
        //Passing _userManager allows the registration window to access user management functions,
        //such as adding new users or verifying user information.
        private void OpenRegisterWindow()
        {
            //Open RegisterWindow first so as not to close MainWindow immediately
            var registerWindow = new RegisterWindow(_userManager);
            registerWindow.Show();

            // Close the current MainWindow
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MainWindow)?
                .Close();
        }


        // Events
        public new event PropertyChangedEventHandler? PropertyChanged;
        private new void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
