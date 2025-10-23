using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModels
{
    public class RegisterWindowViewModel : INotifyPropertyChanged
    {
        //Fields
        private readonly UserManagers _userManager;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _selectedCountry = string.Empty;
        private string _errorMessage = string.Empty;

        //Properties
        public string Username{ get => _username; set { _username = value; OnPropertyChanged(); }}
        public string Password{ get => _password; set { _password = value; OnPropertyChanged(); }}
        public string ConfirmPassword{ get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); }}
        public string SelectedCountry{ get => _selectedCountry; set { _selectedCountry = value; OnPropertyChanged(); }}
        public string ErrorMessage{get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); }}

        // List of countries for selection
        public List<string> Countries { get; } = new List<string> { "Sweden", "Norway", "Denmark", "Finland" };


        // Commands
        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }


        // Constructor
        public RegisterWindowViewModel(UserManagers userManager)
        {
            _userManager = userManager;
            RegisterCommand = new RelayCommand(execute => Register(), canExecute => CanRegister());
            CancelCommand = new RelayCommand(execute => Cancel());
        }


        // Validation for registration
        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   !string.IsNullOrWhiteSpace(SelectedCountry) &&
                   Password.Length >= 8 &&
                   Password.Any(char.IsDigit) &&
                   Password.Any(ch => !char.IsLetterOrDigit(ch)) &&
                   Password == ConfirmPassword;
        }


        // Registration logic
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
                Application.Current.Windows[1]?.Close(); // Close RegisterWindow
            }
            else
            {
                // Registration failed
                ErrorMessage = message;
            }
        }

        private void Cancel()
        {
            // Close the RegisterWindow
            Application.Current.Windows[1]?.Close(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}