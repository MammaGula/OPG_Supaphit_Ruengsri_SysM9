using CookMaster_Project.Managers;// Import UserManagers
using CookMaster_Project.MVVM;// Import RelayCommand
using CookMaster_Project.Services; // Import WindowService / IWindowService
using CookMaster_Project.Views;// Import RegisterWindow
using System.Windows; // Import Application, MessageBox, Window
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Fields
        private readonly UserManagers _userManager;
        private readonly IWindowService _windowService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _error = string.Empty;


        // Properties biding to the View
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string Error { get => _error; set { _error = value; OnPropertyChanged(); } }


        // Commands that the View can bind to
        public ICommand LoginCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand OpenRegisterCommand { get; }


        // Constructor: Dependency Injection of UserManagers (+ optional window service)
        public MainWindowViewModel(UserManagers userManager, IWindowService? windowService = null)
        {
            _userManager = userManager;
            _windowService = windowService ?? new WindowService();

            LoginCommand = new RelayCommand(execute => Login(), canExecute => CanLogin());
            OpenRegisterCommand = new RelayCommand(execute => OpenRegisterWindow());
            ForgotPasswordCommand = new RelayCommand(execute => ForgotPassword());
        }


        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);



        // Perform login and handle two-factor authentication
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

            // Show 2FA dialog (use window service to resolve owner safely)
            var twoFactorWindow = new TwoFactorWindow(_userManager, Username);
            var owner = _windowService.GetOwner(this);
            if (owner != null)
                twoFactorWindow.Owner = owner;

            var result = twoFactorWindow.ShowDialog();
            if (result != true)
            {
                MessageBox.Show("Two-factor verification failed or canceled.", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(msg, "Login completed", MessageBoxButton.OK);

            // Create and show the RecipeListWindow
            var recipeListWindow = new RecipeListWindow();

            // Set it to a new MainWindow to reduce the problem of owner pointing to a closed window.
            Application.Current.MainWindow = recipeListWindow;

            recipeListWindow.Show();

            // Close old MainWindow via window service (DataContext == this)
            _windowService.CloseWindowFor(this);
        }



        // Open the Forgot Password Window
        private void ForgotPassword()
        {
            var forgetPasswordWindow = new ForgetPasswordWindow(_userManager);

            // Resolve owner through WindowService instead of direct MainWindow usage
            var owner = _windowService.GetOwner(this);
            if (owner != null)
                forgetPasswordWindow.Owner = owner;

            forgetPasswordWindow.ShowDialog();
        }



        // Open the Registration Window
        //Passing _userManager allows the registration window to access user management functions,
        //such as adding new users or verifying user information.
        private void OpenRegisterWindow()
        {
            //Open RegisterWindow first so as not to close MainWindow immediately
            var registerWindow = new RegisterWindow(_userManager);

            // Make RegisterWindow the current MainWindow to avoid app shutdown
            Application.Current.MainWindow = registerWindow;

            registerWindow.Show();

            // Close the current MainWindow using window service (DataContext == this)
            _windowService.CloseWindowFor(this);
        }

    }
}