using CookMaster_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CookMaster_Project.MVVM;


//ViewModel för att hantera användare
namespace CookMaster_Project.Managers
{
    public class UserManagers : BaseViewModel
    {

        private List<User> _users = new();
        private User? _loggedIn;

        public User? LoggedIn { get; private set; }

        public UserManagers()
        {           
            _users.Add(new User { Username = "admin", Password = "password", Country = "Thailand" });
            _users.Add(new User { Username = "user", Password = "password", Country = "Sweden"});
        }

       

        // Find the user with the given username and password, return null if not found any matching                                     
        public bool Login(string username, string password, out string message)
        {
            User? user = _users.FirstOrDefault(u => !string.Equals(u.Username == username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            if (user == null)
            {
                message = "Invalid username or password";
                return false;
            }

            LoggedIn = user;
            message = "Log in successful!";
            return true;
        }

        public bool Register(string username, string password, string country, string securityAnswer, out string message)
        {
            // Check if username already exists
            if (_users.Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)))
            {
                message = "Username is invalid";
                return false; 
            }

            User newUser = new User
            {
                Username = username,
                Password = password,
                Country = country,
                SecurityAnswer = securityAnswer,
            };
            _users.Add(newUser);
            message = "Registered";
            return true;
        }


        public bool FindUser(string username)
        {
            return _users.Any(user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));
        }

        public bool ChangePassword(string username, string password, string securityAnswer, string newPassword, out string message)
        {
            // Find the user with the given username and password
            var user = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)
                                                  && u.Password == password);

            if (user == null)
            {
                message = "Invalid username or password.";
                return false;
            }

            // Validate the security answer
            // if (u.SecurityAnswer != securityAnswer)

            if (!string.Equals(user.SecurityAnswer, securityAnswer, StringComparison.OrdinalIgnoreCase))
            {
                message = "Incorrect security answer.";
                return false;
            }

            // Update the password
            user.Password = newPassword;
            message = "Password changed successfully.";
            return true;
        }


        // Method to get the currently logged-in user
        public User? GetLoggedInUser() => _loggedIn;
        //public List<User> GetAllUsers() => _users;

        public void Logout()
        {
            LoggedIn = null;
        }

    }
}


