using CookMaster_Project.Models;

namespace CookMaster_Project.Services
{
    public sealed class DefaultUserSeed : IUserSeed
    {

        private static readonly User[] _users =
        {
            new AdminUser
            {
                Username = "admin",
                Password = "password",
                Country = "Thailand",
                SecurityQuestion = "What is the Admin PIN?",
                SecurityAnswer = "1234"
            },
            new User
            {
                Username = "user",
                Password = "password",
                Country = "Sweden",
                SecurityQuestion = "What is your lucky number",
                SecurityAnswer = "5678",
                IsAdmin = false
            }
        };

        private static readonly Recipe[] _recipes =
        {
            new Recipe
            {
                Title = "Pancake",
                Description = "Sweet pancake",
                Ingredients = "Flour, Egg, Milk",
                Instructions = "Mix & fry",
                Type = "Dessert",
                TimeMinutes = 20,
                CreatedBy = "admin",
                CreatedDate = DateTime.UtcNow.AddDays(-10)
            },
            new Recipe
            {
                Title = "Fried rice",
                Description = "Easy and healthy meal",
                Ingredients = "Rice, Garlic, Sugar, Eggs, Soy sauce, vegetables, meat",
                Instructions = "Chop & Fry",
                Type = "Main Course",
                TimeMinutes = 10,
                CreatedBy = "user",
                CreatedDate = DateTime.UtcNow.AddDays(-10)
            },
            new Recipe
            {
                Title = "Spaghetti Cabonara",
                Description = "Creamy noodle and dangerous for belly!",
                Ingredients = "Spaghetti, Bacon, Cream, Cheese, Onion, Garlic, Olive oil, Vegetables, Salt, Pepper",
                Instructions = "Cook & Fry",
                Type = "Main Course",
                TimeMinutes = 20,
                CreatedBy = "user",
                CreatedDate = DateTime.UtcNow.AddDays(-7)
            },

            new Recipe
            {
                Title = "Tzatziki cucumber",
                Description = "Appetizer",
                Ingredients = "Greek yogurt, Cucumber, Garlic, Lemon juice, Olive oil, Salt, Black Pepper, Mint",
                Instructions = "Mix and keep it cold",
                Type = "Appetizer",
                TimeMinutes = 15-30,
                CreatedBy = "user",
                CreatedDate = DateTime.UtcNow
            }
        };

        public IEnumerable<User> GetInitialUsers() => _users;
        public IEnumerable<Recipe> GetInitialRecipes() => _recipes;
    }
}