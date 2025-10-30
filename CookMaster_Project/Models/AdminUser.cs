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
        //ICollectoin inherits from IEnumerable with additional methods: Add(), Remove(), Contain(), Count()
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
        //IEnumerable supports iteration over a collection of a specified type and suitable for read-only access.
        public IEnumerable<Recipe> ViewAllRecipes(IEnumerable<Recipe> recipes)
        {
            if (recipes == null) throw new ArgumentNullException(nameof(recipes));
            return recipes;
        }
    }
}


//If the system is likely to add additional administrative functions in the future,
//such as user management, activity monitoring, or permissions management,
//an AdminManager should be added to separate responsibilities and support system expansion.

//Currently, the system has simple functionality and there are no plans to add new functions,
//so I think this approach is more appropriate.