using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.ViewModels;
using System.Windows;
namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for RecipeDetailWindow.xaml
    /// </summary>
    public partial class RecipeDetailWindow : Window
    {

        // Constructor with dependency injection, for Run-time use
        public RecipeDetailWindow(Recipe selectedRecipe, IRecipeService recipeService)
        {
            InitializeComponent();

            // Pull shared UserManagers from App resources to keep one central data source
            var userManagers = (UserManagers)Application.Current.Resources["UserManagers"];

            // Bind to DataContext. Pass both services(Dependency Injection)
            DataContext = new RecipeDetailWindowViewModel(userManagers, recipeService, selectedRecipe);
        }

    }
}






