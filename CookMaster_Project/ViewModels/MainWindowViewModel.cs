using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly UserManagers _userManager;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _error = string.Empty;

        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string Error { get => _error; set { _error = value; OnPropertyChanged(); } }

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

            MessageBox.Show(msg, "Login completed", MessageBoxButton.OK);
            return;

            // Close MainWindow
            Application.Current.MainWindow?.Close();
        }

        private void OpenRegisterWindow()
        {
            
            var registerWindow = new RegisterWindow();
            registerWindow.Show();

            //// Stäng huvudfönstret
            //Application.Current.MainWindow?.Close();
        }

  
        private void ForgotPassword()
        {
            
            CookMaster_Project.Views.ForgetPasswordWindow forgetPasswordWindow = new CookMaster_Project.Views.ForgetPasswordWindow(_userManager)
            {
                Owner = Application.Current.MainWindow
            };

            forgetPasswordWindow.ShowDialog();
        }

        public event System.EventHandler? OnLoginSuccess; 

        public event PropertyChangedEventHandler? PropertyChanged; 
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
