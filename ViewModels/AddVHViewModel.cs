using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using System;
using H2_Gruppe_project.Classes;
using Avalonia.Controls;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

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

        [ObservableProperty]
        private bool isNormalVHVisible;


        // Command for going back to dashboard
        [RelayCommand]
        public void GoBackCancel()
        {
            _mainViewModel.SwitchViewModel(new DashboardViewModel(_mainViewModel, _loggedInUser, _database));
        }

        [ObservableProperty]
        private bool isCommercialVH;

        // Visibility properties for private and commercial vehicles
        [ObservableProperty]
        private bool isCommercialVisible;

        [ObservableProperty]
        private bool isPrivateVisible;

        partial void OnIsCommercialVHChanged(bool value)
        {
            // Set visibility based on the checkbox
            IsCommercialVisible = value;
            IsPrivateVisible = !value;
        }


        public List<string> VehicleTypes { get; } = new List<string> { "Truck", "Bus", "Normal Vehicle" };

        partial void OnSelectedVehicleTypeChanged(string value)
        {
            // Reset all visibilities
            IsTruckVisible = false;
            IsBusVisible = false;
            IsHeavyVehicleVisible = false;
            IsNormalVHVisible = false;

            IsCommercialVisible = false;
            IsCommercialVH = false;
            IsPrivateVisible = false;

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
            else if (value == "Normal Vehicle")
            {
                IsNormalVHVisible = true;
                IsPrivateVisible = true;
            }
        }
    }
}