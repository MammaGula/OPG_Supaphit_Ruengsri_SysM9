using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.MVVM;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class UserDetailsWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManager;
        private string _username = string.Empty;
        private string _country = string.Empty;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        // Constructor
        public UserDetailsWindowViewModel(UserManagers userManager)
        {
            //Retrieve the information of the currently logged -in user.
            //Set the Username and Country values ??of the ViewModel to match the currently logged -in user.
           
            _userManager = userManager;

            var user = _userManager.GetLoggedInUser();
            if (user != null)
            {
                Username = user.Username;
                Country = user.Country;
            }

            // Initialize commands 
            SaveCommand = new RelayCommand(execute => SaveChanges());
            CancelCommand = new RelayCommand(execute => Cancel());
        }


        //checks and saves changes to the data again as we go along,
        //displaying a notification when an event occurs or when the research is complete,
        //and closing the window after the process is complete.
        private void SaveChanges()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Country))
            {
                MessageBox.Show("Username and Country cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _userManager.GetLoggedInUser();
            if (user != null)
            {
                user.Username = Username;
                user.Country = Country;
                _userManager.LoggedIn.Username = Username;
	               _userManager.LoggedIn.Country = Country;
                MessageBox.Show("User details updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            CloseWindow();
        }


        // Cancel changes and close the window
        private void Cancel()
        {
            CloseWindow();
        }


        
        //Close the window associated with the current ViewModel.
        //It searches for a window in Application.Current.Windows
        //and checks if its DataContext matches the ViewModel.
        //If a matching window is found, it calls Close() to close that window.
        private void CloseWindow()
        {
            Application.Current.Windows
                ?.OfType<Window>()
                ?.FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}