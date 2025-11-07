using CookMaster_Project.Models;

namespace CookMaster_Project.Services
{
    public interface IUserSeed
    {
        IEnumerable<User> GetInitialUsers();
        IEnumerable<Recipe> GetInitialRecipes();
    }
}