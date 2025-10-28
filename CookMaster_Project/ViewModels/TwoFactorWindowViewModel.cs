using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using System.CodeDom.Compiler;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModels
{
    public class TwoFactorWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManagers;
        private readonly string _username;

        private string _code = string.Empty;
        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand VerifyCommand { get; }
        public ICommand CancelCommand { get; }

        public TwoFactorWindowViewModel(UserManagers userManagers, string username)
        {
            _userManagers = userManagers;
            _username = username;

            //RelayCommand binds a method to be executed (Execute) and a condition that can be pressed (CanExecute).
            //VerifyCommand contains Execute and CanExecute.
            //CancelCommand, which only has Execute, can be pressed at any time.
            VerifyCommand = new RelayCommand(_ => Verify(), _ => CanVerify());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private bool CanVerify()
        {
            return !string.IsNullOrWhiteSpace(Code) && Code.Trim().Length == 6;
        }


        
        private void Verify()
        {
            ErrorMessage = string.Empty;

            if (_userManagers.ValidateTwoFactorCode(_username, (Code ?? string.Empty).Trim()))
            {
                var wnd = Application.Current.Windows.OfType<TwoFactorWindow>().FirstOrDefault(w => ReferenceEquals(w.DataContext, this));
                if (wnd != null)
                {
                    wnd.DialogResult = true;
                    wnd.Close();
                }
            }
            else
            {
                ErrorMessage = "Invalid verification code.";
                MessageBox.Show(ErrorMessage, "Verification Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel()
        {
            var wnd = Application.Current.Windows.OfType<TwoFactorWindow>().FirstOrDefault(w => ReferenceEquals(w.DataContext, this));
            if (wnd != null)
            {
                wnd.DialogResult = false;
                wnd.Close();
            }
        }
    }
}