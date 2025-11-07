using CookMaster_Project.Managers;
using CookMaster_Project.ViewModels;
using System.Windows;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for AddRecipeWindow.xaml
    /// </summary>
    public partial class AddRecipeWindow : Window
    {

        // Constructor 
        public AddRecipeWindow()
        {
            InitializeComponent();
            var userManager = (UserManagers)Application.Current.Resources["UserManagers"];
            var recipeService = new RecipeManager(userManager);

            // Dependency injection 
            DataContext = new AddRecipeWindowViewModel(userManager, recipeService);
        }

    }
}

