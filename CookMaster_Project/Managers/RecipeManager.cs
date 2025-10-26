using System.Collections.ObjectModel;
using CookMaster_Project.Models;

namespace CookMaster_Project.Managers
{
    public class RecipeManager : IRecipeService
    {
        private readonly ObservableCollection<Recipe> _recipes;

        // ใช้คอลเลกชันเดียวกับ UserManagers เพื่อไม่ให้ข้อมูลซ้ำ
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