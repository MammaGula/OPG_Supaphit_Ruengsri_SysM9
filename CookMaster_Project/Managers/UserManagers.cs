using CookMaster_Project.Models; // Import the User and AdminUser models
using CookMaster_Project.MVVM; // Import BaseViewModel
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; // Import for RandomNumberGenerator


// ViewModel to manage user-related operations 
namespace CookMaster_Project.Managers
{
    public class UserManagers : BaseViewModel
    {
        private readonly List<User> _users = new(); // List to store all users, Only UserManagers can access it

        public User? LoggedIn { get; private set; } // Public property to access the logged-in user, only UserManagers can set it
        public bool IsLoggedIn => LoggedIn != null;


        // Use as a central data source (real CRUD)
        public ObservableCollection<Recipe> Recipes { get; } = new();


        // Temporary code storage per username (case-insensitive)      
        private readonly Dictionary<string, string> _twoFactorCodes = new(StringComparer.OrdinalIgnoreCase);



        // Constructor to initialize with some default users
        public UserManagers()
        {
            // Add default admin and user accounts
            _users.Add(new AdminUser
            {
                Username = "admin",
                Password = "password",
                Country = "Thailand",
                SecurityQuestion = "What is the Admin PIN?",
                SecurityAnswer = "1234"
            });

            _users.Add(new User
            {
                Username = "user",
                Password = "password",
                Country = "Sweden",
                SecurityQuestion = "What is your lucky number",
                SecurityAnswer = "5678",
                IsAdmin = false
            });

            // Initialize with some default recipes
            Recipes.Add(new Recipe
            {
                Title = "Pancake",
                Description = "Sweet pancake",
                Ingredients = "Flour, Egg, Milk",
                Instructions = "Mix & fry",
                Type = "Dessert",
                TimeMinutes = 20,
                CreatedBy = "admin"
            });

            Recipes.Add(new Recipe
            {
                Title = "Fried rice",
                Description = "Easy and healthy meal",
                Ingredients = "Rice, Garlic, Sugar, Eggs, Soy sauce, vegetables, meat",
                Instructions = "Chop & Fry",
                Type = "Main Course",
                TimeMinutes = 10,
                CreatedBy = "user"
            });
        }



        // Find the user with the given username and password, return null if not found any matching                                     
        public bool Login(string username, string password, out string message)
        {
            // Find the user in the _users list.
            var user = _users.FirstOrDefault(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
            if (user == null)
            {
                message = "Invalid username or password";
                return false;
            }


            LoggedIn = user; //We found a matching user, set as logged in
            OnPropertyChanged(nameof(LoggedIn));
            OnPropertyChanged(nameof(IsLoggedIn));

            message = "Log in successful!";
            return true;
        }



        // Generate 6-digit code for provided username
        public string GenerateTwoFactorCode(string username)
        {
            int num = RandomNumberGenerator.GetInt32(0, 1_000_000);
            string code = num.ToString("D6");
            _twoFactorCodes[username] = code;
            return code;
        }

        // Validate code and consume it on success
        public bool ValidateTwoFactorCode(string username, string code)
        {
            if (_twoFactorCodes.TryGetValue(username, out var stored) && string.Equals(stored, code, StringComparison.Ordinal))
            {
                _twoFactorCodes.Remove(username);
                return true;
            }
            return false;
        }



        // Register a new user
        public bool Register(string username, string password, string country, string securityQuestion, string securityAnswer, out string message)
        {
            // Check if username already exists(LINQ)
            if (_users.Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)))
            {
                message = "Username is not available";
                return false;
            }

            _users.Add(new User
            {
                Username = username,
                Password = password,
                Country = country,
                SecurityQuestion = securityQuestion,
                SecurityAnswer = securityAnswer,
                IsAdmin = false
            });

            message = "Registered";
            return true;
        }


        // Check if any user in List users
        public bool FindUser(string username)
        {
            return _users.Any(user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));
        }


        // Change password method
        public bool ChangePassword(string username, string securityAnswer, string newPassword, out string message)
        {
            // Find user by username
            var user = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                message = "User not found";
                return false;
            }

            // Check security answer
            if (!string.Equals(user.SecurityAnswer, securityAnswer, StringComparison.OrdinalIgnoreCase))
            {
                message = "Incorrect security answer.";
                return false;
            }

            // Update password
            user.Password = newPassword;
            message = "Password changed successfully.";
            return true;
        }



        // Get security questions by Username
        public string? GetSecurityQuestion(string username)
        {
            var user = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            return user?.SecurityQuestion;
        }

        public User? GetLoggedInUser() => LoggedIn;



        // Logout method
        public void Logout()
        {
            LoggedIn = null;
            OnPropertyChanged(nameof(LoggedIn));
            OnPropertyChanged(nameof(IsLoggedIn));
        }


        // CRUD for Recipe
        public void AddRecipe(Recipe recipe) => Recipes.Add(recipe);

        public void RemoveRecipe(Recipe recipe)
        {
            // Admin can remove any recipe
            if (LoggedIn is AdminUser)
            {
                if (recipe != null && Recipes.Contains(recipe))
                {
                    Recipes.Remove(recipe);
                }
                return;
            }



            // Regular users can only remove their own recipes
            if (LoggedIn != null &&
                recipe != null &&
                string.Equals(recipe.CreatedBy, LoggedIn.Username, StringComparison.OrdinalIgnoreCase) &&
                Recipes.Contains(recipe))
            {
                Recipes.Remove(recipe);
            }
        }


        // View all recipes (admin-specific functionality)
        public IEnumerable<Recipe> ViewAllRecipes()
        {
            if (LoggedIn is AdminUser)
            {
                return Recipes;
            }

            // Regular users can only view their own recipes
            return Recipes.Where(r => string.Equals(r.CreatedBy, LoggedIn?.Username, StringComparison.OrdinalIgnoreCase));
        }
    }
}


//If the system is likely to add additional administrative functions in the future,
//such as user management, activity monitoring, or permissions management,
//an AdminManager should be added to separate responsibilities and support system expansion.

//Currently, the system has simple functionality and there are no plans to add new functions,
//so I think this is sufficient.