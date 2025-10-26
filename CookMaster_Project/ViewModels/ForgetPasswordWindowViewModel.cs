using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModels
{
    public class ForgetPasswordWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManager;

        private string _username = string.Empty;
        private string _securityQuestion = string.Empty;
        private string _securityAnswer = string.Empty;
        private string _newPassword = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _errorMessage = string.Empty;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                // Load security questions immediately when changing username
                SecurityQuestion = _userManager.GetSecurityQuestion(_username) ?? "(????????????????)";
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SecurityQuestion
        {
            get => _securityQuestion;
            private set { _securityQuestion = value; OnPropertyChanged(); }
        }

        public string SecurityAnswer
        {
            get => _securityAnswer;
            set { _securityAnswer = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }

        public ForgetPasswordWindowViewModel(UserManagers userManager)
        {
            _userManager = userManager;
            ResetCommand = new RelayCommand(execute => ResetPassword(), canExecute => CanReset());
            CancelCommand = new RelayCommand(execute => Cancel());
        }

        private bool CanReset()
        {
            bool hasUser = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(SecurityQuestion) && SecurityQuestion != "(????????????????)";
            bool passOk = !string.IsNullOrWhiteSpace(NewPassword) &&
                          !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                          NewPassword.Length >= 8 &&
                          NewPassword.Any(char.IsDigit) &&
                          NewPassword.Any(ch => !char.IsLetterOrDigit(ch)) &&
                          NewPassword == ConfirmPassword;
            bool answerOk = !string.IsNullOrWhiteSpace(SecurityAnswer);
            return hasUser && passOk && answerOk;
        }

        private void ResetPassword()
        {
            if (!_userManager.ChangePassword(Username, SecurityAnswer, NewPassword, out string message))
            {
                ErrorMessage = message;
                return;
            }

            MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            var wnd = Application.Current.Windows.OfType<ForgetPasswordWindow>().FirstOrDefault();
            wnd?.Close();
        }

        private void Cancel()
        {
            var wnd = Application.Current.Windows.OfType<ForgetPasswordWindow>().FirstOrDefault();
            wnd?.Close();
        }
    }
}