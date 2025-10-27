using System;
using System.Collections.Generic;
using System.Linq;

namespace CookMaster_Project.Models
{
    // AdminUser inherits from User and adds admin-specific functionality
    public class AdminUser : User
    {
        public AdminUser()
        {
            IsAdmin = true; // Ensure IsAdmin is always true for AdminUser
        }



        // Admin can remove any recipe from the collection
        public bool RemoveRecipe(ICollection<Recipe> recipes, Recipe recipe)
        {
            if (recipes == null) throw new ArgumentNullException(nameof(recipes));
            if (recipe == null) return false;

            if (recipes.Contains(recipe))
            {
                return recipes.Remove(recipe);
            }
            return false;
        }



        // Admin can view all recipes in the collection
        public IEnumerable<Recipe> ViewAllRecipes(IEnumerable<Recipe> recipes)
        {
            if (recipes == null) throw new ArgumentNullException(nameof(recipes));
            return recipes;
        }
    }
}