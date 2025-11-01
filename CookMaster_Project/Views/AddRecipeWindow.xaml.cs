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
        // Constructor for Design-time support, no parameters, can create instance
        public AddRecipeWindow()
        {
            InitializeComponent();


            var userManager = (UserManagers)Application.Current.Resources["UserManagers"];
            var recipeService = new RecipeManager(userManager);
            DataContext = new AddRecipeWindowViewModel(userManager, recipeService);
        }



        // Constructor for Run-time with dependency injection
        public AddRecipeWindow(UserManagers userManager, IRecipeService recipeService)
        {
            InitializeComponent();
            DataContext = new AddRecipeWindowViewModel(userManager, recipeService);
        }
    }
}

