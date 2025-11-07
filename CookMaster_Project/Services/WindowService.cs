using System.Windows;

namespace CookMaster_Project.Services
{
    public class WindowService : IWindowService
    {
        // Gets the owner window based on the provided DataContext.
        public Window? GetOwner(object? dataContext)
        {
            var windows = Application.Current?.Windows?.OfType<Window>();
            if (windows is null) return null;

            // First try exact DataContext match, otherwise first active window.
            return windows.FirstOrDefault(w => ReferenceEquals(w.DataContext, dataContext))
                   ?? windows.FirstOrDefault(w => w.IsActive);
        }


        // Shows a dialog window with the specified owner based on DataContext.
        public void ShowDialog(Window window, object? ownerDataContext)
        {
            var owner = GetOwner(ownerDataContext);
            if (owner is not null && owner.IsLoaded)
                window.Owner = owner;

            window.ShowDialog();
        }


        // Closes the window associated with the specified view model.
        public void CloseWindowFor(object viewModel)
        {
            var win = Application.Current?.Windows?
                .OfType<Window>()
                .FirstOrDefault(w => ReferenceEquals(w.DataContext, viewModel));
            win?.Close();
        }
    }
}