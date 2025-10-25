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
        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _selectedCountry;
        private string _errorMessage;

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
            Password.Length >= 8 &&

            // Check if password contains at least one digit and one special character
            Password.Any(char.IsDigit) &&
            Password.Any(ch => !char.IsLetterOrDigit(ch)) &&
            Password == ConfirmPassword;

        private void Register()
        {
            // Check if username is already taken
            if (_userManager.FindUser(Username))
            {
                ErrorMessage = "Username is already taken.";
                return;
            }
            // Register new user
            if (_userManager.Register(Username, Password, SelectedCountry, "DefaultAnswer", out string message))
            {
                MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

            var registerWindow = Application.Current.Windows
                .OfType<RegisterWindow>() // Find only RegisterWindow
                .FirstOrDefault();

            registerWindow?.Close(); // Close the RegisterWindow if found
        }
    }
}