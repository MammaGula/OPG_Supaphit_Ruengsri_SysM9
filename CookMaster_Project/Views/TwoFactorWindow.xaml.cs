using CookMaster_Project.Managers;
using CookMaster_Project.ViewModels;
using System.Windows;

namespace CookMaster_Project.Views
{
    public partial class TwoFactorWindow : Window
    {
        public TwoFactorWindow(UserManagers userManagers, string username)
        {
            InitializeComponent();
            DataContext = new TwoFactorWindowViewModel(userManagers, username);
        }
    }
}