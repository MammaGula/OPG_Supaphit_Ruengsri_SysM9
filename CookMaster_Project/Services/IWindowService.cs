using System.Windows;

namespace CookMaster_Project.Services
{
    public interface IWindowService
    {
        Window? GetOwner(object? dataContext);
        void ShowDialog(Window window, object? ownerDataContext);
        void CloseWindowFor(object viewModel);
    }
}