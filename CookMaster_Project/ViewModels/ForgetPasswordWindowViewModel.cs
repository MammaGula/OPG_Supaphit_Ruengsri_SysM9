using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModels
{
    public class ForgetPasswordWindowViewModel : BaseViewModel
    {
        //Readonly: can only be assigned at declaration or in the constructor; cannot be reassigned later.
        //This keeps the dependency stable.
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

                // Load security question immediately when changing username, If GetSecurityQuestion returns null, initail value is string.Empty.
                SecurityQuestion = _userManager.GetSecurityQuestion(_username) ?? string.Empty;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // CommandManager.InvalidateRequerySuggested() : Tell WPF to update the command's state (e.g., CanExecute) when the value of a property changes.
        //In this file, properties such as Username, SecurityAnswer, NewPassword, and ConfirmPassword determine whether a command (ResetCommand) can be executed (CanReset()).


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


        // Constructor
        public ForgetPasswordWindowViewModel(UserManagers userManager)
        {
            _userManager = userManager;
            ResetCommand = new RelayCommand(execute => ResetPassword(), canExecute => CanReset());
            CancelCommand = new RelayCommand(execute => Cancel());
        }

        private bool CanReset()
        {
            bool hasUser =
                !string.IsNullOrWhiteSpace(Username) &&
                !string.IsNullOrWhiteSpace(SecurityQuestion);

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