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
        public AddRecipeWindow()
        {
            InitializeComponent();


            var userManager = (UserManagers)Application.Current.Resources["UserManagers"];
            var recipeService = new RecipeManager(userManager);
            DataContext = new AddRecipeWindowViewModel(userManager, recipeService);
        }


        public AddRecipeWindow(UserManagers userManager, IRecipeService recipeService)
        {
            InitializeComponent();
            DataContext = new AddRecipeWindowViewModel(userManager, recipeService);
        }
    }
}

