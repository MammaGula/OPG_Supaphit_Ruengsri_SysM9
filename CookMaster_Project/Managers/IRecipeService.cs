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


//IRecipeService for easy decoupling and testing.
// The Recipe has clear service usage from multiple ViewModels
// (AddRecipeWindowViewModel, RecipeDetailWindowViewModel, RecipeListWindowViewModel)