using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.MVVM;
using CookMaster_Project.Services;
using CookMaster_Project.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModel
{
    public class RecipeListWindowViewModel : BaseViewModel
    {
        // Readonly: assign in constructor only
        //UserManagers: To manage data and operations related to users, ex. Login, Add/Remove Recipe, etc.
        private readonly UserManagers? _userManager;

        //To manage recipe Via IRecipeService(ex. RecipeManager)
        private readonly IRecipeService _recipeService = null!;

        //To manage window operations Via IWindowService(ex. WindowService)
        private readonly IWindowService _windowService = null!;


        // Currently selected recipe
        private Recipe? _selectedRecipe;
        private string _searchQuery = string.Empty;
        private string _selectedFilter = "All";

        // Date range + Sort
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private string _sortBy = "CreatedDate";
        private bool _sortDescending = true;

        // Favorites filter
        private bool _showOnlyFavorites;

        // All loaded recipes
        public ObservableCollection<Recipe> Recipes { get; } = new();

        //Recipes after filtering
        public ObservableCollection<Recipe> FilteredRecipes { get; } = new();

        // Filter options
        public List<string> Filters { get; } = new() { "All", "Dessert", "Main Course", "Appetizer" };

        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();
                // Update the state of the command when the selection changes.
                CommandManager.InvalidateRequerySuggested();
            }
        }




        //These properties allow users to filter and sort recipes.
        // When the values ​​of these properties change, ApplyFilters is called to update FilteredRecipes.
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

        // Sorting options
        public List<string> SortOptions { get; } = new() { "CreatedDate", "Title", "Type" };

        // Selected sorting option
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

        public bool ShowOnlyFavorites
        {
            get => _showOnlyFavorites;
            set { _showOnlyFavorites = value; OnPropertyChanged(); ApplyFilters(); }
        }




        //Check if the value returned by ?.Username is null.If null, return "Guest". Otherwise,return the value of Username.
        public string LoggedInUsername => _userManager?.GetLoggedInUser()?.Username ?? "Guest";



        public ICommand AddRecipeCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ShowInfoCommand { get; }
        public ICommand ToggleFavoriteCommand { get; } // Favorite mark/unmark support (assignment requirement)


        //Constructor
        // In case of injecting external dependencies.
        public RecipeListWindowViewModel(UserManagers userManager, IRecipeService? recipeService = null, IWindowService? windowService = null)
        {
            _userManager = userManager;
            _recipeService = recipeService ?? new RecipeManager(_userManager);
            _windowService = windowService ?? new WindowService();

            AddRecipeCommand = new RelayCommand(execute => OpenAddRecipeWindow());
            RemoveRecipeCommand = new RelayCommand(execute => RemoveRecipe()/*, canExecute => SelectedRecipe != null*/);
            ViewDetailsCommand = new RelayCommand(execute => OpenRecipeDetailsWindow()/*, canExecute => SelectedRecipe != null*/);
            OpenUserDetailsCommand = new RelayCommand(execute => OpenUserDetailsWindow());
            SignOutCommand = new RelayCommand(execute => SignOut());
            ShowInfoCommand = new RelayCommand(execute => ShowInfo());
            ToggleFavoriteCommand = new RelayCommand(execute => ToggleFavorite(), canExecute => SelectedRecipe != null);

            // Update list and displayed username when login state changes
            _userManager.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(UserManagers.LoggedIn))
                {
                    OnPropertyChanged(nameof(LoggedInUsername));
                    LoadRecipes();
                }
            };

            LoadRecipes();
        }



        //Load recipes from IRecipeService and filter by logged in user.
        // and adds them to the Recipes collection, then calls ApplyFilters to filter the data before displaying it in the UI.
        private void LoadRecipes()
        {
            Recipes.Clear();

            var currentUser = _userManager?.GetLoggedInUser();

            // Get All recipes from IRecipeService (use empty collection in case of no data)
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



        // Filter and sort recipes by user filters and preferences.
        //Use LINQ(via Where) to filter data in the Recipes collection based on specified conditions:
        //LINQ: Search, Sort, Filter and combine data
        private void ApplyFilters()
        {
            FilteredRecipes.Clear();

            var filteredRecipes = Recipes.Where(recipe =>
                (SelectedFilter == "All" || recipe.Type == SelectedFilter) &&
                (string.IsNullOrWhiteSpace(SearchQuery) || recipe.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (!FromDate.HasValue || recipe.CreatedDate.Date >= FromDate.Value.Date) &&
                (!ToDate.HasValue || recipe.CreatedDate.Date <= ToDate.Value.Date) &&
                (!ShowOnlyFavorites || recipe.IsFavorite));

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

                default:
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



        // Open the AddRecipeWindow window to allow users to add new recipes.
        // Set the Owner Window for AddRecipeWindow to maintain the window hierarchy.
        // Load a new recipe list after closing the AddRecipeWindow window.
        private void OpenAddRecipeWindow()
        {
            try
            {

                var addRecipeWindow = new AddRecipeWindow();


                // Uses the window service to show the window as a modal dialog and set ownership safely.
                _windowService.ShowDialog(addRecipeWindow, this);

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

            //// Use UserManagers to authenticate and delete from the central data source.
            _userManager?.RemoveRecipe(SelectedRecipe);
            LoadRecipes();
        }



        // Open the RecipeDetailWindow window to display the details of the user-selected recipe.
        // Set the Owner Window for the RecipeDetailWindow to maintain the window hierarchy.
        // Reload the recipe list after closing the RecipeDetailWindow window.
        private void OpenRecipeDetailsWindow()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Please select a recipe to view details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Safely select the current window's owner (don't use Application.Current.MainWindow, as it's already closed).
                var recipeDetailWindow = new RecipeDetailWindow(SelectedRecipe, _recipeService);

                _windowService.ShowDialog(recipeDetailWindow, this); // Set owner and show dialog

                // Show dialog and refresh list after it closes to reflect changes
                LoadRecipes();
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

            try
            {
                //
                var userDetailsWindow = new UserDetailsWindow(_userManager);

                // Show as modal and set a safe owner via IWindowService
                _windowService.ShowDialog(userDetailsWindow, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open User Details. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //The SignOut method logs out the current user 
        private void SignOut()
        {
            _userManager?.Logout();

            // Back to MainWindow
            var mainWindow = new MainWindow();
            mainWindow.Show();

            //Close the window where DataContext == this (supports if MainWindow is not the current page)
            _windowService.CloseWindowFor(this);
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



        // Toggle favorite marker for selected recipe, then re-apply filters to reflect ShowOnlyFavorites
        private void ToggleFavorite()
        {
            if (SelectedRecipe == null)
            {
                MessageBox.Show("Please select a recipe first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedRecipe.IsFavorite = !SelectedRecipe.IsFavorite;
            ApplyFilters();
        }
    }
}
