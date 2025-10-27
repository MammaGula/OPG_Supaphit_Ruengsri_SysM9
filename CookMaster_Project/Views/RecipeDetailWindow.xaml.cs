using System;
using System.Windows;
using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.ViewModels;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for RecipeDetailWindow.xaml
    /// </summary>
    public partial class RecipeDetailWindow : Window
    {
        // Constructor with dependency injection
        public RecipeDetailWindow(Recipe selectedRecipe, IRecipeService recipeService)
        {
            InitializeComponent();

            // Pull shared UserManagers from App resources to keep one central data source
            var userManagers = (UserManagers)Application.Current.Resources["UserManagers"];

            // Bind to DataContext. Pass both services.
            DataContext = new RecipeDetailWindowViewModel(userManagers, recipeService, selectedRecipe);
        }

        // Optional parameterless constructor for design-time support
        public RecipeDetailWindow()
        {
            InitializeComponent();

            if (Application.Current?.Resources["UserManagers"] is UserManagers userManagers)
            {
                var sample = new Recipe
                {
                    Title = "Sample",
                    Description = "Sample desc",
                    Ingredients = "A, B, C",
                    Instructions = "Do X then Y",
                    Type = "Dessert",
                    TimeMinutes = 15,
                    CreatedBy = "admin"
                };

                // Create a service for design-time
                var recipeService = new RecipeManager(userManagers);
                DataContext = new RecipeDetailWindowViewModel(userManagers, recipeService, sample);
            }
        }
    }
}



