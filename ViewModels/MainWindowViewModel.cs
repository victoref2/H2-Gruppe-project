using H2_Gruppe_project.DatabaseClasses;
using H2_Gruppe_project.Classes;
using System;

namespace H2_Gruppe_project.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; }

        public MainWindowViewModel()
        {
            Greeting = TestBusDatabaseOperations();
        }

        private string TestBusDatabaseOperations()
        {
            try
            {
                Database db = new Database();

                // Test AddBus
                Bus bus = new Bus(
                    "1", "Mercedes Benz", "50000", "XY12345", "2020", true, "10.5", 14.0m, "diesel", "Class A",
                    30000, 2, 4.5m, 15000m, 12.0m, 50, 20, true
                );
                db.AddBus(bus); 

                Bus retrievedBus = db.GetBusById("1");
                string result = "Bus added to database.\n";
                if (retrievedBus != null)
                {
                    result += retrievedBus.ToString() + "\n";
                }
                else
                {
                    result += "Bus not found.\n";
                }

                db.DeleteBus("1");
                result += "Bus deleted from database.";

                return result;
            }
            catch (Exception ex)
            {
                return $"Error during database operations: {ex.Message}";
            }
        }
    }
}
