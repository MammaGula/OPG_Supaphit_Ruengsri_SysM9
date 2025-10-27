using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.MVVM;
using CookMaster_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class RecipeListWindowViewModel : BaseViewModel
    {
        private readonly UserManagers? _userManager;
        private readonly IRecipeService _recipeService = null!; // Add null-forgiving operator

        private Recipe? _selectedRecipe;
        private string _searchQuery = string.Empty;
        private string _selectedFilter = "All";

        // Date range + Sort
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private string _sortBy = "CreatedDate";
        private bool _sortDescending = true;

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


        // Public properties for date filtering and sorting
        public DateTime? FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public List<string> SortOptions { get; } = new() { "CreatedDate", "Title", "Type" };

        public string SortBy
        {
            get => _sortBy;
            set { _sortBy = value; OnPropertyChanged(); ApplyFilters(); }
        }

        public bool SortDescending
        {
            get => _sortDescending;
            set { _sortDescending = value; OnPropertyChanged(); ApplyFilters(); }
        }



        //Check if the value returned by ?.Username is null.If null, return "Guest". Otherwise,return the value of Username.
        public string LoggedInUsername => _userManager?.GetLoggedInUser()?.Username ?? "Guest";

        public ICommand AddRecipeCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ShowInfoCommand { get; }



        // In case of injecting external dependencies.
        public RecipeListWindowViewModel(UserManagers userManager, IRecipeService? recipeService = null)
        {
            _userManager = userManager;
            _recipeService = recipeService ?? new RecipeManager(_userManager);

            AddRecipeCommand = new RelayCommand(execute => OpenAddRecipeWindow());
            RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe(), canExecute => SelectedRecipe != null);
            ViewDetailsCommand = new RelayCommand(execute => OpenRecipeDetailsWindow(), canExecute => SelectedRecipe != null);
            OpenUserDetailsCommand = new RelayCommand(execute => OpenUserDetailsWindow());
            SignOutCommand = new RelayCommand(execute => SignOut());
            ShowInfoCommand = new RelayCommand(execute => ShowInfo());

            LoadRecipes();
        }



        // In case of being created from XAML/design: pulling UserManagers from App.Resources.
        public RecipeListWindowViewModel()
        {
            if (Application.Current?.Resources["UserManagers"] is UserManagers userManagersFromResources)
            {
                _userManager = userManagersFromResources;
                _recipeService = new RecipeManager(_userManager);

                AddRecipeCommand = new RelayCommand(execute => OpenAddRecipeWindow());
                RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe(), canExecute => SelectedRecipe != null);
                ViewDetailsCommand = new RelayCommand(execute => OpenRecipeDetailsWindow(), canExecute => SelectedRecipe != null);
                OpenUserDetailsCommand = new RelayCommand(execute => OpenUserDetailsWindow());
                SignOutCommand = new RelayCommand(execute => SignOut());
                ShowInfoCommand = new RelayCommand(execute => ShowInfo());

                LoadRecipes();
            }
            else
            {
                // fallback design-time
                // In case of being created from XAML/design: pulling UserManagers from App.Resources.
                AddRecipeCommand = new RelayCommand(execute => { });
                RemoveRecipeCommand = new RelayCommand(execute => { }, canExecute => false);
                ViewDetailsCommand = new RelayCommand(execute => { }, canExecute => false);
                OpenUserDetailsCommand = new RelayCommand(execute => { });
                SignOutCommand = new RelayCommand(execute => { });
                ShowInfoCommand = new RelayCommand(execute => { });
            }
        }



        // The LoadRecipes method loads recipes from a data source (in this case, UserManagers)
        // and adds them to the Recipes collection, then calls ApplyFilters to filter the data before displaying it in the UI.
        private void LoadRecipes()
        {
            Recipes.Clear();

            var currentUser = _userManager?.GetLoggedInUser();
            var recipeSource = _recipeService?.Recipes?.AsEnumerable() ?? Enumerable.Empty<Recipe>();

            // Admin sees all; Regular users only see their own.
            if (currentUser != null && !currentUser.IsAdmin)
            {
                recipeSource = recipeSource.Where(recipe =>
                    string.Equals(recipe.CreatedBy, currentUser.Username, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var recipe in recipeSource)
            {
                Recipes.Add(recipe);
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

            var filteredRecipes = Recipes.Where(recipe =>
                (SelectedFilter == "All" || recipe.Type == SelectedFilter) &&
                (string.IsNullOrWhiteSpace(SearchQuery) || recipe.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (!FromDate.HasValue || recipe.CreatedDate.Date >= FromDate.Value.Date) &&
                (!ToDate.HasValue || recipe.CreatedDate.Date <= ToDate.Value.Date));

            //  Sorting
            IEnumerable<Recipe> orderedRecipes = filteredRecipes;
            switch (SortBy)
            {
                case "Title":
                    orderedRecipes = SortDescending
                        ? filteredRecipes.OrderByDescending(recipe => recipe.Title)
                        : filteredRecipes.OrderBy(recipe => recipe.Title);
                    break;
                case "Type":
                    orderedRecipes = SortDescending
                        ? filteredRecipes.OrderByDescending(recipe => recipe.Type)
                        : filteredRecipes.OrderBy(recipe => recipe.Type);
                    break;
                default: // "CreatedDate"
                    orderedRecipes = SortDescending
                        ? filteredRecipes.OrderByDescending(recipe => recipe.CreatedDate)
                        : filteredRecipes.OrderBy(recipe => recipe.CreatedDate);
                    break;
            }

            foreach (var recipe in orderedRecipes)
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
            try
            {
                // Find the correct owner: the window holding this DataContext == ViewModel , or if none is found, use the Active window.
                var ownerWindow =
                    Application.Current?.Windows?.OfType<Window>()?.FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
                    ?? Application.Current?.Windows?.OfType<Window>()?.FirstOrDefault(window => window.IsActive);

                var addRecipeWindow = new AddRecipeWindow();

                // Set Owner only if the owner is still open to avoid throwing exceptions.
                if (ownerWindow != null && ownerWindow.IsLoaded)
                {
                    addRecipeWindow.Owner = ownerWindow;
                }

                addRecipeWindow.ShowDialog();
                LoadRecipes();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Failed to open Add Recipe window. {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

            // Delete through the central data source and reload to make the UI/FILTER consistent.
            _recipeService.RemoveRecipe(SelectedRecipe);
            LoadRecipes();
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

            try
            {
                //// Safely select the current window's owner (don't use Application.Current.MainWindow, as it's already closed).
                var ownerWindow =
                    Application.Current?.Windows?.OfType<Window>()?.FirstOrDefault(window => ReferenceEquals(window.DataContext, this))
                    ?? Application.Current?.Windows?.OfType<Window>()?.FirstOrDefault(window => window.IsActive);

                var recipeDetailWindow = new RecipeDetailWindow(SelectedRecipe, _recipeService);

                if (ownerWindow != null && ownerWindow.IsLoaded)
                {
                    recipeDetailWindow.Owner = ownerWindow;
                }

                recipeDetailWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open recipe details. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //The OpenUserDetailsWindow method opens a new window (UserDetailsWindow)
        private void OpenUserDetailsWindow()
        {
            if (_userManager == null)
            {
                MessageBox.Show("User manager is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var userDetailsWindow = new UserDetailsWindow(_userManager)
            {
                Owner = Application.Current.MainWindow
            };
            userDetailsWindow.ShowDialog();
        }



        //The SignOut method logs out the current user using the UserManagers instance
        private void SignOut()
        {
            _userManager?.Logout();

            // Back to MainWindow
            var mainWindow = new MainWindow();
            mainWindow.Show();

            //Close the window where DataContext == this (supports if MainWindow is not the current page)
            Application.Current?.Windows
                ?.OfType<Window>()
                ?.FirstOrDefault(window => window.DataContext == this)
                ?.Close();
        }



        //Displays an informational message box about the CookMaster application.
        private void ShowInfo()
        {
            MessageBox.Show(
                "Welcome to CookMaster! Your ultimate kitchen companion for whipping up delicious recipes. Whether you're a master chef or a curious foodie, CookMaster is here to spice up your cooking journey. Let's turn your kitchen into a world of flavors!",
                "About CookMaster",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
