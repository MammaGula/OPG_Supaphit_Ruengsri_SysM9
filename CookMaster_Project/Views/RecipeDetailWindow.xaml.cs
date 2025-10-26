using System;
using System.Windows;
using CookMaster_Project.Managers;
using CookMaster_Project.Models;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for RecipeDetailWindow.xaml
    /// </summary>
    public partial class RecipeDetailWindow : Window
    {
        private readonly IRecipeService _recipeService;
        private readonly Recipe _boundRecipe;

        // Constructor with dependency injection
        public RecipeDetailWindow(Recipe selectedRecipe, IRecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;
            _boundRecipe = selectedRecipe ?? throw new System.ArgumentNullException(nameof(selectedRecipe));

            // Bind to DataContext to allow XAML access to Recipe properties.
            DataContext = _boundRecipe;
        }
    }
}

