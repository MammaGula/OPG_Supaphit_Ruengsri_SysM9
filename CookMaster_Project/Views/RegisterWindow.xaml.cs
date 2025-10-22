using CookMaster_Project.Managers;
using System.Text.RegularExpressions;
using System.Windows;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly UserManagers _userManager;

        public RegisterWindow(UserManagers userManager)
        {
            InitializeComponent();
            _userManager = userManager;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string country = CountryComboBox.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(country))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsPasswordValid(password))
            {
                MessageBox.Show("Password must be at least 8 characters long, contain at least one number, and one special character.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Attempt to register the user
            if (_userManager.Register(username, password, country, string.Empty, out string message))
            {
                MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close(); // Close RegisterWindow
            }
            else
            {
                MessageBox.Show(message, "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"\d") && // At least one digit
                   Regex.IsMatch(password, @"[^\w\d\s]"); // At least one special character
        }
    }
}
