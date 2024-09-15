using Avalonia.Controls;
using H2_Gruppe_project.DatabaseClasses;

namespace H2_Gruppe_project.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            Database.TestConnection();
        }
    }
}