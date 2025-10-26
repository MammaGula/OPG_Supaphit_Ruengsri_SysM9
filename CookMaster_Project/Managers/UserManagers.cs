using CookMaster_Project.Models; // Import the User model
using CookMaster_Project.MVVM; // Import BaseViewModel
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//ViewModel to manage user-related operations 
namespace CookMaster_Project.Managers
{
    public class UserManagers : BaseViewModel
    {
        private List<User> _users = new();
        private User? _loggedIn;

        public User? LoggedIn { get; private set; }

        // Use as a central data source (real CRUD)
        public ObservableCollection<Recipe> Recipes { get; } = new();

        //public IEnumerable<object> Recipes { get; internal set; } = new List<object>();
        ////an interface used for iterating through data such as lists or arrays.
        ////Recipes used only for reading recipe information, Prevents users of this property from being able to directly add or delete items in the list.
        ////save memory and improve performance in case of large data size.



        // Constructor to initialize with some default users
        public UserManagers()
        {
            _users.Add(new User { Username = "admin", Password = "password", Country = "Thailand", SecurityQuestion = "What is the Admin PIN?", SecurityAnswer = "1234", IsAdmin = true });
            _users.Add(new User { Username = "user", Password = "password", Country = "Sweden", SecurityQuestion = "What is your lucky number", SecurityAnswer = "5678", IsAdmin = false });


            // Initialize with some default recipes
            Recipes.Add(new Recipe { Title = "Pancake", Description = "Sweet pancake", Ingredients = "Flour, Egg, Milk", Instructions = "Mix & fry", Type = "Dessert", TimeMinutes = 20, CreatedBy = "admin" });
            Recipes.Add(new Recipe { Title = "fried rice", Description = "Easy and helthy meal", Ingredients = "Rice, Garlic, Sugar, Eggs, Soya suace, vegetables, meat", Instructions = "Chop & Fry", Type = "Appetizer", TimeMinutes = 10, CreatedBy = "user" });
        }


        // Find the user with the given username and password, return null if not found any matching                                     
        public bool Login(string username, string password, out string message)
        {
            // Find the user in the _users list.
            User? user = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            if (user == null)
            {
                message = "Invalid username or password";
                return false;
            }

            LoggedIn = user;
            _loggedIn = user;
            message = "Log in successful!";
            return true;
        }


        // Register a new user
        public bool Register(string username, string password, string country, string securityQuestion, string securityAnswer, out string message)
        {
            // Check if username already exists
            // Any: Check if there are users in the _users list, ​​if there is at least one user , return true; otherwise, return false.
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



        public User? GetLoggedInUser() => _loggedIn;
        //public List<User> GetAllUsers;



        // Logout method
        public void Logout()
        {
            LoggedIn = null;// Bug fix: Clear real state
        }


        // CRUD for Recipe
        public void AddRecipe(Recipe recipe) => Recipes.Add(recipe);
        public void RemoveRecipe(Recipe recipe)
        {
            if (Recipes.Contains(recipe)) Recipes.Remove(recipe);
        }

    }
}