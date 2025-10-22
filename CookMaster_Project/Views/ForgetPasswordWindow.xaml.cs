using CookMaster_Project.Managers; 
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

// Ensure the namespace matches your project structure
namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for ForgetPasswordWindow.xaml
    /// </summary>
    public partial class ForgetPasswordWindow : Window
    {
        private readonly UserManagers userManager;

        public ForgetPasswordWindow(UserManagers userManager)
        {
            InitializeComponent();

            // Store the reference to UserManagers
            this.userManager = userManager;
        }

        // Event handler for the Reset Password button click
        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string securityAnswer = SecurityAnswerTextBox.Text;
            string newPassword = NewPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(securityAnswer) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Attempt to change the password linked to UserManagers.cs
            bool success = userManager.ChangePassword(username, securityAnswer, newPassword, out string message);

            MessageBox.Show(message);

            if (success)
            {
                this.Close();
            }
        }
    }
}
