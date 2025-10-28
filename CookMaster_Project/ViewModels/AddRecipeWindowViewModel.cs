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
    public class AddRecipeWindowViewModel : BaseViewModel
    {
        private readonly UserManagers _userManagers;
        private readonly IRecipeService _recipeService;

        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _ingredients = string.Empty;
        private string _instructions = string.Empty;
        private string _selectedType = string.Empty;
        private string _timeMinutesText = string.Empty;
        private string _errorMessage = string.Empty;
        private DateTime? _selectedDate = DateTime.Today;

        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
        public string Ingredients { get => _ingredients; set { _ingredients = value; OnPropertyChanged(); } }
        public string Instructions { get => _instructions; set { _instructions = value; OnPropertyChanged(); } }
        public string SelectedType { get => _selectedType; set { _selectedType = value; OnPropertyChanged(); } }
        public string TimeMinutesText { get => _timeMinutesText; set { _timeMinutesText = value; OnPropertyChanged(); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        public DateTime? SelectedDate { get => _selectedDate; set { _selectedDate = value; OnPropertyChanged(); } }

        public List<string> Types { get; } = new() { "Dessert", "Main Course", "Appetizer" };

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddRecipeWindowViewModel(UserManagers userManagers, IRecipeService recipeService)
        {
            _userManagers = userManagers;
            _recipeService = recipeService;

            SaveCommand = new RelayCommand(execute => Save());
            CancelCommand = new RelayCommand(execute => CloseWindow());
        }

        // Show a warning popup when validation fails.
        private void Save()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorMessage = "Title is required.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedType))
            {
                ErrorMessage = "Type is required.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                ErrorMessage = "Description is required.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Ingredients))
            {
                ErrorMessage = "Ingredients are required.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Instructions))
            {
                ErrorMessage = "Instructions are required.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TimeMinutesText, out var minutes) || minutes < 0)
            {
                ErrorMessage = "Time (minutes) must be a non-negative number.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!SelectedDate.HasValue)
            {
                ErrorMessage = "Date is required.";
                MessageBox.Show(ErrorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _userManagers.GetLoggedInUser();
            var createdBy = user?.Username ?? "Guest";

            var recipe = new Recipe
            {
                Title = Title.Trim(),
                Description = (Description ?? string.Empty).Trim(),
                Ingredients = (Ingredients ?? string.Empty).Trim(),
                Instructions = (Instructions ?? string.Empty).Trim(),
                Type = SelectedType,
                TimeMinutes = minutes,
                CreatedBy = createdBy,
                CreatedDate = SelectedDate.Value.Date
            };

            _recipeService.AddRecipe(recipe);
            MessageBox.Show("Recipe added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows
                ?.OfType<Window>()
                ?.FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}