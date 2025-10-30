using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModels
{
    public class RegisterWindowViewModel : BaseViewModel
    {
        // Fields
        private readonly UserManagers _userManager;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _selectedCountry = string.Empty;
        private string _securityQuestion = string.Empty;
        private string _securityAnswer = string.Empty;
        private string _errorMessage = string.Empty;

        // Properties
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
                //Forces CommandManager to recheck if the command can be executed.
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SecurityQuestion
        {
            get => _securityQuestion;
            set
            {
                _securityQuestion = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SecurityAnswer
        {
            get => _securityAnswer;
            set
            {
                _securityAnswer = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public List<string> Countries { get; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland" };

        // Commands
        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor
        public RegisterWindowViewModel(UserManagers userManager)
        {
            _userManager = userManager;

            RegisterCommand = new RelayCommand(
                execute => Register(),
                canExecute => CanRegister());

            CancelCommand = new RelayCommand(
                execute => Cancel());
        }

        // Methods
        private bool CanRegister() =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            !string.IsNullOrWhiteSpace(SelectedCountry) &&
            !string.IsNullOrWhiteSpace(SecurityQuestion) &&
            !string.IsNullOrWhiteSpace(SecurityAnswer) &&
            Password.Length >= 8 &&

            // Check if password contains at least one digit and one special character
            Password.Any(char.IsDigit) &&
            Password.Any(ch => !char.IsLetterOrDigit(ch)) &&
            Password == ConfirmPassword;

        private void Register()
        {
            // Clear old error messages 
            ErrorMessage = string.Empty;

            var username = Username?.Trim() ?? string.Empty;

            // Check if username is already taken
            if (_userManager.FindUser(username))
            {
                ErrorMessage = "Username is already taken.";
                return;
            }

            // Register new user
            if (_userManager.Register(username, Password, SelectedCountry, SecurityQuestion, SecurityAnswer, out string message))
            {
                MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                //// Open MainWindow first and set it as the current MainWindow.
                var newMain = new MainWindow();
                Application.Current.MainWindow = newMain;
                newMain.Show();

                // Searches for any open RegisterWindow windows in the application,
                // and if it finds one, it closes it by calling the Close() method.
                var registerWindow = Application.Current.Windows.OfType<RegisterWindow>().FirstOrDefault();
                registerWindow?.Close();


            }
            else
            {
                // Registration failed
                ErrorMessage = message;
            }
        }

        private void Cancel()
        {
            // Prepare new MainWindow and set it as current MainWindow BEFORE closing RegisterWindow
            var main = new MainWindow();
            Application.Current.MainWindow = main;
            main.Show();

            // Find RegisterWindow close the RegisterWindow
            var registerWindow = Application.Current.Windows
                .OfType<RegisterWindow>()
                .FirstOrDefault();

            registerWindow?.Close();
        }
    }
}