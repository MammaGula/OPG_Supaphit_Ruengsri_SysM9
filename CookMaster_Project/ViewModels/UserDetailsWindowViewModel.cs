using CookMaster_Project.Managers;
using CookMaster_Project.MVVM;
using CookMaster_Project.Services;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class UserDetailsWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManager;
        private readonly IWindowService _windowService;


        private string _username = string.Empty;
        private string _country = string.Empty;

        // New properties for current values (read-only in UI)
        public string CurrentUsername { get; private set; } = string.Empty;
        public string CurrentCountry { get; private set; } = string.Empty;

        // New input properties
        private string _newUsername = string.Empty;
        public string NewUsername
        {
            get => _newUsername;
            set { _newUsername = value; OnPropertyChanged(); }
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        private string _selectedCountry = string.Empty;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set { _selectedCountry = value; OnPropertyChanged(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        // Reuse country list style from RegisterWindow
        public List<string> Countries { get; } = new() { "Sweden", "Norway", "Denmark", "Finland" };


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



        // Constructor: Dependency injection of UserManagers
        public UserDetailsWindowViewModel(UserManagers userManager, IWindowService? windowService = null)
        {
            //Retrieve the information of the currently logged -in user.
            //Set the Username and Country values ??of the ViewModel to match the currently logged -in user.
            _userManager = userManager;
            _windowService = windowService ?? new WindowService();

            var user = _userManager.GetLoggedInUser();
            if (user != null)
            {

                Username = user.Username;
                Country = user.Country;

                // new fields for UI
                CurrentUsername = user.Username;
                CurrentCountry = user.Country;
                SelectedCountry = user.Country;

                // By default we leave NewUsername empty so user clearly knows it's optional to change.
                // NewPassword/ConfirmPassword remain empty until user provides as well.
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
            ErrorMessage = string.Empty;

            var user = _userManager.GetLoggedInUser();
            if (user == null)
            {
                MessageBox.Show("No logged in user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWindow();
                return;
            }

            // Validate New Username if provided
            var trimmedNewUsername = (NewUsername ?? string.Empty).Trim();
            bool wantsUsernameChange =
                !string.IsNullOrWhiteSpace(trimmedNewUsername) &&
                !string.Equals(trimmedNewUsername, CurrentUsername, StringComparison.OrdinalIgnoreCase);

            if (wantsUsernameChange)
            {
                if (trimmedNewUsername.Length < 3)
                {
                    ErrorMessage = "Username must be at least 3 characters.";
                    MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // If different from current, ensure not taken
                if (_userManager.FindUser(trimmedNewUsername))
                {
                    ErrorMessage = "Username is already taken.";
                    MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }



            // Validate Password if provided
            var newPwd = NewPassword ?? string.Empty;
            var confirmPwd = ConfirmPassword ?? string.Empty;
            bool wantsPasswordChange = !string.IsNullOrEmpty(newPwd) || !string.IsNullOrEmpty(confirmPwd);

            if (wantsPasswordChange)
            {
                if (newPwd.Length < 5)
                {
                    ErrorMessage = "Password must be at least 5 characters.";
                    MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!string.Equals(newPwd, confirmPwd, StringComparison.Ordinal))
                {
                    ErrorMessage = "New password and confirm password do not match.";
                    MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }



            // Validate Country selection (should have a value)
            if (string.IsNullOrWhiteSpace(SelectedCountry))
            {
                ErrorMessage = "Please select a country.";
                MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool hasAnyChange =
                wantsUsernameChange ||
                wantsPasswordChange ||
                !string.Equals(SelectedCountry, CurrentCountry, StringComparison.Ordinal);

            if (!hasAnyChange)
            {
                // Not mandatory, but improves UX
                MessageBox.Show("No changes to save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            // Apply changes
            string oldUsername = user.Username;

            if (wantsUsernameChange)
            {
                user.Username = trimmedNewUsername;
                _userManager.LoggedIn!.Username = trimmedNewUsername;

                // Keep list integrity: update CreatedBy for all recipes belonging to this user
                foreach (var recipe in _userManager.Recipes.Where(r => string.Equals(r.CreatedBy, oldUsername, StringComparison.OrdinalIgnoreCase)))
                {
                    recipe.CreatedBy = trimmedNewUsername;
                }
            }

            if (!string.Equals(SelectedCountry, user.Country, StringComparison.Ordinal))
            {
                user.Country = SelectedCountry;
                _userManager.LoggedIn!.Country = SelectedCountry;
            }

            if (wantsPasswordChange)
            {
                user.Password = newPwd;
            }



            // Refresh current display values
            CurrentUsername = user.Username;
            CurrentCountry = user.Country;
            OnPropertyChanged(nameof(CurrentUsername));
            OnPropertyChanged(nameof(CurrentCountry));

            MessageBox.Show("User details updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseWindow();
        }

        // Cancel changes and close the window
        private void Cancel()
        {
            CloseWindow(); // Simply close without saving
        }


        // Close the associated window
        private void CloseWindow()
        {
            _windowService.CloseWindowFor(this);
        }
    }
}