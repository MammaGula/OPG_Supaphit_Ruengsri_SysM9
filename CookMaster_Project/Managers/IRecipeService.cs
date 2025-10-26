using System.Collections.ObjectModel;
using CookMaster_Project.Models;

namespace CookMaster_Project.Managers
{
    public interface IRecipeService
    {
        ObservableCollection<Recipe> Recipes { get; }
        void AddRecipe(Recipe recipe);
        void RemoveRecipe(Recipe recipe);
    }
}