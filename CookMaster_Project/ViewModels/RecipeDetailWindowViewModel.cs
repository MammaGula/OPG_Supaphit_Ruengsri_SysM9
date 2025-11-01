using CookMaster_Project.Managers;
using CookMaster_Project.Models;
using CookMaster_Project.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookMaster_Project.ViewModels
{
    public class RecipeDetailWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManagers;
        private readonly IRecipeService _recipeService;
        private readonly Recipe _original; // Keep reference to the original recipe object

        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _ingredients = string.Empty;
        private string _instructions = string.Empty;
        private string _selectedType = string.Empty;
        private string _timeMinutesText = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isEditing;

        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
        public string Ingredients { get => _ingredients; set { _ingredients = value; OnPropertyChanged(); } }
        public string Instructions { get => _instructions; set { _instructions = value; OnPropertyChanged(); } }
        public string SelectedType { get => _selectedType; set { _selectedType = value; OnPropertyChanged(); } }
        public string TimeMinutesText { get => _timeMinutesText; set { _timeMinutesText = value; OnPropertyChanged(); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        public List<string> Types { get; } = new() { "Dessert", "Main Course", "Appetizer" };

        // Read-only metadata labels
        public string CreatedByLabel => _original?.CreatedBy ?? string.Empty;
        public string CreatedDateLabel => _original?.CreatedDate.ToString("yyyy-MM-dd HH:mm") ?? string.Empty;

        public bool CanEdit
        {
            get
            {
                var u = _userManagers.GetLoggedInUser();
                if (u == null) return false;

                if (u.IsAdmin) return true;
                return string.Equals(_original.CreatedBy, u.Username, StringComparison.OrdinalIgnoreCase);
            }
        }

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CopyAsNewCommand { get; }


        // Constructor
        public RecipeDetailWindowViewModel(UserManagers userManagers, IRecipeService recipeService, Recipe selected)
        {
            _userManagers = userManagers;
            _recipeService = recipeService;
            _original = selected;

            // Initialize fields from the original recipe
            Title = selected.Title;
            Description = selected.Description;
            Ingredients = selected.Ingredients;
            Instructions = selected.Instructions;
            SelectedType = selected.Type;
            TimeMinutesText = selected.TimeMinutes.ToString();
            IsEditing = false; // inputs are locked by default

            EditCommand = new RelayCommand(_ => StartEdit());
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => CloseWindow());
            CopyAsNewCommand = new RelayCommand(_ => CopyAsNew());
        }

        // Start editing mode
        private void StartEdit()
        {
            if (!CanEdit)
            {
                MessageBox.Show("You don't have permission to edit this recipe.", "Permission", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            IsEditing = true;
        }


        //  Validate and save changes to the original recipe object
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorMessage = "Title is required.";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedType))
            {
                ErrorMessage = "Type is required.";
                return;
            }

            if (!int.TryParse(TimeMinutesText, out var minutes) || minutes < 0)
            {
                ErrorMessage = "Time (minutes) must be a non-negative number.";
                return;
            }

            // Overwrite original recipe values
            _original.Title = Title.Trim();
            _original.Description = (Description ?? string.Empty).Trim();
            _original.Ingredients = (Ingredients ?? string.Empty).Trim();
            _original.Instructions = (Instructions ?? string.Empty).Trim();
            _original.Type = SelectedType;
            _original.TimeMinutes = minutes;
            // CreatedBy and CreatedDate remain unchanged

            MessageBox.Show("Recipe updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Just close this detail dialog. Let the caller (list VM) refresh its view.
            CloseWindow();
        }


        // Create a new recipe by copying the current one
        private void CopyAsNew()
        {
            //Retrieves the currently logged in user, if no user is logged in, set createdBy to "Guest".
            var currentUser = _userManagers.GetLoggedInUser();
            var createdBy = currentUser?.Username ?? "Guest";

            if (!int.TryParse(TimeMinutesText, out var minutes) || minutes < 0)
            {
                MessageBox.Show("Time (minutes) must be a non-negative number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Create a new Recipe object as a clone of the original with modifications
            var clone = new Recipe
            {
                Title = string.IsNullOrWhiteSpace(Title) ? $"{_original.Title} - Copy" : $"{Title.Trim()} - Copy",
                Description = (Description ?? string.Empty).Trim(),
                Ingredients = (Ingredients ?? string.Empty).Trim(),
                Instructions = (Instructions ?? string.Empty).Trim(),
                Type = SelectedType,
                TimeMinutes = minutes,
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now
            };

            // Save the cloned recipe via IRecipeService
            _recipeService.AddRecipe(clone);

            MessageBox.Show("Recipe copied as new.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void CloseWindow()
        {
            Application.Current.Windows
                ?.OfType<Window>()
                ?.FirstOrDefault(w => ReferenceEquals(w.DataContext, this))
                ?.Close();
        }
    }
}