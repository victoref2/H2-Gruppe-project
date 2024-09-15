using System;

namespace H2_Gruppe_project.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; }

        public MainWindowViewModel()
        {
            Greeting = TestDatabaseConnection();
        }

        private string TestDatabaseConnection()
        {
            try
            {
                DatabaseClasses.Database.TestConnection();
                return "Connection to the database was successful!";
            }
            catch (Exception ex)
            {
                return $"Failed to connect to the database: {ex.Message}";
            }
        }
    }
}
