using System.Collections.ObjectModel;
using CookMaster_Project.Models;

namespace CookMaster_Project.Managers
{
    public class RecipeManager : IRecipeService
    {
        private readonly ObservableCollection<Recipe> _recipes;

        // Use the same collection as UserManagers to avoid duplicate data.
        public RecipeManager(UserManagers userManagers)
            : this(userManagers.Recipes)
        {
        }

        public RecipeManager(ObservableCollection<Recipe> sharedRecipes)
        {
            _recipes = sharedRecipes;
        }

        public ObservableCollection<Recipe> Recipes => _recipes;

        public void AddRecipe(Recipe recipe) => _recipes.Add(recipe);

        public void RemoveRecipe(Recipe recipe)
        {
            if (_recipes.Contains(recipe))
            {
                _recipes.Remove(recipe);
            }
        }
    }
}



//public RecipeManager(UserManagers userManagers): this(userManagers.Recipes){ }

//A delegate constructor of the same class, using this(...) to call another constructor in the RecipeManager class.
//public RecipeManager(UserManagers userManagers) is an overload that takes UserManagers.
// : this(userManagers.Recipes) is passed to another constructor that takes ObservableCollection<Recipe> , causing the RecipeManager to use the same Recipes collection as the UserManagers.
// Result: Adding / removing recipes to the RecipeManager is immediately reflected in the UserManagers (single source of truth), and the UI bound to that collection is automatically updated in WPF.