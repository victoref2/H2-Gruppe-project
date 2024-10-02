using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using System;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AddVHViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly User _loggedInUser;

        private readonly Database _database;

        

        public AddVHViewModel(MainWindowViewModel mainViewModel, User loggedInUser, Database database)
        {
            _mainViewModel = mainViewModel;
            _loggedInUser = loggedInUser;
            _database = database;

        }

        // ComboBox selection binding
        [ObservableProperty]
        private string selectedVehicleType;

        // Visibility properties for various vehicle sections
        [ObservableProperty]
        private bool isTruckVisible;

        [ObservableProperty]
        private bool isBusVisible;

        [ObservableProperty]
        private bool isHeavyVehicleVisible;

        // Command for going back to dashboard
        [RelayCommand]
        public void GoBackCancel()
        {
            _mainViewModel.SwitchViewModel(new DashboardViewModel(_mainViewModel, _loggedInUser, _database));
        }

        // Method to handle ComboBox selection changes
        partial void OnSelectedVehicleTypeChanged(string value)
        {
            // Reset all visibilities
            IsTruckVisible = false;
            IsBusVisible = false;
            IsHeavyVehicleVisible = false;

            // Set visibility based on the selected vehicle type
            if (value == "Truck")
            {
                IsTruckVisible = true;
                IsHeavyVehicleVisible = true;
            }
            else if (value == "Bus")
            {
                IsHeavyVehicleVisible = true;
                IsBusVisible = true;
            }
        }
    }
}