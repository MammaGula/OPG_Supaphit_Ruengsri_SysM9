using CookMaster_Project.Managers; // Fixes CS0246: Ensure correct namespace is used
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookMaster_Project.Views
{
    /// <summary>
    /// Interaction logic for ForgetPasswordWindow.xaml
    /// </summary>
    public partial class ForgetPasswordWindow : Window
    {
        private readonly UserManagers userManager; // Add 'readonly' for clarity

        public ForgetPasswordWindow(UserManagers userManager)
        {
            InitializeComponent();
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
    }
}
