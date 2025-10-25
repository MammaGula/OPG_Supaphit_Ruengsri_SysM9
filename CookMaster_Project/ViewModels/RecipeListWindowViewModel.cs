using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class RecipeListWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManager;
        private Recipe? _selectedRecipe;
        private string _searchQuery = string.Empty;
        private string _selectedFilter = "All";

        public ObservableCollection<Recipe> Recipes { get; } = new();
        public ObservableCollection<Recipe> FilteredRecipes { get; } = new();
        public List<string> Filters { get; } = new() { "All", "Dessert", "Main Course", "Appetizer" };

        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set { _selectedRecipe = value; OnPropertyChanged(); }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public string SelectedFilter
        {
            get => _selectedFilter;
            set { _selectedFilter = value; OnPropertyChanged(); ApplyFilters(); }
        }

        //Check if the value returned by ?.Username is null.If null, return "Guest". Otherwise,return the value of Username.
        public string LoggedInUsername => _userManager.GetLoggedInUser()?.Username ?? "Guest";

        public ICommand AddRecipeCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ShowInfoCommand { get; }


        public RecipeListWindowViewModel(UserManagers userManager)
        {
            _userManager = userManager;

            AddRecipeCommand = new RelayCommand(execute => OpenAddRecipeWindow());
            RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe(), canExecute => SelectedRecipe != null);
            ViewDetailsCommand = new RelayCommand(execute => OpenRecipeDetailsWindow(), canExecute => SelectedRecipe != null);
            OpenUserDetailsCommand = new RelayCommand(execute => OpenUserDetailsWindow());
            SignOutCommand = new RelayCommand(execute => SignOut());
            ShowInfoCommand = new RelayCommand(execute => ShowInfo());

            LoadRecipes();
        }

        public RecipeListWindowViewModel()
        {
        }

        private void LoadRecipes()
        {
            // The LoadRecipes method loads recipes from a data source (in this case, UserManagers)
            // and adds them to the Recipes collection, then calls ApplyFilters to filter the data before displaying it in the UI.

            var recipes = _userManager.Recipes; 

            foreach (var recipe in recipes)
            {
                Recipes.Add((Recipe)recipe);
            }

            ApplyFilters();
        }



        //The ApplyFilters method is used to filter recipes in Recipes by type (SelectedFilter)
        //and search term (SearchQuery), and then append the filtered results to FilteredRecipes for display in the UI.
        //Use LINQ(via Where) to filter data in the Recipes collection based on specified conditions:
        //LINQ: Search, Sort, Filter and combine data
        private void ApplyFilters()
        {
            FilteredRecipes.Clear();

            var filtered = Recipes.Where(r => (SelectedFilter == "All" || r.Type == SelectedFilter) &&
                (string.IsNullOrWhiteSpace(SearchQuery) || r.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));

            foreach (var recipe in filtered)
            {
                FilteredRecipes.Add(recipe);
            }
        }


        //Create AddRecipeWindow: User can fill in new recipe information such as the recipe name (Title), description (Description), type (Type), etc.
        //MainWindow will be the owner of AddRecipeWindow
        // Show AddRecipeWindow as a dialog 
        //After closing the AddRecipeWindow window, the LoadRecipes method is called to load a new list of recipes and refresh the UI.

        private void OpenAddRecipeWindow()
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow()
            {
                Owner = Application.Current.MainWindow
            };
            addRecipeWindow.ShowDialog();
            LoadRecipes();
        }


        //The RemoveRecipe method removes the user-selected recipe (SelectedRecipe)
        //from the Recipes collection and updates the list displayed in the UI.
        private void RemoveRecipe()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Please select a recipe to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Recipes.Remove(SelectedRecipe);
            ApplyFilters();
        }


        //The OpenRecipeDetailsWindow method opens a new window (RecipeDetailWindow)
        //It checks that a recipe is selected before running and displays the window in a Modal Dialog format.
        private void OpenRecipeDetailsWindow()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Please select a recipe to view details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            RecipeDetailWindow recipeDetailWindow = new RecipeDetailWindow(SelectedRecipe)
            {
                Owner = Application.Current.MainWindow
            };
            recipeDetailWindow.ShowDialog();
        }


        //The OpenUserDetailsWindow method opens a new window (UserDetailsWindow)
        private void OpenUserDetailsWindow()
        {
            UserDetailsWindow userDetailsWindow = new UserDetailsWindow(_userManager)
            {
                Owner = Application.Current.MainWindow
            };
            userDetailsWindow.ShowDialog();
        }


        //The SignOut method logs out the current user using the UserManagers instance
        private void SignOut()
        {
            _userManager.Logout();
            Application.Current.MainWindow?.Close();
        }


        //Displays an informational message box about the CookMaster application.
        private void ShowInfo()
        {
            MessageBox.Show("Welcome to CookMaster! Your ultimate kitchen companion for whipping up delicious recipes. Whether you're a master chef or a curious foodie, CookMaster is here to spice up your cooking journey. Let's turn your kitchen into a world of flavors!", "About CookMaster", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}