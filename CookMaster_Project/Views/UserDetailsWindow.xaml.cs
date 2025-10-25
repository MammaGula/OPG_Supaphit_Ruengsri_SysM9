using CookMaster_Project.Managers;
using CookMaster_Project.ViewModel;
using System.Windows;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        public UserDetailsWindow(UserManagers userManager)
        {
            InitializeComponent();
            DataContext = new UserDetailsWindowViewModel(userManager);
        }
    }
}
