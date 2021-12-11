using System.Windows;
using LopushokApp.Model;

namespace LopushokApp.Views.Windows
{
    public partial class MainWindow : Window
    {
        readonly LopushokDatabaseModel _database = new LopushokDatabaseModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ApplicationExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenProducts(object sender, RoutedEventArgs e)
        {
            ProductsWindow window = new ProductsWindow
            {
                Database = _database
            };

            this.Hide();

            if(window.ShowDialog() == false)
            {
                this.Show();
            }
        }
    }
}
